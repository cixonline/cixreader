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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Serialization;
using CIXClient.Models;
using CIXClient.Properties;
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
    /// Specifies how the rules in the group relate for the rule
    /// to be considered matched.
    /// </summary>
    public enum RuleGroupType
    {
        /// <summary>
        /// Match any of the rules in the group
        /// </summary>
        Any,

        /// <summary>
        /// Must match all of the rules in the group
        /// </summary>
        All
    }

    /// <summary>
    /// Defines a collection of rules that are applied to all incoming
    /// forum messages.
    /// </summary>
    public sealed class RuleCollection
    {
        private List<RuleGroup> ruleGroups = new List<RuleGroup>();

        /// <summary>
        /// Instantiate an instance of the RuleCollection
        /// </summary>
        public RuleCollection()
        {
            LoadDefaultRules();
            LoadRules();
            CompileRules();
        }

        /// <summary>
        /// Return an array of all rules, both active and inactive
        /// </summary>
        public List<RuleGroup> AllRules
        {
            get { return ruleGroups; }
        }

        /// <summary>
        /// Add the specified rule group to the list.
        /// </summary>
        public void AddRule(RuleGroup newRuleGroup)
        {
            ruleGroups.Add(newRuleGroup);
            CompileRules();
            Save();
        }

        /// <summary>
        /// Add name to the block list if it is not already present.
        /// </summary>
        /// <param name="name">Name of user to block</param>
        public void Block(string name)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (RuleGroup ruleGroup in ruleGroups)
            {
                if (ruleGroup.rule.Any(rule => rule.property == "Author" && rule.value.ToString() == name))
                {
                    return;
                }
            }
            RuleGroup newRuleGroup = new RuleGroup
            {
                type = RuleGroupType.Any,
                title = string.Format(Resources.BlockFrom, name),
                active = true,
                actionCode = RuleActionCodes.Unread | RuleActionCodes.Clear,
                rule = new[]
                {
                    new Rule
                    {
                        property = "Author",
                        value = name,
                        op = PredicateBuilder.Op.Equals
                    }
                }
            };
            AddRule(newRuleGroup);

            FolderCollection.ApplyRules(newRuleGroup);
        }

        /// <summary>
        /// Delete the specified rule.
        /// </summary>
        /// <param name="ruleGroup">Rule to remove</param>
        public void DeleteRule(RuleGroup ruleGroup)
        {
            ruleGroups.Remove(ruleGroup);
        }

        /// <summary>
        /// Save the rules to the rules.xml file.
        /// </summary>
        public void Save()
        {
            StreamWriter fileStream = null;

            string rulesFilename = string.Format("{0}.rules2.xml", CIX.Username);
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
                    XmlSerializer serializer = new XmlSerializer(typeof(rules));
                    serializer.Serialize(writer, new rules
                    {
                        Items = ruleGroups.ToArray()
                    });
                }

                LogFile.WriteLine("Saved rules to {0}", filename);
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot save rules to {0} : {1}", filename, e.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Reset all rules back to the default
        /// </summary>
        public void Reset()
        {
            LoadDefaultRules();
            CompileRules();

            string rulesFilename = string.Format("{0}.rules2.xml", CIX.Username);
            string filename = Path.Combine(CIX.HomeFolder, rulesFilename);

            try
            {
                File.Delete(filename);
            }
            catch (IOException e)
            {
                LogFile.WriteLine("Error deleting rule file {0} : {1}", filename, e.Message);
            }
        }

        /// <summary>
        /// Compile any missing Criteria for each rule.
        /// </summary>
        private void CompileRules()
        {
            foreach (RuleGroup ruleGroup in ruleGroups.Where(ruleGroup => ruleGroup.Criteria == null))
            {
                try
                {
                    List<PredicateBuilder.Filter> filters = new List<PredicateBuilder.Filter>();
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (Rule rule in ruleGroup.rule)
                    {
                        List<string> propertyNames = rule.property.Split('.').ToList();
                        if (propertyNames[0] == "Parent")
                        {
                            propertyNames[0] = "SafeParent";
                        }
                        PredicateBuilder.Filter filter = new PredicateBuilder.Filter
                        {
                            PropertyName = string.Join(".", propertyNames),
                            Operation = PredicateBuilder.Op.Equals,
                            Value = rule.value
                        };
                        filters.Add(filter);
                    }
                    Expression<Func<CIXMessage, bool>> newCriteria = PredicateBuilder.GetExpression<CIXMessage>(ruleGroup.type, filters);
                    ruleGroup.Criteria = newCriteria;
                }
                catch (Exception e)
                {
                    LogFile.WriteLine("Error compiling rulegroup {0} : {1}", ruleGroup, e.Message);
                }
            }
        }

        /// <summary>
        /// Load the default set of rules
        /// </summary>
        private void LoadDefaultRules()
        {
            ruleGroups = new List<RuleGroup>
            {
                // Mark messages priority if they're from me or the parent
                // message is also priority
                new RuleGroup
                {
                    type = RuleGroupType.Any,
                    title = "Priority",
                    active = true,
                    actionCode = RuleActionCodes.Priority,
                    rule = new[]
                    {
                        new Rule
                        {
                            property = "IsMine",
                            value = true,
                            op = PredicateBuilder.Op.Equals
                        },
                        new Rule
                        {
                            property = "Parent.Priority",
                            value = true,
                            op = PredicateBuilder.Op.Equals
                        }
                    }
                },

                // Mark this message as read if it is withdrawn
                new RuleGroup
                {
                    type = RuleGroupType.Any,
                    title = "Withdrawn",
                    active = true,
                    actionCode = RuleActionCodes.Unread | RuleActionCodes.Clear,
                    rule = new[]
                    {
                        new Rule
                        {
                            property = "IsWithdrawn",
                            value = true,
                            op = PredicateBuilder.Op.Equals
                        }
                    }
                },

                // Mark this message ignored if the parent is ignored
                new RuleGroup
                {
                    type = RuleGroupType.Any,
                    title = "Ignored",
                    active = true,
                    actionCode = RuleActionCodes.Ignored,
                    rule = new[]
                    {
                        new Rule
                        {
                            property = "Parent.Ignored",
                            value = true,
                            op = PredicateBuilder.Op.Equals
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Load rules from the rules.xml file.
        /// </summary>
        private void LoadRules()
        {
            string rulesFilename = string.Format("{0}.rules2.xml", CIX.Username);
            string filename = Path.Combine(CIX.HomeFolder, rulesFilename);

            if (File.Exists(filename))
            {
                StreamReader fileStream = null;

                try
                {
                    fileStream = new StreamReader(filename);
                    using (XmlReader reader = XmlReader.Create(fileStream))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof (rules));
                        rules rules = (rules) serializer.Deserialize(reader);

                        ruleGroups = new List<RuleGroup>();
                        foreach (RuleGroup ruleGroup in rules.Items)
                        {
                            ruleGroups.Add(ruleGroup);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogFile.WriteLine("Error parsing rules file {0} : {1}", filename, e.Message);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Apply all rules to the specified message
        /// </summary>
        /// <param name="message">Message to which rules are applied</param>
        /// <returns>True if any rule changed the message, false otherwise</returns>
        internal Void ApplyRules(CIXMessage message)
        {
            ruleGroups.Aggregate(false, (current, ruleGroup) => current || ApplyRule(ruleGroup, message));
        }

        /// <summary>
        /// Apply the specified rule group to the message. On completion, the flags
        /// in the message will be adjusted as per the rule.
        /// 
        /// The caller must persist both the message and the associated folder back
        /// to the database if the function returns true since both will likely have
        /// been altered.
        /// </summary>
        /// <param name="ruleGroup">Rule group to apply</param>
        /// <param name="message">CIXMessage to which rule is applied</param>
        /// <returns>True if the rule changed the message, false otherwise</returns>
        internal static bool ApplyRule(RuleGroup ruleGroup, CIXMessage message)
        {
            Func<CIXMessage, bool> evaluateCriteria = ruleGroup.Criteria.Compile();
            bool changed = false;
            if (ruleGroup.active && evaluateCriteria(message))
            {
                bool isUnread = message.Unread;
                bool isClear = ruleGroup.actionCode.HasFlag(RuleActionCodes.Clear);

                if (ruleGroup.actionCode.HasFlag(RuleActionCodes.Unread))
                {
                    if (!message.ReadLocked)
                    {
                        message.Unread = !isClear;
                        if (isUnread != message.Unread)
                        {
                            message.ReadPending = true;
                            Folder folder = CIX.FolderCollection[message.TopicID];
                            folder.Unread += (message.Unread) ? 1 : -1;
                            if (message.Priority)
                            {
                                folder.UnreadPriority += (message.Unread) ? 1 : -1;
                            }
                            folder.MarkReadRangePending = true;

                            changed = true;
                        }
                    }
                }
                if (ruleGroup.actionCode.HasFlag(RuleActionCodes.Priority))
                {
                    bool oldPriority = message.Priority;
                    message.Priority = !isClear;
                    changed = message.Priority != oldPriority;
                    if (changed && message.Unread)
                    {
                        Folder folder = CIX.FolderCollection[message.TopicID];
                        folder.UnreadPriority += (message.Priority) ? 1 : -1;
                    }
                }
                if (ruleGroup.actionCode.HasFlag(RuleActionCodes.Ignored))
                {
                    message.Ignored = !isClear;
                    if (message.Ignored && message.Unread)
                    {
                        message.Unread = false;
                        message.ReadPending = true;
                        Folder folder = CIX.FolderCollection[message.TopicID];
                        folder.Unread -= 1;
                        if (message.Priority)
                        {
                            folder.UnreadPriority -= 1;
                        }
                        folder.MarkReadRangePending = true;

                        changed = true;
                    }
                }
                if (ruleGroup.actionCode.HasFlag(RuleActionCodes.Flag))
                {
                    bool oldStarred = message.Starred;
                    message.Starred = !isClear;
                    if (oldStarred != message.Starred)
                    {
                        message.StarPending = true;
                        changed = true;
                    }
                }
            }
            return changed;
        }
    }
}