// *****************************************************
// CIXReader
// CRSortOrder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 05/02/2015 14:01
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using CIXReader.Utilities;

namespace CIXReader.Controls
{
    /// <summary>
    /// Class that manages the sorting of items within a view.
    /// </summary>
    public sealed class CRSortOrder
    {
        /// <summary>
        /// Delegate to handle ordering changes
        /// </summary>
        public delegate void OrderingChangedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Controls how the messages in the topic view are ordered.
        /// </summary>
        public enum SortOrder
        {
            /// <summary>
            /// No sort ordering
            /// </summary>
            None,

            /// <summary>
            /// Order by conversation
            /// </summary>
            Conversation,

            /// <summary>
            /// Order by date
            /// </summary>
            Date,

            /// <summary>
            /// Order by author
            /// </summary>
            Author,

            /// <summary>
            /// Order by subject
            /// </summary>
            Subject,

            /// <summary>
            /// Order by popularity
            /// </summary>
            Popularity,

            /// <summary>
            /// Order by description
            /// </summary>
            Title,

            /// <summary>
            /// Order by subcategory
            /// </summary>
            SubCategory,

            /// <summary>
            /// Order by name
            /// </summary>
            Name
        }

        private SortOrder _currentSortOrder = SortOrder.Date;
        private bool _ascending;
        private bool _sortOrderInitialised;
        private readonly string _className;

        /// <summary>
        /// Initialises a new instance of the <see cref="CRSortOrder"/> class
        /// </summary>
        /// <param name="className"></param>
        public CRSortOrder(string className)
        {
            _className = className;
        }

        /// <summary>
        /// Provide the default sort ordering
        /// </summary>
        public SortOrder DefaultSortOrder { get; set; }

        /// <summary>
        /// Change to the sort direction
        /// </summary>
        public bool Ascending
        {
            get
            {
                if (!_sortOrderInitialised)
                {
                    InitialiseSort();
                }
                return _ascending;
            }
            set
            {
                if (value != _ascending)
                {
                    _ascending = value;
                    if (OrderingChanged != null)
                    {
                        OrderingChanged(this, new EventArgs());
                    }

                    string settingName = string.Format("{0}SortAscending", _className);
                    Preferences.WriteBoolean(settingName, _ascending);
                }
            }
        }

        /// <summary>
        /// Change to the sort order
        /// </summary>
        public SortOrder Order
        {
            get
            {
                if (!_sortOrderInitialised)
                {
                    InitialiseSort();
                }
                return _currentSortOrder;
            }
            set
            {
                if (value != _currentSortOrder)
                {
                    _currentSortOrder = value;
                    if (OrderingChanged != null)
                    {
                        OrderingChanged(this, new EventArgs());
                    }
                    string settingName = string.Format("{0}SortOrder", _className);
                    Preferences.WriteInteger(settingName, (int)_currentSortOrder);
                }
            }
        }

        private void InitialiseSort()
        {
            string settingName = string.Format("{0}SortAscending", _className);
            _ascending = Preferences.ReadBoolean(settingName);

            settingName = string.Format("{0}SortOrder", _className);
            _currentSortOrder = (SortOrder)Preferences.ReadInteger(settingName, (int)DefaultSortOrder);

            // From 1.55, conversation is no longer a sort order but a grouping.
            if (_currentSortOrder == SortOrder.Conversation)
            {
                _currentSortOrder = SortOrder.Date;
            }

            _sortOrderInitialised = true;
        }

        /// <summary>
        /// Event fired when the sort order has been changed programmatically or
        /// by the user.
        /// </summary>
        public event OrderingChangedEventHandler OrderingChanged;
    }
}