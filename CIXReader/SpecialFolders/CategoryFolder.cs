// *****************************************************
// CIXReader
// CategoryFolder.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 08/06/2015 19:24
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Generic;
using System.Drawing;
using CIXClient;
using CIXReader.Properties;

namespace CIXReader.SpecialFolders
{
    /// <summary>
    /// Implements a folder tree item for directory categories
    /// </summary>
    public sealed class CategoryFolder : FolderBase
    {
        private static Dictionary<string, Image> _categoryImageMap;
        private Image _image;

        /// <summary>
        /// Initialises a new instance of the <see cref="CategoryFolder"/> class
        /// </summary>
        public CategoryFolder()
        {
            if (_categoryImageMap == null)
            {
                _categoryImageMap = new Dictionary<string, Image>
                {
                    {"Arts", Resources.ArtsCategory},
                    {"Business", Resources.BusinessCategory},
                    {"CIX", Resources.CIXCategory},
                    {"Computers", Resources.ComputersCategory},
                    {"Games", Resources.GamesCategory},
                    {"Health", Resources.HealthCategory},
                    {"Home & Family", Resources.HomeCategory},
                    {"Kids and Teens", Resources.KidsCategory},
                    {"Money", Resources.MoneyCategory},
                    {"News", Resources.NewsCategory},
                    {"Reference", Resources.ReferenceCategory},
                    {"Science & Technology", Resources.ScienceCategory},
                    {"Shopping", Resources.ShoppingCategory},
                    {"Society", Resources.SocietyCategory},
                    {"Sports", Resources.SportsCategory},
                    {"Travel & Transport", Resources.TravelCategory}
                };
            }
        }

        /// <summary>
        /// This is a directory view folder.
        /// </summary>
        public override AppView ViewForFolder
        {
            get { return AppView.AppViewDirectory; }
        }

        /// <summary>
        /// Return the addressable location of this folder.
        /// </summary>
        public override string Address
        {
            get
            {
                return Name == AllCategoriesName ? "cixdirectory:all" : string.Format("cixdirectory:{0}", Name);
            }
        }

        /// <summary>
        /// Return the icon for the specified category.
        /// </summary>
        public static Image IconForCategory(string categoryName)
        {
            Image image;

            if (!_categoryImageMap.TryGetValue(categoryName, out image))
            {
                image = _categoryImageMap["CIX"];
            }
            return image;
        }

        /// <summary>
        /// Return the Image that corresponds to the specified category name.
        /// </summary>
        public override Image Icon
        {
            get
            {
                if (_image == null)
                {
                    if (!_categoryImageMap.TryGetValue(Name, out _image))
                    {
                        _image = Resources.Categories;
                    }
                    _image = _image.ResizeImage(12, 12);
                }
                return _image;
            }
        }

        /// <summary>
        /// Return the localised name for all categories
        /// </summary>
        public static string AllCategoriesName
        {
            get { return Resources.AllCategories; }
        }

        /// <summary>
        /// Refresh all categories. This will run asynchronously so it
        /// will return immediately and completion will occur when the
        /// DirectoryChanged event is fired.
        /// </summary>
        public override void Refresh()
        {
            CIX.DirectoryCollection.Refresh();
        }

        /// <summary>
        /// Category folders allow scoped searches
        /// </summary>
        public override bool AllowsScopedSearch
        {
            get { return true; }
        }
    }
}