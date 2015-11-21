// *****************************************************
// CIXReader
// RuleCollection.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/02/2015 9:11 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// Defines a collection of rules that are applied to all incoming
    /// forum messages.
    /// </summary>
    public sealed class RuleCollection
    {
        /// <summary>
        /// Hardcoded rules later to be replaced by the rules engine ported from the
        /// OSX version.
        /// </summary>
        /// <param name="message">Message to which rules are applied</param>
        internal static void ApplyRules(CIXMessage message)
        {
            CIXMessage parentMessage = message.Parent;
            if (message.IsMine)
            {
                message.Priority = true;
            }
            if (message.IsWithdrawn && message.Unread)
            {
                message.Unread = false;
                message.ReadPending = true;
            }
            if (parentMessage != null)
            {
                if (parentMessage.Ignored)
                {
                    message.Ignored = true;
                    if (message.Unread)
                    {
                        message.Unread = false;
                        message.ReadPending = true;
                    }
                }
                if (parentMessage.Priority)
                {
                    message.Priority = true;
                }
            }
        }
    }
}