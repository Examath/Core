using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Examath.Core.Environment
{
    /// <summary>
    /// A dialog that allows the selection of an item from a pickable list
    /// </summary>
    /// <remarks>
    /// Use the static method <see cref="Select(IList, string)"/>
    /// </remarks>
    public partial class Searcher : Window
    {
        private readonly CollectionViewSource _ViewSource;

        /// <summary>
        /// Gets or sets whether this dialog supports multiple selections
        /// </summary>
        /// <remarks>
        /// If true, <see cref="SelectedItems"/> is populated.
        /// </remarks>
        public bool MultiSelect { get; set; } = false;

        /// <summary>
        /// Gets the selected item, if any
        /// </summary>
        public object? SelectedItem { get; private set; }

        /// <summary>
        /// Gets a list of selected items, if <see cref="MultiSelect"/> is enabled
        /// </summary>
        public ObservableCollection<object> SelectedItems { get; private set; } = new();

        /// <summary>
        /// Creates a new searcher dialog
        /// </summary>
        public Searcher()
        {
            InitializeComponent();
            _ViewSource = ((CollectionViewSource)this.FindResource("Source"));
            _ViewSource.Filter += ViewSource_Filter;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBox.Focus();

            if (MultiSelect)
            {
                OkButton.IsEnabled = false;
                View.SelectionMode = SelectionMode.Extended;
            }
        }

        private void ViewSource_Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = e.Item.ToString()?.Contains(SearchBox.Text) ?? false;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (View?.ItemsSource as CollectionView)?.Refresh();
        }

        private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MultiSelect)
            {
                foreach (object item in e.RemovedItems) SelectedItems.Remove(item);
                foreach (object item in e.AddedItems) SelectedItems.Add(item);

                OkButton.IsEnabled = SelectedItems.Count > 0;
            }
            else
            {
                SelectedItem = View.SelectedItem;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Show a dialog to prompt the user to select a single item from <paramref name="dataContext"/>
        /// </summary>
        /// <param name="dataContext">The collection or list to bind to</param>
        /// <param name="title">The title of the dialog window</param>
        /// <returns>The selected object, or null if action is cancelled</returns>
        public static object? Select(IList dataContext, string title = "Select")
        {
            Searcher searcher = new()
            {
                DataContext = dataContext,
                Title = title
            };

            if(searcher.ShowDialog() == true)
            {
                return searcher.SelectedItem;
            }
            else
            {
                return null;
            }
        }

        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            Utils.WindowPosition.MoveToMouseCursor(this);
        }
    }
}
