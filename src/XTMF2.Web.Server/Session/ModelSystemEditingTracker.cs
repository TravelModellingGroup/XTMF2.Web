//     Copyright 2017-2020 University of Toronto
// 
//     This file is part of XTMF2.
// 
//     XTMF2 is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     XTMF2 is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using AutoMapper;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Utils;

namespace XTMF2.Web.Server.Session
{
    /// <summary>
    ///     Tracks the editing state of a model system. Will fire events for underlying changes to the model system
    ///     and updated / free stored object references.
    /// </summary>
    public class ModelSystemEditingTracker : IDisposable
    {
        private readonly IMapper _mapper;

        /// <summary>
        ///     List of registered deligates for the tracker callback
        /// </summary>
        /// <returns></returns>
        private readonly List<EventHandler<ModelSystemChangedEventArgs>> _delegates =
            new List<EventHandler<ModelSystemChangedEventArgs>>();

        /// <summary>
        ///     Tracks GUID to model system editing references
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        public Dictionary<Guid, ViewObject> ModelSystemEditingObjectReferenceMap { get; } =
            new Dictionary<Guid, ViewObject>();

        /// <summary>
        ///     Tracks the underlying model system to editing model references
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        public Dictionary<object, ViewObject> ModelSystemObjectRefrenceMap { get; } =
            new Dictionary<object, ViewObject>();

        /// <summary>
        ///     Delegate tracker for objects subscribiding to model system changes
        /// </summary>
        /// <value></value>
        public ModelSystemEditingModel ModelSystem { get; }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewObject GetModelSystemEditingObject(Guid id)
        {
            return ModelSystemEditingObjectReferenceMap[id];
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModelSystemObject<T>(Guid id) where T : class
        {
            if (ModelSystemEditingObjectReferenceMap.ContainsKey(id))
            {
                if (ModelSystemEditingObjectReferenceMap[id].ObjectReference is T assignedReference)
                {
                    return assignedReference;
                }
            }
            return null;
        }

        /// <summary>
        ///     Attempts to return the model system object of specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="modelSystemObject"></param>
        /// <returns></returns>
        public bool TryGetModelSystemObject<T>(Guid id, out T modelSystemObject)
        {
            if (ModelSystemEditingObjectReferenceMap.ContainsKey(id))
            {
                var objectReference = ModelSystemEditingObjectReferenceMap[id].ObjectReference;
                if (objectReference is T assignedReference)
                {
                    modelSystemObject = assignedReference;
                    return true;
                }
            }

            modelSystemObject = default;
            return false;
        }

        /// <summary>
        ///     Register to track changes
        /// </summary>
        public event EventHandler<ModelSystemChangedEventArgs> OnModelSystemChanged
        {
            add
            {
                _delegates.Add(value);
                _onModelSystemChanged += value;
            }
            remove
            {
                _delegates.Remove(value);
                _onModelSystemChanged -= value;
            }
        }

        private event EventHandler<ModelSystemChangedEventArgs> _onModelSystemChanged;

        ~ModelSystemEditingTracker()
        {
            Dispose();
        }

        /// <summary>
        ///     Stores all objects in the object/view/edit model reference maps
        /// </summary>
        /// <param name="session"></param>
        /// <param name="editingModel"></param>
        private void StoreModelSystemObjectReferenceMap(ModelSystemEditingModel editingModel)
        {
            foreach (var viewObject in ModelSystemUtils.ModelSystemObjects(editingModel))
            {
                ModelSystemEditingObjectReferenceMap[viewObject.Id] = viewObject;
                ModelSystemObjectRefrenceMap[viewObject.ObjectReference] = viewObject;
                if (viewObject.ObjectReference is INotifyPropertyChanged prop)
                {
                    prop.PropertyChanged += OnModelSystemPropertyChanged;
                }

                if (viewObject.ObjectReference is Boundary boundary)
                {
                    ((INotifyCollectionChanged) boundary.Boundaries).CollectionChanged +=
                        OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged) boundary.Modules).CollectionChanged += OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged) boundary.Links).CollectionChanged += OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged) boundary.CommentBlocks).CollectionChanged +=
                        OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged) boundary.Starts).CollectionChanged += OnModelSystemCollectionChanged;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnModelSystemPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            _onModelSystemChanged?.Invoke(this, new ModelSystemChangedEventArgs(args)
            {
                EditingModelObject = ModelSystemObjectRefrenceMap[sender]
            });
        }

        /// <summary>
        ///     Called when a model system collection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnModelSystemCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in args.NewItems)
                {
                    var editingObject = (ViewObject) _mapper.Map(item, item.GetType(),
                        ModelSystemUtils.GetModelSystemEditingType(item));
                    // traverse also returns the passed item for convenience
                    foreach (var e in ModelSystemUtils.Traverse(editingObject))
                    {
                        ModelSystemEditingObjectReferenceMap[e.Id] = e;
                        ModelSystemObjectRefrenceMap[e.ObjectReference] = e;
                    }
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in args.OldItems)
                {
                    var editingObject = (ViewObject) _mapper.Map(item, item.GetType(),
                        ModelSystemUtils.GetModelSystemEditingType(item));
                    foreach (var e in ModelSystemUtils.Traverse(editingObject))
                    {
                        ModelSystemEditingObjectReferenceMap.Remove(e.Id);
                        ModelSystemObjectRefrenceMap.Remove(e.ObjectReference);
                    }
                }
            }

            _onModelSystemChanged?.Invoke(this, new ModelSystemChangedEventArgs(args)
            {
                EditingModelObject = ModelSystemObjectRefrenceMap[sender]
            });
        }


        /// <summary>
        /// </summary>
        /// <param name="modelSystem"></param>
        /// <param name="mapper"></param>
        public ModelSystemEditingTracker(ModelSystemEditingModel modelSystem, IMapper mapper)
        {
            ModelSystem = modelSystem;
            _mapper = mapper;
            StoreModelSystemObjectReferenceMap(modelSystem);
        }

        /// <summary>
        ///     Disposes the tracker and unregisters delegates
        /// </summary>
        public void Dispose()
        {
            foreach (var e in _delegates)
            {
                _onModelSystemChanged -= e;
            }
        }
    }

    public class ModelSystemChangedEventArgs : EventArgs
    {
        public ViewObject EditingModelObject { get; set; }

        public EventArgs Args { get; set; }

        public ModelSystemChangedEventArgs(EventArgs args)
        {
            Args = args;
        }
    }
}