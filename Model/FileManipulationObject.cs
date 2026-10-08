using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Examath.Core.Model
{
    /// <summary>
    /// Represents a viewmodel with data associated with a file
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Exposes a <see cref="FileLocation"/> property and <see cref="Open"/>, <see cref="Save"/> and <see cref="SaveAs"/> commands.
    ///     The <see cref="SaveFileAsync"/>, <see cref="LoadFile"/> and <see cref="LoadFileAsync"/> methods must be implemented by derivied type.
    /// </para>
    /// </remarks>
    public abstract partial class FileManipulationObject : ObservableObject
    {
        #region Fields

        private string OpenSaveFileFilter;

        #endregion

        #region Init

        /// <summary>
        /// Creates a view model associated with a file with the specified <see cref="FileFilter"/>
        /// </summary>
        /// <param name="fileFilters">The file filter (excluding importers and exporters) of the data file of this model</param>
        public FileManipulationObject(params FileFilter[] fileFilters)
        {
            List<FileFilter> filtersList = new();
            filtersList.AddRange(fileFilters);
            filtersList.Add(new("All files", false, "*.*"));
            OpenSaveFileFilter = String.Join("|", filtersList);
        }

        #endregion

        #region File Location

        private string? _FileLocation;
        /// <summary>
        /// The property storing the location of this file.
        /// </summary>
        public string? FileLocation
        {
            get => _FileLocation;
            set
            {
                if (SetProperty(ref _FileLocation, value))
                {
                    FileName = Path.GetFileName(_FileLocation);
                    OnPropertyChanged(nameof(FileName));
                    SaveCommand.NotifyCanExecuteChanged();
                };
            }
        }

        /// <summary>
        /// Gets the file name, including extension, of the file given by <see cref="FileLocation"/>
        /// </summary>
        public string? FileName { get; private set; }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Create a new, blank file
        /// </summary>
        public abstract void CreateFile();

        /// <summary>
        /// Loads the data from the file at <see cref="FileLocation"/>. This method may throw exceptions.
        /// </summary>
        public abstract void LoadFile();

        /// <summary>
        /// Loads the data from the file at <see cref="FileLocation"/> asynchronously. This method may throw exceptions.
        /// </summary>
        public abstract Task LoadFileAsync();

        /// <summary>
        /// Saves the data to a file at <see cref="FileLocation"/>
        /// </summary>
        public abstract void SaveFile();

        /// <summary>
        /// Saves the data to a file at <see cref="FileLocation"/> asynchronously. This method may throw exceptions.
        /// </summary>
        public abstract Task SaveFileAsync();

		#endregion

		#region Open & Load

		/// <summary>
		/// Attempts to synchronously <see cref="LoadFileAsync"/> this object from the file at <see cref="FileLocation"/>. This method may throw exceptions.
		/// </summary>
		/// <remarks>
		/// Call for autoloading at application startup
		/// No dialogs are created to inform the user if this works or fails, use <see cref="Open"/> if some user control is required.
		/// </remarks>
		/// <returns>
		/// True if file exists and is isLoaded without error, and false otherwise.
		/// </returns>
		public bool TryLoad()
        {
            if (File.Exists(FileLocation))
            {
                try
                {
                    ResetNotifyChanges("");
                    LoadFile();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Opens a <see cref=" Microsoft.Win32.OpenFileDialog"/> and attempts to load the user-selected file asynchronously inside a try-catch block.
        /// </summary>
        /// <remarks>
        /// Checks if <see cref="IsUserReadyToPartWithCurrentFile"/> before executing. 
        /// Also sets <see cref="FileLocation"/> to the result of the dialog if succesfully loaded.
        /// </remarks>
        [RelayCommand]
        public async Task Open()
        {
            if (!await IsUserReadyToPartWithCurrentFile()) return;

            Microsoft.Win32.OpenFileDialog dialog = new()
            {
                Filter = OpenSaveFileFilter, // Filter files by extension
                RestoreDirectory = true,
                CheckFileExists = true,
            };

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string? oldFileLocation = FileLocation;
                FileLocation = dialog.FileName;
                try
                {
                    ResetNotifyChanges("");
                    await LoadFileAsync();
                }
                catch (Exception e)
                {
                    OnLoadError(e);
                    FileLocation = oldFileLocation;
                }
            }
        }

		/// <summary>
		/// Attempts to load the file <paramref name="newFileLocation"/> asynchronously inside a try-catch block.
		/// </summary>
		/// <remarks>
		/// Checks if <see cref="IsUserReadyToPartWithCurrentFile"/> before executing. 
        /// Also sets <see cref="FileLocation"/> if successful.
		/// </remarks>
        /// <param name="newFileLocation">The specified new file location</param>
        /// <param name="checkIfUserReadyToPartWithCurrentFile">Whether to check if the user want to save any unsaved changes</param>
		public async Task Open(string newFileLocation, bool checkIfUserReadyToPartWithCurrentFile = false)
		{
			if (checkIfUserReadyToPartWithCurrentFile && !await IsUserReadyToPartWithCurrentFile()) return;

			// Open document
			string? oldFileLocation = FileLocation;
			FileLocation = newFileLocation;
			try
			{
				ResetNotifyChanges("");
				await LoadFileAsync();
			}
			catch (Exception e)
			{
				OnLoadError(e);
				FileLocation = oldFileLocation;
			}
		}

		/// <summary>
		/// Repeatedly prompts the user till a valid existing file is opened 
		/// or a new file is made, setting 
		/// </summary>
		/// <remarks>
		/// Use <see cref="TryLoad"/>, <see cref="Open"/> where possible for friendlier user experience.
		/// </remarks>
		public async Task CreateOrOpen()
        {
            bool isLoaded = false;

            while (!isLoaded)
            {
                Microsoft.Win32.OpenFileDialog dialog = new()
                {
                    Filter = OpenSaveFileFilter, // Filter files by extension
                    RestoreDirectory = true,
                    CheckFileExists = false,
                    Title = "Open an existing file or create a new file",
                };

                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    FileLocation = dialog.FileName;
                    try
                    {
                        if (File.Exists(FileLocation))
                        {
                            ResetNotifyChanges("");
                            await LoadFileAsync();
                        }
                        else
                        {

                        }
                        isLoaded = true;
                    }
                    catch (Exception e)
                    {
                        OnLoadError(e);
                    }
                }
                else
                {
                    Messager.Out(
                        "You can either open an existing file (don't forget to Save As if you want to keep the original) " +
                        "or type in a filename at the desired directory.\nPress [OK] to continue ...",
                        "File location required",
                        ConsoleStyle.WarningBlockStyle);
                }
            }
        }

        /// <summary>
        /// Shows a <see cref="Messager"/> for error <paramref name="e"/>. Override for custom prompt.
        /// </summary>
        /// <param name="e">The error to display</param>
        protected virtual void OnLoadError(Exception e)
        {
            Messager.Out($"A {e.GetType().Name} was thrown when attempting to load {FileName}\n" +
                $"Details: {e.Message}", "Could not open file", ConsoleStyle.ErrorBlockStyle);
        }

        #endregion

        #region Modification Tracking

        private int _ChangesCount = 0;
        /// <summary>
        /// Gets the number of changes that have been made since last save
        /// </summary>
        public int ChangesCount
        {
            get => _ChangesCount;
            private set => SetProperty(ref _ChangesCount, value);
        }

        /// <summary>
        /// Gets whether this model has been modified
        /// </summary>
        /// <remarks>
        /// To set as modified, call <see cref="NotifyChange(object?, EventArgs?)"/>
        /// </remarks>
        public bool IsModified => ChangesCount > 0;

        private string _LastChangeDescription = "Loaded";
        /// <summary>
        /// Gets a string showing what the last modification was, and how many unsaved changes there are
        /// </summary>
        public string LastChangeDescription
        {
            get => _LastChangeDescription;
            private set => SetProperty(ref _LastChangeDescription, value);
        }


        /// <summary>
        /// Updates <see cref="SaveCommand"/> can execute and other properties
        /// </summary>
        /// <param name="label">The object that made the last change. It's ToString is added to <see cref="ChangesDescription"/> if it is not null</param>
        /// <param name="e"></param>
        public void NotifyChange(object? label = null, EventArgs? e = null)
        {
            ChangesCount++;
            LastChangeDescription = (label == null) ? $"{ChangesCount} changes" : $"{ChangesCount} changes: {label}";

            OnPropertyChanged(nameof(IsModified));
            SaveCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Resets properties associated with <see cref="NotifyChange(object?, EventArgs?)"/>
        /// </summary>
        /// <param name="label">Label for <see cref="ChangesDescription"/>, by default 'Saved'</param>
        public void ResetNotifyChanges(string label = "Saved")
        {
            ChangesCount = 0;
            LastChangeDescription = label;

            OnPropertyChanged(nameof(IsModified));
            SaveCommand.NotifyCanExecuteChanged();
        }

		/// <summary>
		/// If this file <see cref="IsModified"/>, a <see cref="ShowUnsavedChangesPrompt"/>
		/// is shown and if the user intends to save, either the <see cref="Save"/> 
		/// or <see cref="SaveAs"/> commands are called. 
		/// </summary>
		/// <example>
		/// Implement in MainWindow.xaml.cs by:
		/// <code> 		
		/// #region Closing
		/// private bool _IsReadyToClose = false;
		/// 
		/// protected override async void OnClosing(CancelEventArgs e)
		/// {
		/// 	// Avoid Refire
		/// 	if (_IsReadyToClose) return;
		/// 	base.OnClosing(e);
		/// 
		/// 	// If dirty
		/// 	if (_VM != null &amp;&amp; _VM.IsModified)
		/// 	{
		/// 		// Temp cancel Closing
		/// 		e.Cancel = true;
		/// 
		/// 		if (await _VM.IsUserReadyToPartWithCurrentFile())
		/// 		{
		/// 			// Restart closing
		/// 			_IsReadyToClose = true;
		/// 			Application.Current.Shutdown();
		/// 		}
		/// 	}
		/// }
		/// 
		/// #endregion
		/// </code>
		/// </example>
		/// <returns>False if user intends to cancel their command</returns>
		public async Task<bool> IsUserReadyToPartWithCurrentFile()
        {
            if (IsModified)
            {
                DialogResult dialogResult = ShowUnsavedChangesPrompt();

                switch (dialogResult)
                {
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.Yes:
                        if (File.Exists(FileLocation))
                        {
                            await Save();
                        }
                        else
                        {
                            await SaveAs();
                        }
                        return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Shows a <see cref="MessageBox"/> to tell if user intends to save, discard or cancel
        /// any unsaved changes. Override for custom prompting behaviour.
        /// </summary>
        /// <returns>The result of the <see cref="MessageBoxButton.YesNoCancel"/></returns>
        protected virtual DialogResult ShowUnsavedChangesPrompt()
        {
            return Messager.Out(
                    $"Would you like to save the {ChangesCount} changes to {FileName}",
                    "Unsaved changes",
                    ConsoleStyle.WarningBlockStyle,
                    isNoButtonVisible: true,
                    isCancelButtonVisible: true);
        }

        #endregion

        #region Save

        /// <summary>
        /// Gets or sets whether
        /// </summary>
        /// <returns></returns>
        public bool CanSave() => IsModified && FileLocation != null;

        /// <summary>
        /// Calls <see cref="SaveFileAsync"/> inside a try-catch block
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSave))]
        public async Task Save()
        {
            try
            {
                await SaveFileAsync();
                ResetNotifyChanges();
            }
            catch (Exception e)
            {
                Messager.OutException(e, "Saving");
            }
        }

        /// <summary>
        /// Opens a <see cref="Microsoft.Win32.SaveFileDialog"/> and then calls <see cref="SaveFileAsync"/>
        /// </summary>
        /// <remarks>
        /// Also sets <see cref="FileLocation"/> to the result of the dialog.
        /// </remarks>
        [RelayCommand]
        public async Task SaveAs()
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dialog = new()
            {
                Filter = OpenSaveFileFilter, // Filter files by extension
                RestoreDirectory = true,
            };

            // Show save file dialog box
            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                FileLocation = dialog.FileName;
                await Save();
            }
        }

        #endregion
    }
}