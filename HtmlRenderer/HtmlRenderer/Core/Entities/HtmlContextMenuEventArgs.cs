// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System;
using System.Collections.Generic;
using TheArtOfDev.HtmlRenderer.Adapters;

namespace TheArtOfDev.HtmlRenderer.Core.Entities
{
    /// <summary>
    /// Raised when the user clicks on a link in the html.
    /// </summary>
    public sealed class HtmlContextMenuEventArgs : EventArgs
    {
        /// <summary>
        /// the link href that was clicked
        /// </summary>
        private readonly RContextMenu _contextMenu;

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="link">the context menu</param>
        public HtmlContextMenuEventArgs(RContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
        }

        /// <summary>
        /// the context menu
        /// </summary>
        public RContextMenu ContextMenu
        {
            get { return _contextMenu; }
        }

        public override string ToString()
        {
            return string.Format("Context Menu: {0}", ContextMenu);
        }
    }
}