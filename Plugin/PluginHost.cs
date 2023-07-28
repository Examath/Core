using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examath.Core.Environment;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Examath.Core.Plugin
{
    public abstract partial class PluginHost : ObservableObject
    {
        #region Fields

        /// <summary>
        /// The type of <see cref="IPlugin"/> to initialise
        /// </summary>
        protected Type? _Type;

        /// <summary>
        /// What to display if there is no <see cref="IPlugin"/> loaded
        /// </summary>
        protected const string EMPTY_NAME = "----";

        private bool _IsLoaded = false;

        /// <summary>
        /// The <see cref="Env"/> to run the plugin in
        /// </summary>
        protected Env _Env { get; set; }

        /// <summary>
        /// The initialised plugin
        /// </summary>
        protected IPlugin? _Plugin { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the filename this plugin was loaded from. This is not he file location.
        /// </summary>
        public abstract string FileName { get; }

        /// <summary>
        /// Gets or sets whether the <see cref="IPlugin"/> assembly and instance is loaded
        /// </summary>
        public bool IsPluginLoaded
        {
            get => _IsLoaded;
            protected set => SetProperty(ref _IsLoaded, value);
        }

        private string _Name = EMPTY_NAME;
        /// <summary>
        /// Gets the name of the <see cref="IPlugin"/> instance,
        /// or <c>----</c> if none existed
        /// </summary>
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private Color _Colour = Color.FromRgb(255, 255, 255);
        /// <summary>
        /// Gets or sets the colour of the plugin. This is set to <see cref="IPlugin.Colour"/> whenever a method in <see cref="IPlugin"/> is executed
        /// </summary>
        public Color Colour
        {
            get => _Colour;
            set => SetProperty(ref _Colour, value);
        }

        private object _Tooltip;
        /// <summary>
        /// Gets the tooltip of this plugin. This is set to <see cref="IPlugin.Tooltip"/> on etup
        /// </summary>
        public object Tooltip
        {
            get => _Tooltip;
            set => SetProperty(ref _Tooltip, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new plugin host
        /// </summary>
        /// <param name="env">The environment the plugin runs in</param>
        /// <remarks>This does not load the plugin</remarks>
        public PluginHost(Env env)
        {
            _Env = env;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the plugin
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Creates an instance of <see cref="_Type"/> if it is not null and sets it to the <see cref="_Plugin"/> if it is not null. 
        /// Then updates <see cref="Name"/>, <see cref="Color"/> and <see cref="CanExecute"/>
        /// </summary>
        /// <remarks>
        /// The type must be <see cref="IPlugin"/>. Afterwards, <see cref="CallSetup"/> synchronously.
        /// </remarks>
        protected void CreateInstanceOfPluginType()
        {
            if (_Type != null)
            {
                Name = _Type.Name;
                IPlugin? plugin = (IPlugin?)Activator.CreateInstance(_Type);

                if (plugin != null)
                {
                    _Plugin = plugin;
                    IsPluginLoaded = true;
                    Colour = plugin.Colour;
                    CheckCanExecute();
                }
            }
        }

        #endregion

        #region Plugin Setup Wrapper

        /// <summary>
        /// Tries to call <see cref="IPlugin.Setup(Env)"/>
        /// </summary>
        public void CallSetup()
        {
            try
            {
                if (_Plugin != null)
                {
                    _Plugin.Setup(_Env);
                    Colour = _Plugin.Colour;
                    Tooltip = _Plugin.Tooltip;
                }
            }
            catch (Exception e)
            {
                _Env.OutException(e, "Plugin Setup");
            }
        }

        #endregion

        #region Plugin Execute Wrapper

        /// <summary>
        /// gets whether the plugin supports <see cref="IExecute"/> or <see cref="IExecuteAsync"/>
        /// </summary>
        public bool CanExecute { get; private set; }

        /// <summary>
        /// Checks whether the plugin is not null and implements <see cref="IExecute"/> or <see cref="IExecuteAsync"/>,
        /// and updates the execute command.
        /// </summary>
        protected void CheckCanExecute()
        {
            CanExecute = _Plugin != null && (_Plugin is IExecute || _Plugin is IExecuteAsync);
            ExecuteCommand.NotifyCanExecuteChanged();
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
                    Colour = _Plugin.Colour;
                    Tooltip = _Plugin.Tooltip;
                }
            }
            catch (Exception e)
            {
                _Env.OutException(e, "Plugin Execute");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Reinitialises the internal plugin
        /// </summary>
        [RelayCommand]
        public virtual async Task ReloadAsync()
        {
            if (_Type != null)
            {
                // Reset
                IsPluginLoaded = false;
                _Plugin = null;
                CheckCanExecute();

                // Reload
                CreateInstanceOfPluginType();
                if (IsPluginLoaded)
                {
                    CallSetup();
                }
                else
                {
                    _Env.Out($"Could not re-initialise plugin {_Type.Name}", ConsoleStyle.FormatBlockStyle);
                }
            }
        }

        /// <summary>
        /// Shows an <see cref="Asker"/> dialog to modify this <see cref="Name"/>
        /// </summary>
        [RelayCommand]
        public void Rename()
        {
            TextBoxInput textBoxInput = new(this, nameof(Name));
            Asker.Show(new($"Rename instance"), textBoxInput);
        }

        #endregion
    }
}
