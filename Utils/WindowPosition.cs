using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Examath.Core.Utils
{
    /// <summary>
    /// Helper methods to position a window
    /// </summary>
    public static class WindowPosition
    {
        private const double _OFFSET = 24.0;

        // From https://stackoverflow.com/a/19780697/10701111
        /// <summary>
        /// Moves the top-left corner of this window to where the transformedMousePoint cursor is
        /// </summary>
        /// <param name="window">The window to move</param>
        public static void MoveToMouseCursor(Window window)
        {
            // Get the mouse cursor point
            System.Drawing.Point mousePointSystemType = System.Windows.Forms.Control.MousePosition;
            Point mousePoint = new(mousePointSystemType.X, mousePointSystemType.Y);

            // Transform point
            var transform = PresentationSource.FromVisual(window).CompositionTarget.TransformFromDevice;
            var transformedMousePoint = transform.Transform(mousePoint);
            window.Left = transformedMousePoint.X - _OFFSET;
            window.Top = transformedMousePoint.Y - _OFFSET;
        }
    }
}
