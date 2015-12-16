// *****************************************************
// CIXReader
// ActionID.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 13/10/2013 4:04 PM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

namespace CIXReader.Utilities
{
    /// <summary>
    /// Action ID values
    /// </summary>
    public enum ActionID
    {
        /// <summary>
        /// No action ID.
        /// </summary>
        None = -1,

        /// <summary>
        /// Author name.
        /// </summary>
        Profile = 0,

        /// <summary>
        /// Priority field.
        /// </summary>
        Priority,

        /// <summary>
        /// Ignore field.
        /// </summary>
        Ignore,

        /// <summary>
        /// Star field.
        /// </summary>
        Star,

        /// <summary>
        /// Reply field.
        /// </summary>
        Reply,

        /// <summary>
        /// Read field.
        /// </summary>
        Read,

        /// <summary>
        /// Edit field.
        /// </summary>
        Edit,

        /// <summary>
        /// Expand or collapse a thread.
        /// </summary>
        Expand,

        /// <summary>
        /// Author mugshot.
        /// </summary>
        AuthorImage,

        /// <summary>
        /// Resign Forum.
        /// </summary>
        ResignForum,

        /// <summary>
        /// Join Forum. 
        /// </summary>
        JoinForum,

        /// <summary>
        /// Withdraw a message.
        /// </summary>
        Withdraw,

        /// <summary>
        /// User's location.
        /// </summary>
        Location,

        /// <summary>
        /// User's e-mail address.
        /// </summary>
        Email,

        /// <summary>
        /// Delete a selected item.
        /// </summary>
        Delete,

        /// <summary>
        /// Go to the original message.
        /// </summary>
        Original,

        /// <summary>
        /// Go to the next unread.
        /// </summary>
        NextUnread,

        /// <summary>
        /// Go to the next priority unread message.
        /// </summary>
        NextPriorityUnread,

        /// <summary>
        /// Create a new message.
        /// </summary>
        NewMessage,

        /// <summary>
        /// Mark the thread as read.
        /// </summary>
        MarkThreadRead,

        /// <summary>
        /// Mark the thread as read then go to the next root.
        /// </summary>
        MarkThreadReadThenRoot,

        /// <summary>
        /// Go to the next root message.
        /// </summary>
        NextRoot,

        /// <summary>
        /// View the Accounts page.
        /// </summary>
        ViewAccounts,

        /// <summary>
        /// Go to the previous message in the backtrack queue.
        /// </summary>
        BackTrack,

        /// <summary>
        /// Go forward to the next message in the backtrack queue.
        /// </summary>
        GoForward,

        /// <summary>
        /// Initiate a chat session.
        /// </summary>
        Chat,

        /// <summary>
        /// Copy the selection to the clipboard.
        /// </summary>
        Copy,

        /// <summary>
        /// Page down the message pane.
        /// </summary>
        PageMessage,

        /// <summary>
        /// Go to the source message.
        /// </summary>
        GoToSource,

        /// <summary>
        /// Copy a link to the message to the clipboard
        /// </summary>
        Link,

        /// <summary>
        /// Go to a specified message
        /// </summary>
        GoTo,

        /// <summary>
        /// Set focus to the search field.
        /// </summary>
        Search,

        /// <summary>
        /// Run a Lua script.
        /// </summary>
        Script,

        /// <summary>
        /// Read-lock a message
        /// </summary>
        ReadLock,

        /// <summary>
        /// Mark an entire topic as read
        /// </summary>
        MarkTopicRead,

        /// <summary>
        /// Drop down the system menu
        /// </summary>
        SystemMenu,

        /// <summary>
        /// Display the previous root message
        /// </summary>
        Root,

        /// <summary>
        /// Refresh the current folder
        /// </summary>
        Refresh,

        /// <summary>
        /// Select all text in the message pane
        /// </summary>
        SelectAll,

        /// <summary>
        /// Reply quoting the original text.
        /// </summary>
        Quote,

        /// <summary>
        /// Print the current message
        /// </summary>
        Print,

        /// <summary>
        /// Display participants list
        /// </summary>
        Participants,

        /// <summary>
        /// Manage the current forum
        /// </summary>
        ManageForum,

        /// <summary>
        /// Toggle markdown display on and off
        /// </summary>
        Markdown,

        /// <summary>
        /// Display the change log
        /// </summary>
        ViewChangeLog,

        /// <summary>
        /// Reply to a message by mail
        /// </summary>
        ReplyByMail,

        /// <summary>
        /// Display the Help file
        /// </summary>
        KeyboardHelp,

        /// <summary>
        /// Placeholder for flexible space
        /// </summary>
        FlexibleSpace,

        /// <summary>
        /// Placeholder for space
        /// </summary>
        Space,

        /// <summary>
        /// Toggle online/offline state
        /// </summary>
        Offline,

        /// <summary>
        /// Block the selected user
        /// </summary>
        Block
    }
}