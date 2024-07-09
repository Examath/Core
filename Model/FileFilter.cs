using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Model
{
    /// <summary>
    /// Represents an extension filter for file dialogs
    /// </summary>
    public struct FileFilter
    {
        private string _Header;
        private string[] _Patterns;
        private bool _ShowPatternsInHeader;

        /// <summary>
        /// Creates a new file filter of the specified <paramref name="patterns"/>
        /// for <see cref="System.Windows.Forms.FileDialog"/>,
        /// where the <paramref name="header"/> and <paramref name="patterns"/> are displayed to the user.
        /// </summary>
        /// <param name="header">Text displayed to the user. Do not include pattern</param>
        /// <param name="patterns">Filter pattern(s), e.g. <c>*.txt</c></param>
        /// <remarks>
        /// If patterns should not be automatically displayed,
        /// use <see cref="FileFilter.FileFilter(string, bool, string[])"/>
        /// </remarks>
        public FileFilter(string header, params string[] patterns)
        {
            _Header = header;
            _Patterns = patterns;
            _ShowPatternsInHeader = true;
        }

        /// <summary>
        /// Creates a new file filter of the specified <paramref name="patterns"/>
        /// for <see cref="System.Windows.Forms.FileDialog"/>,
        ///  where the <paramref name="header"/>, and <paramref name="patterns"/> are displayed to the user
        ///  if <paramref name="showPattensInHeader"/> is true.
        /// </summary>
        /// <param name="header">Text displayed to the user</param>
        /// <param name="showPattensInHeader">Whether patterns should be shown in the header in brackets</param>
        /// <param name="patterns">Filter pattern(s), e.g. <c>*.txt</c></param>
        public FileFilter(string header, bool showPattensInHeader, params string[] patterns)
        {
            _Header = header;
            _Patterns = patterns;
            _ShowPatternsInHeader = showPattensInHeader;
        }

        /// <summary>
        /// Gets the extension of the first pattern
        /// </summary>
        /// <returns>The extension as a string, including the dot. e.g: <c>.txt</c></returns>
        public readonly string GetFirstExtension()
        {
            if (_Patterns.Length > 0)
            {
                return _Patterns[0].TrimStart('*');
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the string representation of this file filter in the format expected by
        /// the <see cref="System.Windows.Forms.FileDialog"/>
        /// </summary>
        public override readonly string ToString()
        {
            if (_ShowPatternsInHeader)
            {
                return $"{_Header} ({string.Join(", ", _Patterns)})|{string.Join(";", _Patterns)}";
            }
            else
            {
                return $"{_Header}|{string.Join(";", _Patterns)}";
            }
        }

        /// <summary>
        /// Returns the file filter for all files (<c>*.*</c>)
        /// </summary>
        public static readonly FileFilter All = new("All files", false, "*.*");
    }
}
