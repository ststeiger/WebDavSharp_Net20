using System;
using System.Collections.Generic;
// using System.Linq;

namespace WebDAVSharp.Server.MethodHandlers
{
    /// <summary>
    /// This class contains code to produce the built-in
    /// <see cref="IWebDAVMethodHandler"/> instances known by WebDAV#.
    /// </summary>
    public static class WebDAVMethodHandlers
    {
        private static readonly List<IWebDAVMethodHandler> _BuiltIn = new List<IWebDAVMethodHandler>();

        /// <summary>
        /// Gets the collection of built-in <see cref="IWebDAVMethodHandler"/>
        /// HTTP method handler instances.
        /// </summary>
        public static IEnumerable<IWebDAVMethodHandler> BuiltIn
        {
            get
            {
                lock (_BuiltIn)
                {
                    if (_BuiltIn.Count == 0)
                        ScanAssemblies();
                }

                return _BuiltIn;
            }
        }

        /// <summary>
        /// Scans the WebDAV# assemblies for known <see cref="IWebDAVMethodHandler"/>
        /// types.
        /// </summary>
        private static void ScanAssemblies()
        {
            List<Type> methodHandlerTypes = new List<Type>();

            foreach (Type type in typeof(WebDAVServer).Assembly.GetTypes())
            {
                if (!type.IsAbstract & typeof(IWebDAVMethodHandler).IsAssignableFrom(type))
                    methodHandlerTypes.Add(type);
            }

            List<IWebDAVMethodHandler> methodHandlerInstances = new List<IWebDAVMethodHandler>();

            foreach (var type in methodHandlerTypes)
            {
                methodHandlerInstances.Add((IWebDAVMethodHandler)Activator.CreateInstance(type));
            }

            /*
            var methodHandlerTypes =
                from type in typeof(WebDAVServer).Assembly.GetTypes()
                where !type.IsAbstract
                where typeof(IWebDAVMethodHandler).IsAssignableFrom(type)
                select type;
            

            var methodHandlerInstances =
                from type in methodHandlerTypes
                select (IWebDAVMethodHandler)Activator.CreateInstance(type);
            */
            _BuiltIn.AddRange(methodHandlerInstances);
        }
    }
}