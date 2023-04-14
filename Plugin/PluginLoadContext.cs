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
    public class ShadowAssemblyLoadContext : AssemblyLoadContext
    {
        public ShadowAssemblyLoadContext() : base(isCollectible: true) { }

        public Assembly LoadFromFilePath(string path)
        {
            using var stream = new FileStream(path, FileMode.Open);
            return LoadFromStream(stream);
        }
    }
}

