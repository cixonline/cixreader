// *****************************************************
// CIXReader
// Signatures.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 15/01/2015 13:32
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CIXClient;
using CIXReader.Properties;

namespace CIXReader.Utilities
{
    public sealed class Signatures
    {
        private readonly Dictionary<string, string> _signatures;
        private static Signatures instance;

        /// <summary>
        /// Return singleton instance of Signatures
        /// </summary>
        public static Signatures DefaultSignatures
        {
            get { return instance ?? (instance = new Signatures()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Signatures()
        {
            _signatures = new Dictionary<string, string>();

            ReadOldSignature();
            LoadSignatures();

            if (_signatures.Count == 0)
            {
                AddSignature("cix.support", @"Using CIXReader %VERSION% for " + Program.OSName);
            }
        }

        /// <summary>
        /// Return the string that is used to display None in the UI.
        /// </summary>
        public static string NoSignatureString
        {
            get { return Resources.NoSignature; }
        }

        /// <summary>
        /// Return a list of all signatures
        /// </summary>
        public IEnumerable<string> SignatureTitles
        {
            get { return _signatures.Keys.ToArray(); }
        }

        /// <summary>
        /// Return the signature for the title with any placeholders expanded.
        /// </summary>
        public string ExpandSignatureForTitle(string title)
        {
            string signature = SignatureForTitle(title);
            if (signature != null)
            {
                signature = signature.Replace("%VERSION%", Program.VersionString);
            }
            return signature;
        }

        /// <summary>
        /// Return the signature matching the specified title.
        /// </summary>
        public string SignatureForTitle(string title)
        {
            return _signatures.ContainsKey(title) ? _signatures[title] : null;
        }

        /// <summary>
        /// Add a new signature or replace the existing signature with the
        /// specified title.
        /// </summary>
        public void AddSignature(string title, string text)
        {
            _signatures[title] = text;
            SaveSignatures();
        }

        /// <summary>
        /// Remove the signature with the specified title.
        /// </summary>
        public void RemoveSignature(string title)
        {
            _signatures.Remove(title);
            if (title == Preferences.StandardPreferences.DefaultSignature)
            {
                Preferences.StandardPreferences.DefaultSignature = NoSignatureString;
            }
            SaveSignatures();
        }

        /// <summary>
        /// Load all existing signatures
        /// </summary>
        private void LoadSignatures()
        {
            string filename = Path.ChangeExtension(Path.Combine(CIX.HomeFolder, CIX.Username), "signatures.xml");

            StreamReader fileStream = null;
            try
            {
                fileStream = new StreamReader(filename);
                using (XmlReader reader = XmlReader.Create(fileStream))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof (SignatureFile));
                    SignatureFile sigFile = (SignatureFile) serializer.Deserialize(reader);

                    foreach (Signature item in sigFile.Items)
                    {
                        string rawText = item.text.Replace("\n", "\r\n");
                        _signatures[item.name] = rawText.Trim();
                    }
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error parsing signature file {0} : {1}", filename, e.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Save all signatures
        /// </summary>
        private void SaveSignatures()
        {
            string filename = Path.ChangeExtension(Path.Combine(CIX.HomeFolder, CIX.Username), "signatures.xml");
            StreamWriter fileStream = null;

            try
            {
                fileStream = new StreamWriter(filename, false);
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true,
                    CloseOutput = true,
                    NewLineOnAttributes = true
                };

                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof(SignatureFile));

                    SignatureFile file = new SignatureFile
                    {
                        Items = _signatures.Keys.Select(title => new Signature {name = title, text = _signatures[title]}).ToArray()
                    };
                    serializer.Serialize(writer, file);
                }

                LogFile.WriteLine("Saved signatures to {0}", filename);
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot save signatures to {0} : {1}", filename, e.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }            
        }

        /// <summary>
        /// If a pre v1.50 signature is found, read it and make it the default.
        /// </summary>
        private void ReadOldSignature()
        {
            string signatureFile = Path.ChangeExtension(Path.Combine(CIX.HomeFolder, CIX.Username), "signature");
            if (File.Exists(signatureFile))
            {
                AddSignature("Default", File.ReadAllText(signatureFile));
                Preferences.StandardPreferences.DefaultSignature = "Default";

                // Then delete it so we don't keep it around
                File.Delete(signatureFile);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute(@"code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public sealed class SignatureFile
    {
        private Signature[] itemsField;

        /// <remarks/>
        [XmlElementAttribute("Signature", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Signature[] Items
        {
            get
            {
                return itemsField;
            }
            set
            {
                itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
    [SerializableAttribute]
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.ComponentModel.DesignerCategoryAttribute(@"code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class Signature
    {
        private string textField;

        private string nameField;

        /// <remarks/>
        [XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }
    }
}