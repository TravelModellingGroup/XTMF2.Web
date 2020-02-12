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
using XTMF2.RuntimeModules;
using XTMF2.Web.Server.Utils;
using XTMF2.Web.UnitTests;
using Xunit;


namespace XTMF2.Web.UnitTests.Utils
{
    /// <summary>
    /// Unit tests for ModelSystemUtils
    /// </summary>
    public class TestModelSystemUtils
    {
        private User _user;
        private XTMFRuntime _runtime;
        public TestModelSystemUtils()
        {
            _user = TestHelper.CreateTestUser(Guid.NewGuid().ToString());
            _runtime = TestHelper.Runtime;
        }

        /// <summary>
        /// Tests that an empty path query will return the global boundary.
        /// </summary>
        [Fact]
        public void EmptyPath_ReturnsGlobalBoundary()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = ModelSystemUtils.GetModelSystemObjectByPath<Boundary>(_runtime, modelSystemSession, new Data.Types.Path()
            {
                Parts = new string[0]
            });
            Assert.IsType<Boundary>(result);
            Assert.Equal(modelSystemSession.ModelSystem.GlobalBoundary, result);
        }

        /// <summary>
        /// Tests that the first element of a path is returned correctly
        /// </summary>
        [Fact]
        public void SingleElementPath_ReturnsCorrectObject()
        {
            TestHelper.InitializeTestModelSystem(_user, "TestProject", "TestModelSystem", out var modelSystemSession);
            var result = ModelSystemUtils.GetModelSystemObjectByPath<ModelSystemConstruct.Start>(_runtime, modelSystemSession, new Data.Types.Path()
            {
                Parts =  new []{ "TestStart" }
            });
            Assert.IsType<ModelSystemConstruct.Start>(result);
            Assert.Equal("TestStart", result.Name);
        }
    }
}