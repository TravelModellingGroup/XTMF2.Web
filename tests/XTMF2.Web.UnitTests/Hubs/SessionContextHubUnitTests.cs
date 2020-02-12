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
using Microsoft.Extensions.Logging;
using Moq;
using XTMF2.Web.Server.Hubs;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Hubs
{
    /// <summary>
    ///     Unit tests related to the SessionContextUnitHub
    /// </summary>
    [Collection("Sequential")]
    public class SessionContextHubUnitTests : IDisposable
    {
        public SessionContextHubUnitTests()
        {
            _userName = Guid.NewGuid().ToString();
            TestHelper.CreateTestUser(_userName);
            _runtime = TestHelper.Runtime;
            _projectSessions = new ProjectSessions();
            _modelSystemSessions = new ModelSystemSessions();
            _userSession = new UserSession(_runtime.UserController.GetUserByName(_userName));
            var logger = Mock.Of<ILogger<SessionContextHub>>();
            _sessionContextHub = new SessionContextHub(logger, _projectSessions, _modelSystemSessions);
        }

        public void Dispose()
        {
            TestHelper.CleanUpTestContext(_runtime, _userName);
            _runtime.Shutdown();
        }

        private readonly XTMFRuntime _runtime;
        private readonly string _userName;
        private readonly ProjectSessions _projectSessions;
        private readonly ModelSystemSessions _modelSystemSessions;
        private readonly UserSession _userSession;
        private readonly SessionContextHub _sessionContextHub;

        /// <summary>
        ///     Asserts that a single connection returns the proper amount of user counts
        /// </summary>
        [Fact]
        public void UserConnected_SessionCount_IsValidCount()
        {
            _sessionContextHub.TrackUserConnected(_userSession);
            Assert.Equal(1, _sessionContextHub.UserSessionCounts[_userSession.User]);
        }

        /// <summary>
        ///     Tracks multiple connects and disconnects
        /// </summary>
        [Fact]
        public void UserConnectedDisconnected_SessionCount_IsValidCount()
        {
            //each connected is a new "session"
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserDisconnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserDisconnected(_userSession);
            Assert.Equal(2, _sessionContextHub.UserSessionCounts[_userSession.User]);
        }

        /// <summary>
        ///     Asserts that multiple user connections returns corret user count.
        /// </summary>
        [Fact]
        public void UserConnectedMultipleTimes_SessionCount_IsValidCount()
        {
            //each connected is a new "session"
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            Assert.Equal(3, _sessionContextHub.UserSessionCounts[_userSession.User]);
        }

        /// <summary>
        ///     Tracks session clear, project sessions are all cleared
        /// </summary>
        [Fact]
        public void UserDisconnected_ModelSystemSessionCounts_AreCleared()
        {
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            string error = default;
            _runtime.ProjectController.CreateNewProject(_userSession.User, "ProjectName", out var projectSession,
                ref error);
            projectSession.CreateNewModelSystem(_userSession.User, "MSNAME", out var modelSystem, ref error);
            projectSession.EditModelSystem(_userSession.User, modelSystem, out var modelSystemSession, ref error);
            _modelSystemSessions.TrackSessionForUser(_userSession.User, projectSession.Project, modelSystemSession);
            Assert.Single(_modelSystemSessions.Sessions[_userSession.User]);
            _sessionContextHub.TrackUserDisconnected(_userSession);
            _sessionContextHub.TrackUserDisconnected(_userSession);
            Assert.Empty(_modelSystemSessions.Sessions[_userSession.User]);
        }

        /// <summary>
        ///     Tracks session clear, project sessions are all cleared
        /// </summary>
        [Fact]
        public void UserDisconnected_ProjectSessionCounts_AreCleared()
        {
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserConnected(_userSession);
            string error = default;
            _runtime.ProjectController.CreateNewProject(_userSession.User, "ProjectName", out var projectSession,
                ref error);
            _runtime.ProjectController.CreateNewProject(_userSession.User, "ProjectName2", out var projectSession2,
                ref error);
            _projectSessions.TrackSessionForUser(_userSession.User, projectSession);
            _projectSessions.TrackSessionForUser(_userSession.User, projectSession2);
            Assert.Collection(_projectSessions.Sessions[_userSession.User],
                item => { Assert.Equal("ProjectName", item.Project.Name); },
                item => { Assert.Equal("ProjectName2", item.Project.Name); });
            _sessionContextHub.TrackUserDisconnected(_userSession);
            _sessionContextHub.TrackUserDisconnected(_userSession);
            Assert.Empty(_projectSessions.Sessions[_userSession.User]);
        }

        /// <summary>
        ///     Tests single connect and disconnect
        /// </summary>
        [Fact]
        public void UserDisconnected_SessionCount_IsValidCount()
        {
            //each connected is a new "session"
            _sessionContextHub.TrackUserConnected(_userSession);
            _sessionContextHub.TrackUserDisconnected(_userSession);
            Assert.Equal(0, _sessionContextHub.UserSessionCounts[_userSession.User]);
        }
    }
}