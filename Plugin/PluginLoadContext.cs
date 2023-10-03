using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Examath.Core.Plugin
{
    /// <summary>
    /// Represents a load context for Examath Plugins
    /// </summary>
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _Resolver;

        /// <summary>
        /// Create a new plugin load context
        /// </summary>
        /// <param name="pluginPath"></param>
        public PluginLoadContext(string pluginPath) : base(isCollectible: true)
        {
            _Resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = _Resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = _Resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }

    /// <summary>
    /// Represents a load context that loads plugin into memory
    /// </summary>
    internal class ShadowAssemblyLoadContext : AssemblyLoadContext
    {
        internal ShadowAssemblyLoadContext() : base(isCollectible: true) { }

        internal Assembly LoadFromFilePath(string path)
        {
            using var stream = new FileStream(path, FileMode.Open);
            return LoadFromStream(stream);
        }
    }
}

