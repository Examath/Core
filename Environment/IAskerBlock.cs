using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Examath.Core.Environment
{
    /// <summary>
    /// Represents a block in the <see cref="Asker"/> dialog
    /// </summary>
    public interface IAskerBlock
    {
        internal Control GetControl();

        internal void Finish(Control control);
    }

    /// <summary>
    /// Outputs an <see cref="object"/>, as a <see cref="Label"/>, in the <see cref="Asker"/> dialog
    /// </summary>
    public class AskerNote : IAskerBlock
    {
        private object _Content;
        private string _ContentStringFormat;

        /// <summary>
        /// Defines an <see cref="AskerNote"/> with the specified <paramref name="content"/>
        /// </summary>
        /// <param name="content">The content of the displayed <see cref="Label"/></param>
        /// <param name="contentStringFormat">Sets the <see cref="Label.ContentStringFormat"/> of the displayed <see cref="Label"/></param>
        public AskerNote(object content, string contentStringFormat = "")
        {
            _Content = content;
            _ContentStringFormat = contentStringFormat;
        }

        public Control GetControl()
        {
            Label label = new()
            {
                Content = _Content,
            };
            if (_ContentStringFormat != string.Empty) label.ContentStringFormat = _ContentStringFormat;
            return label;
        }

        public void Finish(Control control)
        {

        }
    }

    public class AskerGroup : IAskerBlock
    {
        private AskerGroupOptions _AskerGroupOptions;
        private IAskerBlock[] _AskerBlocks;

        public AskerGroup(AskerGroupOptions? askerGroupOptions, params IAskerBlock[] askerBlocks)
        {
            _AskerGroupOptions = askerGroupOptions ?? new();
            _AskerBlocks = askerBlocks;
        }

        Control IAskerBlock.GetControl()
        {
            ListBox listBox = new();
            listBox.SetValue(Grid.IsSharedSizeScopeProperty, false);
            PopulateBlocks(listBox, _AskerBlocks);
            Expander expander = new()
            {
                Header = _AskerGroupOptions.Header,
                IsExpanded = _AskerGroupOptions.IsExpanded,
                Content = listBox,
                Margin = new(1),
            };
            return expander;
        }

        void IAskerBlock.Finish(Control control)
        {
            FinishBlocks((ListBox)((Expander)control).Content, _AskerBlocks);
        }

        public static void PopulateBlocks(ListBox listBox, IAskerBlock[] _AskerBlocks)
        {
            foreach (IAskerBlock askerBlock in _AskerBlocks)
            {
                listBox.Items.Add(askerBlock.GetControl());
            }
        }

        public static void FinishBlocks(ListBox listbox, IAskerBlock[] _AskerBlocks)
        {
            if (listbox.Items.Count == _AskerBlocks.Length)
            {
                for (int i = 0; i < _AskerBlocks.Length; i++)
                {
                    _AskerBlocks[i].Finish((Control)listbox.Items[i]);
                }
            }
        }
    }

    public class AskerGroupOptions
    {
        public string Header { get; private set; }
        public bool IsExpanded { get; private set; }

        public AskerGroupOptions(string header = "Group", bool isExpanded = false)
        {
            Header = header;
            IsExpanded = isExpanded;
        }
    }
}
