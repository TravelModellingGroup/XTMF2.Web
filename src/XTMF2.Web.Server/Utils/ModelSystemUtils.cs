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
using System.Linq;
using XTMF2.Editing;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Data.Types;

namespace XTMF2.Web.Server.Utils
{
    public class ModelSystemUtils
    {
        /// <summary>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IEnumerable<ViewObject> Traverse(ViewObject o)
        {
            return ModelSystemObjectsTraverse(o);
        }

        /// <summary>
        ///     Iterate over all model system objects
        /// </summary>
        /// <param name="modelSystem"></param>
        /// <returns></returns>
        public static IEnumerable<ViewObject> ModelSystemObjects(ModelSystemEditingModel modelSystem)
        {
            return ModelSystemObjectsTraverse(modelSystem.GlobalBoundary);
        }

        private static IEnumerable<ViewObject> ModelSystemObjectsTraverse(ViewObject viewObject)
        {
            if (viewObject is BoundaryModel boundary)
            {
                foreach (var c in boundary.CommentBlocks)
                {
                    yield return c;
                }

                foreach (var s in boundary.Starts)
                {
                    yield return s;
                }

                foreach (var b in boundary.Boundaries)
                {
                    foreach (var boundaryChild in ModelSystemObjectsTraverse(b))
                    {
                        yield return boundaryChild;
                    }
                }
            }

            yield return viewObject;
        }

        /// <summary>
        ///     Returns the model system object addressed by the passed path.
        /// </summary>
        /// <param name="runtime">The XTMF Runtime</param>
        /// <param name="modelSystemSession">The model system session to query.</param>
        /// <param name="path">The path to the object in the form of eg: Parent.Child.Child.ObjectName</param>
        /// <returns></returns>
        public static T GetModelSystemObjectByPath<T>(XTMFRuntime runtime, ModelSystemSession modelSystemSession,
            Path path) where T : class
        {
            if (path.Parts.Length == 0)
            {
                return (T) (object) modelSystemSession.ModelSystem.GlobalBoundary;
            }

            return Traverse<T>(modelSystemSession.ModelSystem.GlobalBoundary, path, 0);
        }

        /// <summary>
        ///     Returns the editing type for the passed model system object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Type GetModelSystemEditingType(object o)
        {
            switch (o)
            {
                case Boundary _:
                    return typeof(BoundaryModel);
                case Start _:
                    return typeof(StartModel);
                case SingleLink _:
                    return typeof(SingleLinkModel);
                case MultiLink _:
                    return typeof(MultiLinkModel);
                case CommentBlock _:
                    return typeof(CommentBlockModel);
                case Node _:
                    return typeof(NodeModel);
                case NodeHook _:
                    return typeof(NodeHookModel);
                default:
                    return null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static T Traverse<T>(Boundary current, Path path, int index) where T : class
        {
            if (index >= path.Parts.Length - 1)
            {
                var type = typeof(T);
                if (type == typeof(Boundary))
                {
                    var boundary = current.Boundaries.FirstOrDefault(b => b.Name == path.Parts[index]);
                    if (boundary != null)
                    {
                        return (T) (object) boundary;
                    }
                }
                else if (type == typeof(Start))
                {
                    var start = current.Starts.FirstOrDefault(s => s.Name == path.Parts[index]);
                    if (start != null)
                    {
                        return (T) (object) start;
                    }
                }
                else if (type == typeof(CommentBlock))
                {
                    var commentBlock = current.CommentBlocks.FirstOrDefault(cb => cb.Comment == path.Parts[index]);
                    if (commentBlock != null)
                    {
                        return (T) (object) commentBlock;
                    }
                }
                else if (type == typeof(Node))
                {
                    var node = current.Modules.FirstOrDefault(n => n.Name == path.Parts[index]);
                    if (node != null)
                    {
                        return (T) (object) node;
                    }
                }

                // no matching element 
                return null;
            }

            {
                // find the boundary with this current name
                var boundary = current.Boundaries.FirstOrDefault(b => b.Name == path.Parts[index]);
                if (boundary == null)
                {
                    // not a valid path
                    return null;
                }

                return Traverse<T>(boundary, path, index + 1);
            }
        }
    }
}