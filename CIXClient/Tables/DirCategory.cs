// *****************************************************
// CIXReader
// DirCategory.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/09/2013 11:28 AM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXClient.Database;

namespace CIXClient.Tables
{
    /// <summary>
    /// The DirCategory table lists all categories and subcategories.
    /// </summary>
    public sealed class DirCategory
    {
        /// <summary>
        /// Gets or sets the auto-generated ID of this category.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the sub-category name.
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Gets or sets the category to which this sub-category belongs.
        /// </summary>
        public string Name { get; set; }
    }
}