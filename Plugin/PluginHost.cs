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
using System.Windows.Media;

namespace Examath.Core.Plugin
{
    public abstract partial class PluginHost : ObservableObject
    {

        #region Fields

        protected const string EMPTY_NAME = "----";

        protected bool _CanExecute = false;

        private bool _IsLoaded = false; 

        protected Env _Env { get; set; }

        protected IPlugin? _Plugin { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the filename this plugin was loaded from
        /// </summary>
        public abstract string FileName { get; }

        private string _Name = EMPTY_NAME;
        /// <summary>
        /// Gets or sets wheter the <see cref="IPlugin"/> assembly and instance is loaded
        /// </summary>
        public bool IsPluginLoaded
        {
            get => _IsLoaded;
            protected set => SetProperty(ref _IsLoaded, value);
        }

        /// <summary>
        /// Gets the name of the <see cref="IPlugin"/> instance,
        /// or <c>----</c> if none existed
        /// </summary>
        public string Name
        {
            get => _Name;
            protected set => SetProperty(ref _Name, value);
        }

        private Color _Color = Color.FromRgb(255, 255, 255);
        /// <summary>
        /// Gets or sets 
        /// </summary>
        public Color Color
        {
            get => _Color;
            set => SetProperty(ref _Color, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new plugin host
        /// </summary>
        /// <param name="env">The enviorment the plugin runs in</param>
        public PluginHost(Env env)
        {
            _Env = env;
        }

        #endregion

        #region Methods

        public abstract void Load();

        protected void InitialisePluginFromType(Type type)
        {
            Name = type.Name;
            IPlugin? plugin = (IPlugin?)Activator.CreateInstance(type);

            if (plugin != null)
            {
                _Plugin = plugin;
                IsPluginLoaded = true;
                Color = plugin.Colour;
                if (plugin is IExecute || plugin is IExecuteAsync)
                {
                    _CanExecute = true;
                }
                ExecuteCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion

        #region Plugin Method Wrapper

        public void CallSetup()
        {
            try
            {
                if (_Plugin != null)
                {
                    _Plugin.Setup(_Env);
                    Color = _Plugin.Colour;                    
                }
            }
            catch (Exception e)
            {
                _Env.OutException(e, "Plugin Setup");
            }
        }

        /// <summary>
        /// Executes the <see cref="IExecute"/> plugin in a try/catch loop
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanExecute))]
        public async Task ExecuteAsync()
        {
            try
            {
                if (_Plugin != null)
                {
                    if (_Plugin is IExecute execute)
                    {
                        execute.Execute(_Env);
                    }
                    else if (_Plugin is IExecuteAsync executeAsync)
                    {
                        await executeAsync.Execute(_Env);
                    }
                    Color = _Plugin.Colour;
                }
            }
            catch (Exception e)
            {
                _Env.OutException(e, "Plugin Execute");
            }
        }

        private bool CanExecute()
        {
            return _CanExecute;
        } 

        #endregion
    }
}
