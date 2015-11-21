// *****************************************************
// CIXReader
// TaggedMessage.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 15/05/2015 21:50
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXClient;
using CIXClient.Tables;

namespace CIXReader.Utilities
{
    public static class TaggedMessage
    {
        /// <summary>
        /// Return the string representing the unformatted body
        /// </summary>
        public static string UnformattedText(this CIXMessage message)
        {
            return message.Body;
        }

        /// <summary>
        /// Returns a custom image tag representing the message author. This will
        /// have been resolved earlier in the HTML load code that maps image tags
        /// to actual images.
        /// </summary>
        public static string tagImage(this CIXMessage message, StyleController styleController)
        {
            return styleController.ImageFromTag(message.Author);
        }

        /// <summary>
        /// Returns the message remote ID.
        /// </summary>
        public static string tagID(this CIXMessage message, StyleController styleController)
        {
            return string.Format("{0}", message.RemoteID);
        }

        /// <summary>
        /// Returns the message comment ID.
        /// </summary>
        public static string tagCommentID(this CIXMessage message, StyleController styleController)
        {
            return message.CommentID > 0 ? string.Format("{0}", message.CommentID) : string.Empty;
        }

        /// <summary>
        /// Returns the message body.
        /// </summary>
        public static string tagBody(this CIXMessage message, StyleController styleController)
        {
            return styleController.HtmlFromTag(message.Body);
        }

        /// <summary>
        /// Returns the message author.
        /// </summary>
        public static string tagAuthor(this CIXMessage message, StyleController styleController)
        {
            return styleController.StringFromTag(message.Author);
        }

        /// <summary>
        /// Returns the message date.
        /// </summary>
        public static string tagDate(this CIXMessage message, StyleController styleController)
        {
            return styleController.DateFromTag(message.Date);
        }

        /// <summary>
        /// Returns the message subject.
        /// </summary>
        public static string tagSubject(this CIXMessage message, StyleController styleController)
        {
            return styleController.StringFromTag(message.Subject.FirstNonBlankLine());
        }

        /// <summary>
        /// Returns the message reply.
        /// </summary>
        public static string tagInReplyTo(this CIXMessage message, StyleController styleController)
        {
            return message.CommentID > 0 ? string.Format("cix:{0}/{1}:{2}", message.Forum.Name, message.Topic.Name, message.CommentID) : string.Empty;
        }

        /// <summary>
        /// Returns the message address.
        /// </summary>
        public static string tagLink(this CIXMessage message, StyleController styleController)
        {
            return string.Format("cix:{0}/{1}:{2}", message.Forum.Name, message.Topic.Name, message.RemoteID);
        }
    }
}