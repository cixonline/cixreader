// *****************************************************
// CIXReader
// ManageForumGeneral.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 27/02/2015 5:47 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CIXClient;
using CIXClient.Tables;
using CIXReader.Properties;

namespace CIXReader.Forms
{
    public sealed partial class ManageForumGeneral : Form
    {
        private readonly DirForum _forum;

        readonly Dictionary<string, string> forumTypeMap = new Dictionary<string, string>
            {
                {"o", Resources.OpenForum},
                {"c", Resources.ClosedForum},
                {"p", Resources.PrivateForum}
            };

        public ManageForumGeneral(DirForum forum)
        {
            InitializeComponent();
            _forum = forum;
        }

        public void CloseView()
        {
            _forum.Name = forumName.Text;
            _forum.Title = forumTitle.Text;
            _forum.Desc = forumDesc.Text;
        
            _forum.Cat = (string)forumCategory.SelectedItem ?? "Private";
            _forum.Sub = (string)forumSubCategory.SelectedItem ?? "Private";
        
            string selectedType = (string)forumType.Items[forumType.SelectedIndex];
            _forum.Type = forumTypeMap.FirstOrDefault(x => x.Value == selectedType).Key;
        }

        /// <summary>
        /// Load Forum management UI with forum details.
        /// </summary>
        private void ManageForumGeneral_Load(object sender, EventArgs e)
        {
            forumName.Text = _forum.Name;
            forumTitle.Text = _forum.Title;
            forumDesc.Text = _forum.Desc.FixNewlines();

            forumType.Items.Clear();
            foreach (string typeName in forumTypeMap.Values)
            {
                forumType.Items.Add(typeName);
            }
            forumType.SelectedIndex = forumType.Items.IndexOf(forumTypeMap[_forum.Type]);
            HandleForumTypeChange();

            forumCategory.Items.Clear();
            foreach (string category in CIX.DirectoryCollection.Categories)
            {
                forumCategory.Items.Add(category);
            }
            forumCategory.SelectedIndex = forumCategory.Items.IndexOf(_forum.Cat);

            FillSubCategoryList(_forum.Cat);
            forumSubCategory.SelectedIndex = forumSubCategory.Items.IndexOf(_forum.Sub);
        }

        /// <summary>
        /// Update the list of sub-categories based on the selected category
        /// </summary>
        private void FillSubCategoryList(string category)
        {
            forumSubCategory.Items.Clear();

            List<DirCategory> categories = CIX.DirectoryCollection.SubCategoriesByCategoryName(category);
            if (categories != null)
            {
                foreach (DirCategory subCategory in categories)
                {
                    forumSubCategory.Items.Add(subCategory.Sub);
                }
            }
        }

        /// <summary>
        /// Invoked when the user changes the category.
        /// </summary>
        private void forumCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = (string)forumCategory.Items[forumCategory.SelectedIndex];
            FillSubCategoryList(selectedCategory);
            forumSubCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// Invoked when the user changes the forum type.
        /// </summary>
        private void forumType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleForumTypeChange();
        }

        /// <summary>
        /// Handle the change to the forum type.
        /// For private forums we disable the category and sub-category fields.
        /// </summary>
        private void HandleForumTypeChange()
        {
            string selectedType = (string)forumType.Items[forumType.SelectedIndex];

            forumCategory.Enabled = selectedType != Resources.PrivateForum;
            forumSubCategory.Enabled = selectedType != Resources.PrivateForum;
        }
    }
}