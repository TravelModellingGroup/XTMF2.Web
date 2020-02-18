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
using Microsoft.AspNetCore.SignalR;
using XTMF2.Editing;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Hubs;

namespace XTMF2.Web.Server.Session
{

    /// <summary>
    /// 
    /// </summary>
    public class ModelSystemEditingTracker : IDisposable
    {
        /// <summary>
        /// Tracks GUID to model system editing references
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        public Dictionary<Guid, ViewObject> ModelSystemEditingObjectReferenceMap { get; } = new Dictionary<Guid, ViewObject>();

        /// <summary>
        /// Tracks the underlying model system to editing model references
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        public Dictionary<object, ViewObject> ModelSystemObjectRefrenceMap { get; } = new Dictionary<object, ViewObject>();

        /// <summary>
        /// Delegate tracker for objects subscribiding to model system changes
        /// </summary>
        /// <value></value>
        public ModelSystemEditingModel ModelSystem { get; private set; }

        private List<EventHandler<ModelSystemChangedEventArgs>> _delegates = new List<EventHandler<ModelSystemChangedEventArgs>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewObject GetModelSystemEditingObject(Guid id)
        {
            return ModelSystemEditingObjectReferenceMap[id];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModelSystemObject<T>(Guid id)
        {
            return (T)ModelSystemEditingObjectReferenceMap[id].ObjectReference;
        }

        /// <summary>
        /// Register to track changes
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="modelSystem"></param>
        public ModelSystemEditingTracker(ModelSystemEditingModel modelSystem)
        {
            ModelSystem = modelSystem;
            StoreModelSystemObjectReferenceMap(modelSystem);
        }

        ~ModelSystemEditingTracker()
        {
            Dispose();
        }

        /// <summary>
        /// Stores all objects in the object/view/edit model reference maps
        /// </summary>
        /// <param name="session"></param>
        /// <param name="editingModel"></param>
        private void StoreModelSystemObjectReferenceMap(ModelSystemEditingModel editingModel)
        {
            foreach (var viewObject in Utils.ModelSystemUtils.ModelSystemObjects(editingModel))
            {
                ModelSystemEditingObjectReferenceMap[viewObject.Id] = viewObject;
                ModelSystemObjectRefrenceMap[viewObject.ObjectReference] = viewObject;
                if (viewObject.ObjectReference is INotifyPropertyChanged prop)
                {
                    prop.PropertyChanged += OnModelSystemPropertyChanged;
                }
                if (viewObject.ObjectReference is Boundary boundary)
                {
                    ((INotifyCollectionChanged)boundary.Boundaries).CollectionChanged += OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged)boundary.Modules).CollectionChanged += OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged)boundary.Links).CollectionChanged += OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged)boundary.CommentBlocks).CollectionChanged += OnModelSystemCollectionChanged;
                    ((INotifyCollectionChanged)boundary.Starts).CollectionChanged += OnModelSystemCollectionChanged;
                }
            }
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnModelSystemCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            _onModelSystemChanged?.Invoke(this, new ModelSystemChangedEventArgs(args)
            {
                EditingModelObject = ModelSystemObjectRefrenceMap[sender]
            });
        }

        /// <summary>
        /// Disposes the tracker and unregisters delegates
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
        public ModelSystemChangedEventArgs(EventArgs args)
        {
            Args = args;
        }
        public ViewObject EditingModelObject { get; set; }

        public EventArgs Args { get; set; }

    }

}