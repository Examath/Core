using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Examath.Core.Environment;
using System.Diagnostics;

namespace Examath.Core.Plugin
{
    /// <summary>
    /// loads, hosts and runs an <see cref="IPlugin"/> derivative
    /// </summary>
    public partial class ExternalPluginHost : PluginHost
    {

        #region Private Properties

        private ShadowAssemblyLoadContext? _PluginLoadContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// The location to load (and update) this plugin from
        /// </summary>
        public string FileLocation { get; protected set; }

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
            OldPluginContextReference = new(this);
        }

        #endregion

        #region Static Methods


        #endregion

        #region Load

        /// <summary>
        /// Loads and creates an instance of the <see cref="IPlugin"/> from the provided <see cref="Assembly"/> at <see cref="FileLocation"/>
        /// </summary>
        /// <returns>True if the plugin loaded succesfuly, and false if the plugin was not found in the assembly</returns>
        public override void Load()
        {
            _PluginLoadContext = new();
            //_PluginLoadContext = new(FileLocation);
            Assembly assembly = _PluginLoadContext.LoadFromFilePath(FileLocation);
            //Assembly assembly = _PluginLoadContext.LoadFromAssemblyName(new(Path.GetFileNameWithoutExtension(FileLocation)));
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

        public WeakReference OldPluginContextReference { get; private set; }

        /// <summary>
        /// Discards the plugin and then calls <see cref="System.Runtime.Loader.AssemblyLoadContext.Unload"/>
        /// </summary>
        /// <returns>
        /// True if unload was completed, otherwise false if timeout
        /// </returns>
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
                _Plugin = null;
                OldPluginContextReference = new(_PluginLoadContext, trackResurrection: true);
                _PluginLoadContext.Unload();
                _PluginLoadContext = null;
                IsPluginLoaded = false;

                // Wait for unload
                await Task.Delay(100);
                for (int i = 0; OldPluginContextReference.IsAlive && (i < 3); i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                // Test and report

                bool isModuleLoaded = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().ToList()
                    .Exists(m => m.FileName?.ToLowerInvariant() == FileLocation.ToLowerInvariant());
                log?.EndTiming();

                if (isModuleLoaded || OldPluginContextReference.IsAlive)
                {
                    log?.Out(
                            "Previous plugin was not unloaded:" +
                            ((OldPluginContextReference.IsAlive) ? " PluginContext is still alive." : "") +
                            ((isModuleLoaded) ? " File still locked by this process." : "")
                        , ConsoleStyle.FormatBlockStyle);
                }
            }
        }

        [RelayCommand]
        public async Task EditAsync()
        {
            Log log = _Env.StartLog();

            // File location
            if (!File.Exists(FileLocation))
            {
                log.Out($"File {Path.GetFileName(FileLocation)} no longer exists", ConsoleStyle.FormatBlockStyle);
                return;
            }

            /// Unloading
            await UnloadAsync(log);

            // Loading
            log.StartTiming($"Reloading {Path.GetFileName(FileLocation)}");

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
