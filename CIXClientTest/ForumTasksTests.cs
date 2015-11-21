// *****************************************************
// CIXReader
// ForumTasksTests.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/04/2015 11:16
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CIXClient;
using CIXClient.Collections;
using CIXClient.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIXClientTest
{
    [TestClass]
    public sealed class ForumTasksTests
    {
        [TestMethod]
        public void TestForumsFastSync()
        {
            string databasePath = Path.GetTempFileName();
            CIX.Init(databasePath);

            Folder forumFolder = new Folder
            {
                ParentID = -1, 
                Name = "cix.beta",
                Unread = 0, 
                UnreadPriority = 0
            };
            CIX.FolderCollection.Add(forumFolder);

            Folder topicFolder = new Folder
            {
                Name = "cixreader", 
                ParentID = forumFolder.ID,
                Unread = 0, 
                UnreadPriority = 0
            };
            CIX.FolderCollection.Add(topicFolder);

            // Seed message as fast sync doesn't work on empty topics
            CIXMessage seedMessage = new CIXMessage
            {
                RemoteID = 1,
                Author = "CIX", 
                Body = "Seed message", 
                CommentID = 0
            };
            topicFolder.Messages.Add(seedMessage);

            string userSyncData = Resource1.UserSyncData;
            DateTime sinceDate = default(DateTime);
            FolderCollection.AddMessages(Utilities.GenerateStreamFromString(userSyncData), ref sinceDate, true, false);

            // On completion, verify that there are no duplicates in the topic
            List<CIXMessage> messages = topicFolder.Messages.OrderBy(fld => fld.RemoteID).ToList();
            int lastMessageID = -1;
            foreach (CIXMessage cixMessage in messages)
            {
                Assert.AreNotEqual(cixMessage.RemoteID, 0);
                Assert.AreNotEqual(cixMessage.RemoteID, lastMessageID);
                Assert.AreNotEqual(cixMessage.ID, 0);
                lastMessageID = cixMessage.RemoteID;
            }

            // Verify that specific messages are marked read
            CIXMessage message = topicFolder.Messages.MessageByID(2323);
            Assert.IsNotNull(message);
            Assert.IsFalse(message.Unread);

            // Verify that specific messages are marked unread
            message = topicFolder.Messages.MessageByID(2333);
            Assert.IsNotNull(message);
            Assert.IsTrue(message.Unread);

            // Verify the total unread on the folder and globally
            Assert.AreEqual(topicFolder.Unread, 1);
            Assert.AreEqual(topicFolder.UnreadPriority, 6);
            Assert.AreEqual(CIX.FolderCollection.TotalUnread, 1);
            Assert.AreEqual(CIX.FolderCollection.TotalUnreadPriority, 6);
        }
    }
}