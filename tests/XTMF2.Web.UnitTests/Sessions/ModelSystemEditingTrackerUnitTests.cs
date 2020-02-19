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
using AutoMapper;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Mapping.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Sessions
{
    /// <summary>
    ///     Unit tests related to the model sysem tracker
    /// </summary>
    public class ModelSystemEditingTrackerUnitTests : IDisposable
    {
        private IMapper _mapper;
        private User _user;
        public ModelSystemEditingTrackerUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
                         {
                             cfg.AddProfile<ModelSystemProfile>();
                             cfg.AddProfile<ProjectProfile>();
                         });
            _mapper = config.CreateMapper();
            _user = TestHelper.CreateTestUser(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Tests that references are stored in the object map
        /// </summary>
        [Fact]
        public void NewTracker_StoresAllReferencesCorrectly()
        {
            TestHelper.InitializeTestModelSystem(_user, "ProjectName", "TestModelSystem", out var session);
            var editingModel = _mapper.Map<ModelSystemEditingModel>(session.ModelSystem);
            var tracker = new ModelSystemEditingTracker(editingModel,_mapper);
            Assert.Equal(tracker.ModelSystemObjectReferenceMap.Count, tracker.ModelSystemEditingObjectReferenceMap.Count);
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.Count > 0);
        }

        /// <summary>
        /// Tests that references are stored in the object map
        /// </summary>
        [Fact]
        public void Tracker_Stores_SpecificReferencesCorrectly()
        {
            TestHelper.InitializeTestModelSystem(_user, "ProjectName", "TestModelSystem", out var session);
            var editingModel = _mapper.Map<ModelSystemEditingModel>(session.ModelSystem);
            var tracker = new ModelSystemEditingTracker(editingModel,_mapper);
            Assert.True(tracker.ModelSystemEditingObjectReferenceMap.ContainsKey(editingModel.GlobalBoundary.Id));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(editingModel.GlobalBoundary.ObjectReference));
        }

        /// <summary>
        /// Tests that references are stored in the object map when new items are added
        /// </summary>
        [Fact]
        public void Tracker_AddsNewReferences_WhenModelSystemHasNewObject()
        {
            TestHelper.InitializeTestModelSystem(_user, "ProjectName", "TestModelSystem", out var session);
            var editingModel = _mapper.Map<ModelSystemEditingModel>(session.ModelSystem);
            var tracker = new ModelSystemEditingTracker(editingModel,_mapper);
            session.AddCommentBlock(_user, session.ModelSystem.GlobalBoundary, "A comment", new Rectangle(0, 0, 100, 100), out var block, out var error);
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(block));
            Assert.NotEqual(new Guid(), tracker.ModelSystemObjectReferenceMap[block].Id);
            // Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(editingModel.GlobalBoundary.ObjectReference));
        }

        /// <summary>
        /// Tests that references are removed from the reference map
        /// </summary>
        [Fact]
        public void Tracker_RemovesReferences_WhenModelSystemRemovesObject()
        {
            TestHelper.InitializeTestModelSystem(_user, "ProjectName", "TestModelSystem", out var session);
            var editingModel = _mapper.Map<ModelSystemEditingModel>(session.ModelSystem);
            var tracker = new ModelSystemEditingTracker(editingModel,_mapper);
            session.AddCommentBlock(_user, session.ModelSystem.GlobalBoundary, "A comment", new Rectangle(0, 0, 100, 100), out var block, out var error);
            session.AddCommentBlock(_user, session.ModelSystem.GlobalBoundary, "A comment 2", new Rectangle(0, 0, 100, 100), out var block2, out  error);
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(block));
            Assert.NotEqual(new Guid(), tracker.ModelSystemObjectReferenceMap[block].Id);
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(block2));
            session.RemoveCommentBlock(_user,  session.ModelSystem.GlobalBoundary, block, out error);
            Assert.True(!tracker.ModelSystemObjectReferenceMap.ContainsKey(block));
            Assert.True(tracker.ModelSystemObjectReferenceMap.ContainsKey(block2));
        }


        public void Dispose()
        {

        }
    }
}