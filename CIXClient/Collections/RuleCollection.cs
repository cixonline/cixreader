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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Models;
using CIXClient.Tables;

namespace CIXClient.Collections
{
    /// <summary>
    /// List of combinable action codes for rules
    /// </summary>
    [Flags]
    public enum RuleActionCodes
    {
        /// <summary>
        /// Mark message unread
        /// </summary>
        Unread = 1,

        /// <summary>
        /// Mark message priority
        /// </summary>
        Priority = 2,

        /// <summary>
        /// Mark message ignored
        /// </summary>
        Ignored = 4,

        /// <summary>
        /// Flag message
        /// </summary>
        Flag = 8,

        /// <summary>
        /// Specifies whether the flag should be set or cleared
        /// </summary>
        Clear = 0x1000
    }

    /// <summary>
    /// Defines a collection of rules that are applied to all incoming
    /// forum messages.
    /// </summary>
    public sealed class RuleCollection
    {
        private ArrayList ruleGroups = new ArrayList();

        /// <summary>
        /// Instantiate an instance of the RuleCollection
        /// </summary>
        public RuleCollection()
        {
            LoadRules();
        }

        /// <summary>
        /// Add name to the block list if it is not already present.
        /// </summary>
        /// <param name="name"></param>
        public void Block(string name)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (RuleGroup ruleGroup in ruleGroups)
            {
                if (ruleGroup.rule.Any(rule => rule.property.ToLower() == "author" && rule.value == name))
                {
                    return;
                }
            }
            Expression<Func<CIXMessage, bool>> newCriteria = PredicateBuilder.GetExpression<CIXMessage>(new List<PredicateBuilder.Filter>
            {
                new PredicateBuilder.Filter
                {
                    PropertyName = "Author",
                    Operation = PredicateBuilder.Op.Equals,
                    Value = name
                }
            });
            ruleGroups.Add(new RuleGroup
            {
                group = "and", 
                Criteria = newCriteria,
                actionCode = RuleActionCodes.Unread | RuleActionCodes.Clear,
                rule = new[] { new Rule
                {
                    property = "author",
                    value = name,
                    op = "equals"
                } }
            });
            SaveRules();
        }

        /// <summary>
        /// Save the rules to the rules.xml file.
        /// </summary>
        private bool SaveRules()
        {
            StreamWriter fileStream = null;

            bool success = false;

            string rulesFilename = string.Format("{0}.rules.xml", CIX.Username);
            string filename = Path.Combine(CIX.HomeFolder, rulesFilename);

            try
            {
                fileStream = new StreamWriter(filename, false);
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true,
                    NewLineOnAttributes = true
                };

                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof(rules));
                    serializer.Serialize(writer, new rules
                    {
                        Items = (RuleGroup[]) ruleGroups.ToArray(typeof (RuleGroup))
                    });
                }

                LogFile.WriteLine("Saved rules to {0}", filename);
                success = true;
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot save rules to {0} : {1}", filename, e.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
            return success;
        }

        /// <summary>
        /// Load rules from the rules.xml file.
        /// </summary>
        private void LoadRules()
        {
            StreamReader fileStream = null;
            rules uiConfig;

            string rulesFilename = string.Format("{0}.rules.xml", CIX.Username);
            string filename = Path.Combine(CIX.HomeFolder, rulesFilename);

            try
            {
                fileStream = new StreamReader(filename);
                using (XmlReader reader = XmlReader.Create(fileStream))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof(rules));
                    uiConfig = (rules)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error parsing rules file {0} : {1}", filename, e.Message);
                uiConfig = null;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }

            // Build the predicates and attach to each ruleGroup.
            if (uiConfig != null)
            {
                ruleGroups = new ArrayList();
                foreach (RuleGroup ruleGroup in uiConfig.Items)
                {
                    List<PredicateBuilder.Filter> filters = new List<PredicateBuilder.Filter>();
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (Rule rule in ruleGroup.rule)
                    {
                        PredicateBuilder.Filter filter = new PredicateBuilder.Filter
                        {
                            PropertyName = rule.property,
                            Operation = PredicateBuilder.Op.Equals,
                            Value = rule.value
                        };
                        filters.Add(filter);
                    }
                    Expression<Func<CIXMessage, bool>> newCriteria = PredicateBuilder.GetExpression<CIXMessage>(filters);
                    ruleGroup.Criteria = newCriteria;
                    ruleGroups.Add(ruleGroup);
                }
            }
        }

        /// <summary>
        /// Hardcoded rules later to be replaced by the rules engine ported from the
        /// OSX version.
        /// </summary>
        /// <param name="message">Message to which rules are applied</param>
        internal void ApplyRules(CIXMessage message)
        {
            CIXMessage parentMessage = message.Parent;
            foreach (RuleGroup ruleGroup in ruleGroups)
            {
                Func<CIXMessage, bool> evaluateCriteria = ruleGroup.Criteria.Compile();
                if (evaluateCriteria(message))
                {
                    bool isClear = ruleGroup.actionCode.HasFlag(RuleActionCodes.Clear);
                    if (ruleGroup.actionCode.HasFlag(RuleActionCodes.Unread))
                    {
                        if (!message.ReadLocked)
                        {
                            bool oldState = message.Unread;
                            message.Unread = !isClear;
                            if (oldState != message.Unread)
                            {
                                message.ReadPending = true;
                                Folder folder = CIX.FolderCollection[message.TopicID];
                                folder.MarkReadRangePending = true;
                            }
                        }
                    }
                }
            }
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