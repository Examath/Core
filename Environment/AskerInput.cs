using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Examath.Core.Enviorment
{
    public abstract class AskerInput
    {
        private string _Label;
        private string _HelpText;

        public AskerInput(string label = "", string helpText = "")
        {
            _Label = label;
            _HelpText = helpText;
        }

        protected Control InitialiseAttributes(Control control)
        {
            if(_Label != string.Empty) control.Tag = _Label;
            return control;
        }

        public abstract Control GetControl();
    }



    /// <summary>
    /// Represents a question that uses a <see cref="TextBox"/> in the <see cref="Asker"/> dialog
    /// </summary>
    public class TextlBoxInput : AskerInput
    {
        /// <summary>
        /// Creates a question meant for a a <see cref="TextBox"/>
        /// </summary>
        /// <param name="source">Source of binding</param>
        /// <param name="property">Property to bind</param>
        /// <param name="label">Optional label</param>
        /// <param name="helpText">Optional help text</param>
        public TextBoxInput(string label = "", string helpText = "") : base(label, helpText)
        {
        }

        public override Control GetControl()
        {
            return InitialiseAttributes(new TextBox());
        }
    }
}
