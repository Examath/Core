using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Examath.Core.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

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

        public FileManipulationObject(params FileFilter[] fileFilters)
        {
            List<FileFilter> filtersList = new();
            filtersList.AddRange(fileFilters);
            filtersList.Add(new("All files", false, "*.*"));
            OpenSaveFileFilter = String.Join("|", filtersList);
        }

        #endregion

        #region Abstract

        /// <summary>
        /// The property storing the location of this file, as selected by file dialogs.
        /// </summary>
        /// <remarks>
        /// Overide this property for things like persistence
        /// Setter must call <c>SaveCommand.NotifyCanExecuteChanged();</c> when this property changes
        /// </remarks>
        public abstract string? FileLocation { get; set; }

        public abstract void CreateFile();

        /// <summary>
        /// Abstract async method that loads the data from the file at <see cref="FileLocation"/>
        /// </summary>
        public abstract Task LoadFileAsync();

        /// <summary>
        /// Abstract async method that saves the data to a file at <see cref="FileLocation"/>
        /// </summary>
        public abstract Task SaveFileAsync();

        #endregion

        #region Open & Load

        /// <summary>
        /// Attempts to synchronously <see cref="LoadFileAsync"/> this object from the file at <see cref="FileLocation"/>
        /// </summary>
        /// <remarks>
        /// Call for autoloading at application startup
        /// </remarks>
        /// <returns>
        /// True if file exists and is isLoaded without error, and false otherwise.
        /// </returns>
        public async Task<bool> TryLoad()
        {
            if (File.Exists(FileLocation))
            {
                try
                {
                    IsModified = false;
                    await LoadFileAsync();
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
        /// Opens a <see cref=" Microsoft.Win32.OpenFileDialog"/> and attempts to load the user-selected file.
        /// </summary>
        /// <remarks>
        /// Checks if <see cref="IsUserReadyToPartWithCurrentFile"/> before executing. Also sets <see cref="FileLocation"/> to the result of the dialog.
        /// </remarks>
        [RelayCommand]
        public async Task Open()
        {
            if (!(await IsUserReadyToPartWithCurrentFile())) return;

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
                FileLocation = dialog.FileName;
                try
                {
                    IsModified = false;
                    await LoadFileAsync();
                }
                catch (Exception e)
                {
                    OnLoadError(e);
                }
            }
        }

        /// <summary>
        /// Repeatedly prompts the user till a valid existing file is opened 
        /// or a new file is made, setting 
        /// </summary>
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
                            IsModified = false;
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
        /// Shows a <see cref="MessageBox"/> for error <paramref name="e"/>. Overide for custom prompt.
        /// </summary>
        /// <param name="e">The error to display</param>
        protected virtual void OnLoadError(Exception e)
        {
            MessageBox.Show($"A {e.GetType().Name} was thrown when attempting to load {Path.GetFileName(FileLocation)}\n" +
                $"Details: {e.Message}", "Could not open file", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region Save

        private bool _IsModified = false;
        /// <summary>
        /// Gets or sets whether the data represented by this <see cref="FileManipulationObject"/> has been modified.
        /// </summary>
        /// <remarks>
        /// Set <c>IsModified = true;</c> in the setter of the property containing the data.
        /// </remarks>
        public bool IsModified
        {
            get => _IsModified;
            set
            {
                if (SetProperty(ref _IsModified, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Returns true if the model is modified and the file exists.
        /// </summary>
        /// <returns></returns>
        public bool CanSave()
        {
            return _IsModified && File.Exists(FileLocation);
        }

        /// <summary>
        /// Calls <see cref="SaveFileAsync"/>
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSave))]
        public async Task Save()
        {
            await SaveFileAsync();
            IsModified = false;
        }

        /// <summary>
        /// Opens a <see cref="Microsoft.Win32.SaveFileDialog"/> and then calls <see cref="SaveFileAsync"/>
        /// </summary>
        /// <remarks>
        /// Also sets <see cref="FileLocation"/> to the result of the dialog.
        /// </remarks>
        [RelayCommand]
        private async Task SaveAs()
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

        /// <summary>
        /// If this file <see cref="IsModified"/>, a <see cref="UnsavedChangesPrmpt"/>
        /// is shown and if the user intends to save, either the <see cref="Save"/> 
        /// or <see cref="SaveAs"/> commands are called.
        /// </summary>
        /// <returns>False if user intends to cancel their command</returns>
        public async Task<bool> IsUserReadyToPartWithCurrentFile()
        {
            if (IsModified)
            {
                MessageBoxResult messageBoxResult = UnsavedChangesPrmpt();

                switch (messageBoxResult)
                {
                    case MessageBoxResult.Cancel:
                        return false;
                    case MessageBoxResult.Yes:
                        if (CanSave())
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
        /// any unsaved changes. Overide for custom prompting behavior.
        /// </summary>
        /// <returns>The result of the <see cref="MessageBoxButton.YesNoCancel"/></returns>
        protected virtual MessageBoxResult UnsavedChangesPrmpt()
        {
            return MessageBox.Show(
                    $"Would you like to save any changes to {Path.GetFileName(FileLocation)}",
                    "Unsaved changes",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
        }

        #endregion
    }
}