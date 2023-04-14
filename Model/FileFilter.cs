using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examath.Core.Model
{
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
        /// <param name="patterns">Filter pattern(s)</param>
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
        /// <param name="patterns">Filter pattern(s)</param>
        public FileFilter(string header, bool showPattensInHeader, params string[] patterns)
        {
            _Header = header;
            _Patterns = patterns;
            _ShowPatternsInHeader = showPattensInHeader;
        }

        public override string ToString()
        {
            if (_ShowPatternsInHeader) return $"{_Header} ({string.Join(", ", _Patterns)})|{string.Join(";", _Patterns)}";
            else return $"{_Header}|{string.Join(";", _Patterns)}";
        }
    }
}
