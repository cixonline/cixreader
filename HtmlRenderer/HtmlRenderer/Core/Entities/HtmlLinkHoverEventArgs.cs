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
using System.Reflection;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;

namespace TheArtOfDev.HtmlRenderer.Core.Entities
{
    /// <summary>
    /// Raised when the user hovers on a link in the html.
    /// </summary>
    public sealed class HtmlLinkHoverEventArgs : EventArgs
    {
        /// <summary>
        /// the link href that was hovered over
        /// </summary>
        private readonly string _link;

        /// <summary>
        /// the location that was hovered over
        /// </summary>
        private readonly RPoint _location;

        /// <summary>
        /// collection of all the attributes that are defined on the link element
        /// </summary>
        private readonly Dictionary<string, string> _attributes;

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="link">the link href that was hovered</param>
        public HtmlLinkHoverEventArgs(string link, RPoint location, Dictionary<string, string> attributes) 
        {
            _link = link;
            _location = location;
            _attributes = attributes;
        }

        /// <summary>
        /// the link href that was hovered
        /// </summary>
        public string Link 
        {
            get { return _link; }
        }

        /// <summary>
        /// collection of all the attributes that are defined on the link element
        /// </summary>
        public Dictionary<string, string> Attributes 
        {
            get { return _attributes; }
        }

        /// <summary>
        /// location of the hover
        /// </summary>
        public RPoint Location
        {
            get { return _location; }
        }

        public override string ToString() 
        {
            return string.Format("Link: {0}", _link);
        }
    }
}