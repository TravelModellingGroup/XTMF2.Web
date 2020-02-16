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
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using XTMF2.Editing;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Hubs;

namespace XTMF2.Web.Server.Session
{

    public class ModelSystemEditingTracker : IDisposable
    {
        /// <summary>
        /// Tracks GUID to model system editing references
        /// </summary>
        /// <typeparam name="Guid"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        public Dictionary<Guid, object> ModelSystemEditingObjectReferenceMap { get; } = new Dictionary<Guid, object>();

        /// <summary>
        /// Tracks the underlying model system to editing model references
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        public Dictionary<object, object> ModelSystemObjectRefrenceMap { get; } = new Dictionary<object, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public ModelSystemEditingModel ModelSystem { get; private set; }

        private List<EventHandler<ModelSystemChangedEventArgs>> _delegates = new List<EventHandler<ModelSystemChangedEventArgs>>();

        private event EventHandler<ModelSystemChangedEventArgs> _onModelSystemChanged;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="modelSystem"></param>
        public ModelSystemEditingTracker(ModelSystemEditingModel modelSystem)
        {
            ModelSystem = modelSystem;
        }

         ~ModelSystemEditingTracker() {
            Dispose();
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

    }

}