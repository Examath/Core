using CommunityToolkit.Mvvm.Input;
using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Examath.Core.Plugin
{
    /// <summary>
    /// loads, hosts and runs an <see cref="IPlugin"/> derivative
    /// </summary>
    public partial class ExternalPluginHost : PluginHost
    {

        #region Private Properties

        private ShadowAssemblyLoadContext? _PluginLoadContext;

        public DateTime _CompilationWriteTime = DateTime.MinValue;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the location to load (and update) this plugin from
        /// </summary>
        public string FileLocation { get; protected set; }

        /// <summary>
        /// Gets the file name of this plugin
        /// </summary>
        public override string FileName => Path.GetFileName(FileLocation);

        #endregion

        #region Init

        /// <summary>
        /// Creates a new host to load and run a range of <see cref="IPlugin"/> derivatives
        /// in the specified <paramref name="env"/>
        /// </summary>
        /// <param name="env">
        /// The enviorment given as context to the plugin. 
        /// This includes the console and form.
        /// </param>
        public ExternalPluginHost(Env env, string fileLocation) : base(env)
        {
            FileLocation = fileLocation;
        }

        #endregion

        #region Static Methods


        #endregion

        #region Load

        /// <summary>
        /// Loads and creates an instance of the <see cref="IPlugin"/> from the provided <see cref="Assembly"/> at <see cref="FileLocation"/>
        /// </summary>
        public override void Load()
        {
            _CompilationWriteTime = File.GetLastWriteTime(FileLocation);
            _PluginLoadContext = new();
            Assembly assembly = _PluginLoadContext.LoadFromFilePath(FileLocation);
            IsPluginLoaded = false;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(type))
                {
                    InitialisePluginFromType(type);
                    break;
                }
            }
        }

        #endregion

        #region Unload

        public List<WeakReference> OldPluginContextReferences { get; private set; } = new();

        private int _AliveInstances = 0;
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public int AliveInstances
        {
            get => _AliveInstances;
            set => SetProperty(ref _AliveInstances, value);
        }

        /// <summary>
        /// Discards the plugin and then calls <see cref="System.Runtime.Loader.AssemblyLoadContext.Unload"/>
        /// </summary>
        public async Task UnloadAsync(Log? log = null)
        {
            if (_PluginLoadContext != null)
            {
                // UI
                log?.StartTiming("Unloading");
                Name = EMPTY_NAME;
                _CanExecute = false;
                ExecuteCommand.NotifyCanExecuteChanged();

                // Plugin
                WeakReference pluginWeakReference = new(_PluginLoadContext, trackResurrection: true);
                OldPluginContextReferences.Add(pluginWeakReference);

                // Nulling and unloading
                _Plugin = null;
                _PluginLoadContext.Unload();
                _PluginLoadContext = null;

                // Wait for unload
                await Task.Delay(100);
                for (int i = 0; pluginWeakReference.IsAlive && (i < 3); i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                // End
                IsPluginLoaded = false;
                log?.EndTiming();

                // Test and report memory leak(s)
                OldPluginContextReferences = OldPluginContextReferences.Where((x) => x.IsAlive).ToList();
                AliveInstances = OldPluginContextReferences.Count;
                if (AliveInstances > 0)
                {
                    log?.Out($"Memory leak: {AliveInstances} instance of this plugin still in memory", ConsoleStyle.FormatBlockStyle);
                }
            }
        }

        [RelayCommand]
        public async Task Reload()
        {
            if (File.GetLastWriteTime(FileLocation) <= _CompilationWriteTime) return;

            Log log = _Env.StartLog();

            // File location
            if (!File.Exists(FileLocation))
            {
                log.Out($"Compilation {FileName} not found", ConsoleStyle.ErrorBlockStyle);
                return;
            }

            /// Unloading
            if (IsPluginLoaded) await UnloadAsync(log);

            // Loading
            log.StartTiming($"Loading {Path.GetFileName(FileLocation)}");

            try
            {
                await Task.Run(Load);
                ExecuteCommand.NotifyCanExecuteChanged();
                if (IsPluginLoaded)
                {
                    CallSetup();
                }
                else
                {
                    log.Out("Assembly loaded, but no plugin found or loaded", ConsoleStyle.FormatBlockStyle);
                }
            }
            catch (Exception e)
            {
                log.OutException(e, "Loading Plugin");
            }

            log.EndTiming();
        }

        #endregion
    }
}
