// *****************************************************
// CIXReader
// UIConfig.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 23/09/2013 11:15 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CIXClient;
using CIXReader.Properties;
using CIXReader.Utilities;

namespace CIXReader.UIConfig
{
    /// <summary>
    /// The static UI class defines attributes used to configure the UI layout. A single
    /// UI class represents a theme.
    /// </summary>
    public static class UI
    {
        private static UIConfig _uiConfig;
        private static UIThemes _uiThemes;

        /// <summary>
        /// Event handler for notifying a delegate that a theme has been updated.
        /// </summary>
        public static event EventHandler ThemeChanged;

        /// <summary>
        /// Returns the Menu UI configuration.
        /// </summary>
        public static UIConfigMenu Menu
        {
            get { return Config.menu[0]; }
        }

        /// <summary>
        /// Returns the System UI configuration.
        /// </summary>
        public static UIConfigSystem System
        {
            get { return Config.system[0]; }
        }

        /// <summary>
        /// Returns the Forums UI configuration.
        /// </summary>
        public static UIConfigForums Forums
        {
            get { return Config.forums[0]; }
        }

        /// <summary>
        /// Returns the Keys configuration.
        /// </summary>
        public static UIConfigKeys Keys
        {
            get { return Config.keys[0]; }
        }

        /// <summary>
        /// Raise a theme change event.
        /// </summary>
        public static void InvokeThemeChanged()
        {
            if (ThemeChanged != null)
            {
                ThemeChanged(null, new EventArgs());
            }
        }

        /// <summary>
        /// Return the full path and filename of the custom theme file.
        /// </summary>
        /// <returns></returns>
        public static string CustomThemeFile
        {
            get
            {
                string themesFolder = Path.Combine(CIX.HomeFolder, "Themes");
                return Path.Combine(themesFolder, Resources.Custom, "ui.xml");
            }
        }

        /// <summary>
        /// Revert the theme. If custom is specified and a custom theme is
        /// found, revert to that. Otherwise revert to the default system theme.
        /// </summary>
        public static void RevertConfig(bool custom)
        {
            if (!custom)
            {
                if (File.Exists(CustomThemeFile))
                {
                    File.Delete(CustomThemeFile);
                }
                Themes.Remove(Resources.Custom);
            }
            CurrentTheme = (custom) ? Resources.Custom : Resources.Default;
            _uiConfig = ParseConfig();
            InvokeThemeChanged();
        }

        /// <summary>
        /// Save the current theme settings to a custom configuration file.
        /// </summary>
        public static bool SaveConfig()
        {
            StreamWriter fileStream = null;
            string uiFilePath = CustomThemeFile;

            bool success = false;

            try
            {
                CIX.UIConfigFolder = Path.GetDirectoryName(uiFilePath);

                if (CIX.UIConfigFolder != null)
                {
                    Directory.CreateDirectory(CIX.UIConfigFolder);
                }

                fileStream = new StreamWriter(uiFilePath, false);
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true,
                    NewLineOnAttributes = true
                };

                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof (UIConfig));
                    serializer.Serialize(writer, _uiConfig);
                }

                Themes.Add(Resources.Custom);

                Preferences.StandardPreferences.CurrentTheme = Resources.Custom;

                LogFile.WriteLine("Saved UI themes to {0}", uiFilePath);
                success = true;
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Cannot save UI themes to {0} : {1}", uiFilePath, e.Message);
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
        /// Get or set the current active theme name.
        /// </summary>
        public static string CurrentTheme
        {
            get
            {
                return Preferences.StandardPreferences.CurrentTheme;
            }
            set
            {
                string currentTheme = Preferences.StandardPreferences.CurrentTheme;
                if (currentTheme != value)
                {
                    Preferences.StandardPreferences.CurrentTheme = value;
                    _uiConfig = ParseConfig();
                    InvokeThemeChanged();
                }
            }
        }

        /// <summary>
        /// Return the collection of themes.
        /// </summary>
        public static UIThemes Themes
        {
            get { return _uiThemes ?? (_uiThemes = new UIThemes()); }
        }

        private static UIConfig Config
        {
            get { return _uiConfig ?? (_uiConfig = ParseConfig()); }
        }

        /// <summary>
        /// Parse the current theme file.
        /// </summary>
        private static UIConfig ParseConfig()
        {
            UIConfig uiConfig = null;
            try
            {
                UITheme theme = Themes[CurrentTheme];
                if (theme == null || !File.Exists(theme.Path))
                {
                    theme = Themes[Resources.Default];
                    Preferences.StandardPreferences.CurrentTheme = Resources.Default;
                }
                if (theme != null)
                {
                    UIConfig uiDefaultConfig = null;

                    if (!theme.IsDefault)
                    {
                        UITheme defaultTheme = Themes[Resources.Default];
                        uiDefaultConfig = ParseConfigFile(defaultTheme.Path);
                    }

                    string uiFilePath = theme.Path;
                    uiConfig = ParseConfigFile(uiFilePath);

                    if (uiDefaultConfig != null)
                    {
                        uiDefaultConfig.Merge(uiConfig);
                        uiConfig = uiDefaultConfig;
                    }

                    CIX.UIConfigFolder = Path.GetDirectoryName(uiFilePath);

                    LogFile.WriteLine("Loaded UI themes from {0}", uiFilePath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Cannot open theme file. " + e.Message);
            }
            return uiConfig;
        }

        /// <summary>
        /// Parse the theme file from the specified filename.
        /// </summary>
        private static UIConfig ParseConfigFile(string filename)
        {
            StreamReader fileStream = null;
            UIConfig uiConfig;

            try
            {
                fileStream = new StreamReader(filename);
                using (XmlReader reader = XmlReader.Create(fileStream))
                {
                    fileStream = null;

                    XmlSerializer serializer = new XmlSerializer(typeof (UIConfig));
                    uiConfig = (UIConfig) serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                LogFile.WriteLine("Error parsing config file {0} : {1}", filename, e.Message);
                uiConfig = null;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
            return uiConfig;
        }

        /// <summary>
        /// Attempt to parse a colour value from one of three possible representations in the
        /// configuration file:
        /// 
        /// #XXYYZZ - HTML hex values where XX is red, YY is green and ZZ is blue.
        /// XX,YY,ZZ - integer values where XX is red, YY is green and ZZ is blue.
        /// name - a colour name
        /// 
        /// </summary>
        /// <returns>A Color item that represents the given colour</returns>
        public static Color FromString(string colourString)
        {
            if (colourString.StartsWith("#", StringComparison.Ordinal))
            {
                return ColorTranslator.FromHtml(colourString);
            }

            string[] rgbValues = colourString.Split(',');
            int red, blue, green;
            if (rgbValues.Length == 3 && 
                Int32.TryParse(rgbValues[0], out red) &&
                Int32.TryParse(rgbValues[1], out green) &&
                Int32.TryParse(rgbValues[2], out blue))
            {
                return Color.FromArgb(red, green, blue);
            }

            switch (colourString)
            {
                case "highlight":
                    return SystemColors.Highlight;

                case "highlighttext":
                    return SystemColors.HighlightText;

                case "windowtext":
                    return SystemColors.WindowText;

                case "control":
                    return SystemColors.Control;

                case "controltext":
                    return SystemColors.ControlText;

                case "activeborder":
                    return SystemColors.ActiveBorder;

                case "graytext":
                    return SystemColors.GrayText;
            }

            return Color.FromName(colourString);
        }

        /// <summary>
        /// Return a friendly name for the specified colour.
        /// </summary>
        public static string ToString(Color colour)
        {
            return string.Format("{0},{1},{2}", colour.R, colour.G, colour.B);
        }

        /// <summary>
        /// Get a font of the specified size given a font name or a series of font names separated
        /// by a comma.
        /// </summary>
        public static Font GetFont(string name, float size, FontStyle style = FontStyle.Regular)
        {
            string[] fontParts = name.Split(',');
            foreach (string fontName in fontParts)
            {
                string rwFontName = fontName;
                if (rwFontName == "Roboto Condensed")
                {
                    rwFontName = "Arial";
                }
                try
                {
                    using (FontFamily family = new FontFamily(rwFontName))
                    {
                        if (style == FontStyle.Regular && family.IsStyleAvailable(style))
                        {
                            return new Font(rwFontName, size, style);
                        }
                        if (style == FontStyle.Bold && family.IsStyleAvailable(style))
                        {
                            return new Font(rwFontName, size, style);
                        }
                        if (style == FontStyle.Italic && family.IsStyleAvailable(style))
                        {
                            return new Font(rwFontName, size, style);
                        }
                        return family.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic)
                            ? new Font(rwFontName, size, FontStyle.Bold | FontStyle.Italic)
                            : null;
                    }
                }
                catch (Exception e)
                {
                    LogFile.WriteLine("Skipped font {0} from theme: {1}", fontName, e.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Get a regular font of the specified size, using a custom font if one
        /// is requested.
        /// </summary>
        public static Font GetFont(Font font, float size)
        {
            return GetFont(font.Name, size, font.Style);
        }

        /// <summary>
        /// Return a string that represents the key used for the specified action.
        /// </summary>
        /// <param name="actionId">The action ID</param>
        /// <returns>The key string</returns>
        public static string MapActionToKeyString(ActionID actionId)
        {
            string actionEnum = actionId.ToString();

            UIConfigKeysKey ky = null;
            if (Keys.customkey != null)
            {
                ky = Keys.customkey.FirstOrDefault(key => String.Equals(key.name, actionEnum, StringComparison.CurrentCultureIgnoreCase));
            }
            if (ky == null)
            {
                ky = Keys.key.FirstOrDefault(key => String.Equals(key.name, actionEnum, StringComparison.CurrentCultureIgnoreCase));
            }
            if (ky != null)
            {
                switch (ky.code)
                {
                    case "Oemcomma":    return ",";
                    case "OemPeriod":   return ".";
                    case "Left":        return "←";
                    case "Right":       return "→";
                }
                return ky.code;
            }
            return string.Empty;
        }

        /// <summary>
        /// Map a key to an action code.
        /// </summary>
        /// <param name="keyData">Key data to map</param>
        /// <returns>An ActionID or ActionID.None</returns>
        public static ActionID MapKeyToAction(Keys keyData)
        {
            KeysConverter kc = new KeysConverter();
            string convertedKey = kc.ConvertToString(keyData);

            ActionID result = ActionID.None;
            if (convertedKey != null)
            {
                // Try any custom key fields before the stock fields
                UIConfigKeysKey ky = null;
                if (Keys.customkey != null)
                {
                    ky = Keys.customkey.FirstOrDefault(key => String.Equals(key.code, convertedKey, StringComparison.CurrentCultureIgnoreCase));
                }
                if (ky == null)
                {
                    ky = Keys.key.FirstOrDefault(key => String.Equals(key.code, convertedKey, StringComparison.CurrentCultureIgnoreCase));
                }
                if (ky != null)
                {
                    Enum.TryParse(ky.name, true, out result);
                }
            }
            return result;
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public sealed class UIConfig
    {
        private UIConfigMenu[] menuField;
        private UIConfigSystem[] systemField;
        private UIConfigForums[] forumsField;
        private UIConfigKeys[] keysField;

        /// <summary>
        /// Merge the specified UIConfig into this one.
        /// </summary>
        /// <param name="config">A UIConfig to merge</param>
        public void Merge(UIConfig config)
        {
            if (config.menuField != null)
            {
                menuField[0].Merge(config.menuField[0]);
            }
            if (config.systemField != null)
            {
                systemField[0].Merge(config.systemField[0]);
            }
            if (config.forumsField != null)
            {
                forumsField[0].Merge(config.forumsField[0]);
            }
            if (config.keysField != null)
            {
                keysField[0].Merge(config.keysField[0]);
            }
        }

        /// <remarks />
        [XmlElement("menu", Form = XmlSchemaForm.Unqualified)]
        public UIConfigMenu[] menu
        {
            get { return menuField; }
            set { menuField = value; }
        }

        /// <remarks />
        [XmlElement("system", Form = XmlSchemaForm.Unqualified)]
        public UIConfigSystem[] system
        {
            get { return systemField; }
            set { systemField = value; }
        }

        /// <remarks />
        [XmlElement("forums", Form = XmlSchemaForm.Unqualified)]
        public UIConfigForums[] forums
        {
            get { return forumsField; }
            set { forumsField = value; }
        }

        /// <remarks />
        [XmlElement("keys", Form = XmlSchemaForm.Unqualified)]
        public UIConfigKeys[] keys
        {
            get { return keysField; }
            set { keysField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class UIConfigMenu
    {
        private string backcolourField;

        private string fontField;

        private string forecolourField;

        private string inactivecolourField;

        /// <summary>
        /// Merge the specified UIConfigMenu into this one.
        /// </summary>
        /// <param name="config">A UIConfigMenu to merge</param>
        public void Merge(UIConfigMenu config)
        {
            backcolourField = config.backcolourField ?? backcolourField;
            fontField = config.fontField ?? fontField;
            forecolourField = config.forecolourField ?? forecolourField;
            inactivecolourField = config.inactivecolourField ?? inactivecolourField;
        }

        /// <summary>
        /// Returns the menu background colour as a Color object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color BackgroundColour
        {
            set
            {
                backcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(backcolourField); }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string backcolor
        {
            get { return backcolourField; }
            set { backcolourField = value; }
        }

        /// <summary>
        /// Return the menu font as a Font object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Font Font
        {
            set
            {
                fontField = value.Name;
                UI.InvokeThemeChanged();
            }
            get { return UI.GetFont(fontField, FontSize, FontStyle.Bold); }
        }

        /// <summary>
        /// Get the 
        /// </summary>
        [XmlIgnoreAttribute]
        public static float FontSize
        {
            get { return 21.75f; }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string font
        {
            get { return fontField; }
            set { fontField = value; }
        }

        /// <summary>
        /// Returns the menu text foreground colour as a Color object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color ForegroundColour
        {
            set
            {
                forecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(forecolourField); }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string forecolor
        {
            get { return forecolourField; }
            set { forecolourField = value; }
        }

        /// <summary>
        /// Returns the menu text inactive colour as a Color object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color InactiveColour
        {
            set
            {
                inactivecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(inactivecolourField); }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string inactivecolor
        {
            get { return inactivecolourField; }
            set { inactivecolourField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class UIConfigSystem
    {
        private string fontField;
        
        private string selectioncolourField;

        private string selectiontextcolourField;

        private string forecolourField;

        private string linkcolourField;

        private string countcolourField;

        private string countcolourtextField;

        private string infobarcolourField;

        private string infobartextcolourField;

        private string edgecolourField;

        private string splitterbarcolourField;

        private string toolbarcolourField;

        /// <summary>
        ///  Merge the specified UIConfigSystem into this one.
        /// </summary>
        /// <param name="config">A UIConfigSystem to merge</param>
        public void Merge(UIConfigSystem config)
        {
            fontField = config.fontField ?? fontField;
            selectioncolourField = config.selectioncolourField ?? selectioncolourField;
            selectiontextcolourField = config.selectiontextcolourField ?? selectiontextcolourField;
            forecolourField = config.forecolourField ?? forecolourField;
            linkcolourField = config.linkcolourField ?? linkcolourField;
            countcolourField = config.countcolourField ?? countcolourField;
            countcolourtextField = config.countcolourtextField ?? countcolourtextField;
            infobarcolourField = config.infobarcolourField ?? infobarcolourField;
            infobartextcolourField = config.infobartextcolourField ?? infobartextcolourField;
            edgecolourField = config.edgecolourField ?? edgecolourField;
            splitterbarcolourField = config.splitterbarcolourField ?? splitterbarcolourField;
            toolbarcolourField = config.toolbarcolourField ?? toolbarcolourField;
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string font
        {
            get { return fontField; }
            set { fontField = value; }
        }

        /// <summary>
        /// Return the system font as a Font object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Font Font
        {
            set
            {
                fontField = value.Name;
                UI.InvokeThemeChanged();
            }
            get { return UI.GetFont(fontField, 10); }
        }

        /// <remarks />
        [XmlElement("selectioncolor", Form = XmlSchemaForm.Unqualified)]
        public string selectionColor
        {
            get { return selectioncolourField; }
            set { selectioncolourField = value; }
        }

        /// <summary>
        /// Returns the background colour for selected items.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color SelectionColour
        {
            set
            {
                selectioncolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(selectioncolourField); }
        }

        /// <remarks />
        [XmlElement("linkcolor", Form = XmlSchemaForm.Unqualified)]
        public string linkColor
        {
            get { return linkcolourField; }
            set { linkcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for links.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color LinkColour
        {
            set
            {
                linkcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(linkcolourField); }
        }

        /// <summary>
        /// Returns the colour for the count buttons.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color CountColour
        {
            set
            {
                countcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(countcolourField); }
        }

        /// <remarks />
        [XmlElement("countcolor", Form = XmlSchemaForm.Unqualified)]
        public string countColor
        {
            get { return countcolourField; }
            set { countcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for the text in the count buttons.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color CountTextColour
        {
            set
            {
                countcolourtextField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(countcolourtextField); }
        }

        /// <remarks />
        [XmlElement("counttextcolor", Form = XmlSchemaForm.Unqualified)]
        public string counttextColor
        {
            get { return countcolourtextField; }
            set { countcolourtextField = value; }
        }

        /// <remarks />
        [XmlElement("selectiontextcolor", Form = XmlSchemaForm.Unqualified)]
        public string selectiontextColor
        {
            get { return selectiontextcolourField; }
            set { selectiontextcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for text in selected items.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color SelectionTextColour
        {
            set
            {
                selectiontextcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(selectiontextcolourField); }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string forecolor
        {
            get { return forecolourField; }
            set { forecolourField = value; }
        }

        /// <summary>
        /// Returns the menu text foreground colour as a Color object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color ForegroundColour
        {
            set
            {
                forecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(forecolourField); }
        }

        /// <summary>
        /// Returns the colour for the topic info bar
        /// </summary>
        [XmlIgnoreAttribute]
        public Color InfoBarColour
        {
            set
            {
                infobarcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(infobarcolourField); }
        }

        /// <remarks />
        public string infobarcolor
        {
            get { return infobarcolourField; }
            set { infobarcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for the topic info bar text
        /// </summary>
        [XmlIgnoreAttribute]
        public Color InfoBarTextColour
        {
            set
            {
                infobartextcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(infobartextcolourField); }
        }

        /// <remarks />
        public string infobartextcolor
        {
            get { return infobartextcolourField; }
            set { infobartextcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for edging
        /// </summary>
        [XmlIgnoreAttribute]
        public Color EdgeColour
        {
            set
            {
                edgecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(edgecolourField); }
        }

        /// <remarks />
        public string edgecolor
        {
            get { return edgecolourField; }
            set { edgecolourField = value; }
        }

        /// <summary>
        /// Returns the colour for the splitter bar
        /// </summary>
        [XmlIgnoreAttribute]
        public Color SplitterBarColour
        {
            set
            {
                splitterbarcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(splitterbarcolourField); }
        }

        /// <remarks />
        public string splitterbarcolor
        {
            get { return splitterbarcolourField; }
            set { splitterbarcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for the toolbar background.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color ToolbarColour
        {
            set
            {
                toolbarcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(toolbarcolourField); }
        }

        /// <remarks />
        public string toolbarcolor
        {
            get { return toolbarcolourField; }
            set { toolbarcolourField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class UIConfigForums
    {
        private string fontField;

        private float fontsizeField;

        private string rootfontField;

        private float rootfontsizeField;

        private string listfontField;

        private float listfontsizeField;

        private string messagefontField;

        private float messagefontsizeField;

        private string commentcolourField;

        private string rootcolourField;

        private string bodycolourField;

        private string headerfootercolourField;

        private string collapsedcolourField;

        private string prioritycolourField;

        private string ignoredcolourField;

        private string draftcolourField;

        private string selectioncolourField;

        private string selectiontextcolourField;

        private string level1quotecolourField;

        private string level2quotecolourField;

        private string level3quotecolourField;

        /// <summary>
        ///  Merge the specified UIConfigForums into this one.
        /// </summary>
        /// <param name="config">A UIConfigForums to merge</param>
        public void Merge(UIConfigForums config)
        {
            fontField = config.fontField ?? fontField;
            fontsizeField = (config.fontsizeField > 0) ? config.fontsizeField : fontsizeField;
            rootfontField = config.rootfontField ?? rootfontField;
            rootfontsizeField = (config.rootfontsizeField > 0) ? config.rootfontsizeField : rootfontsizeField;
            messagefontField = config.messagefontField ?? messagefontField;
            messagefontsizeField = (config.messagefontsizeField > 0) ? config.messagefontsizeField : messagefontsizeField;
            listfontField = config.listfontField ?? listfontField;
            listfontsizeField = (config.listfontsizeField > 0) ? config.listfontsizeField : listfontsizeField;
            commentcolourField = config.commentcolourField ?? commentcolourField;
            rootcolourField = config.rootcolourField ?? rootcolourField;
            headerfootercolourField = config.headerfootercolourField ?? headerfootercolourField;
            bodycolourField = config.bodycolourField ?? bodycolourField;
            collapsedcolourField = config.collapsedcolourField ?? collapsedcolourField;
            prioritycolourField = config.prioritycolourField ?? prioritycolourField;
            ignoredcolourField = config.ignoredcolourField ?? ignoredcolourField;
            draftcolourField = config.draftcolourField ?? draftcolourField;
            selectioncolourField = config.selectioncolourField ?? selectioncolourField;
            selectiontextcolourField = config.selectiontextcolourField ?? selectiontextcolourField;
            level1quotecolourField = config.level1quotecolourField ?? level1quotecolourField;
            level2quotecolourField = config.level2quotecolourField ?? level2quotecolourField;
            level3quotecolourField = config.level3quotecolourField ?? level3quotecolourField;
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string font
        {
            get { return fontField; }
            set { fontField = value; }
        }

        /// <summary>
        /// Return the forums font as a Font object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Font Font
        {
            set
            {
                fontField = value.Name;
                UI.InvokeThemeChanged();
            }
            get { return UI.GetFont(fontField, fontsize); }
        }

        /// <remarks />
        [XmlElement("font-size", Form = XmlSchemaForm.Unqualified)]
        public float fontsize
        {
            get { return fontsizeField; }
            set { fontsizeField = value; }
        }

        /// <summary>
        /// Return the height of the messages font in em values.
        /// </summary>
        [XmlIgnoreAttribute]
        public float FontSize
        {
            set
            {
                fontsizeField = value;
                UI.InvokeThemeChanged();
            }
            get { return fontsizeField; }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string listfont
        {
            get { return listfontField; }
            set { listfontField = value; }
        }

        /// <summary>
        /// Return the forums font as a Font object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Font ListFont
        {
            set
            {
                listfontField = value.Name;
                UI.InvokeThemeChanged();
            }
            get { return UI.GetFont(listfontField, listfontsize); }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string rootfont
        {
            get { return rootfontField; }
            set { rootfontField = value; }
        }

        /// <summary>
        /// Return the root message font as a Font object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Font RootFont
        {
            set
            {
                rootfontField = value.Name;
                UI.InvokeThemeChanged();
            }
            get { return UI.GetFont(rootfontField, rootfontsizeField); }
        }

        /// <remarks />
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string messagefont
        {
            get { return messagefontField; }
            set { messagefontField = value; }
        }

        /// <summary>
        /// Return the message pane font as a Font object.
        /// </summary>
        [XmlIgnoreAttribute]
        public Font MessageFont
        {
            set
            {
                messagefontField = value.Name;
                UI.InvokeThemeChanged();
            }
            get { return UI.GetFont(messagefontField, messagefontsizeField); }
        }

        /// <remarks />
        [XmlElement("listfont-size", Form = XmlSchemaForm.Unqualified)]
        public float listfontsize
        {
            get { return listfontsizeField; }
            set { listfontsizeField = value; }
        }

        /// <summary>
        /// Return the height of the messages font in em values.
        /// </summary>
        [XmlIgnoreAttribute]
        public float ListFontSize
        {
            set
            {
                listfontsizeField = value;
                UI.InvokeThemeChanged();
            }
            get { return listfontsizeField; }
        }

        /// <remarks />
        [XmlElement("rootfont-size", Form = XmlSchemaForm.Unqualified)]
        public float rootfontsize
        {
            get { return rootfontsizeField; }
            set { rootfontsizeField = value; }
        }

        /// <summary>
        /// Return the height of the root font in em values.
        /// </summary>
        [XmlIgnoreAttribute]
        public float RootFontSize
        {
            set
            {
                rootfontsizeField = value;
                UI.InvokeThemeChanged();
            }
            get { return rootfontsizeField; }
        }

        /// <remarks />
        [XmlElement("messagefont-size", Form = XmlSchemaForm.Unqualified)]
        public float messagefontsize
        {
            get { return messagefontsizeField; }
            set { messagefontsizeField = value; }
        }

        /// <summary>
        /// Return the height of the message pane font in em values.
        /// </summary>
        [XmlIgnoreAttribute]
        public float MessageFontSize
        {
            set
            {
                messagefontsizeField = value;
                UI.InvokeThemeChanged();
            }
            get { return messagefontsizeField; }
        }

        /// <summary>
        /// Returns the background colour for thread items that are root messages.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color RootColour
        {
            set
            {
                rootcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(rootcolourField); }
        }

        /// <remarks />
        [XmlElement("rootcolor", Form = XmlSchemaForm.Unqualified)]
        public string rootcolor
        {
            get { return rootcolourField; }
            set { rootcolourField = value; }
        }

        /// <summary>
        /// Returns the background colour for thread items that are comments.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color CommentColour
        {
            set
            {
                commentcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(commentcolourField); }
        }

        /// <remarks />
        [XmlElement("commentcolor", Form = XmlSchemaForm.Unqualified)]
        public string commentcolor
        {
            get { return commentcolourField; }
            set { commentcolourField = value; }
        }

        /// <summary>
        /// Returns the background colour for collapsed root messages.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color CollapsedColour
        {
            set
            {
                collapsedcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(collapsedcolourField); }
        }

        /// <remarks />
        [XmlElement("collapsedcolor", Form = XmlSchemaForm.Unqualified)]
        public string collapsedcolor
        {
            get { return collapsedcolourField; }
            set { collapsedcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for the message text
        /// </summary>
        [XmlIgnoreAttribute]
        public Color BodyColour
        {
            set
            {
                bodycolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(bodycolourField); }
        }

        /// <remarks />
        [XmlElement("bodycolor", Form = XmlSchemaForm.Unqualified)]
        public string bodycolor
        {
            get { return bodycolourField; }
            set { bodycolourField = value; }
        }

        /// <summary>
        /// Returns the colour for the text that appears in a thread item header
        /// and footer.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color HeaderFooterColour
        {
            set
            {
                headerfootercolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(headerfootercolourField); }
        }

        /// <remarks />
        [XmlElement("headerfootercolor", Form = XmlSchemaForm.Unqualified)]
        public string headerfootercolor
        {
            get { return headerfootercolourField; }
            set { headerfootercolourField = value; }
        }

        /// <summary>
        /// Returns the colour for priority messages.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color PriorityColour
        {
            set
            {
                prioritycolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(prioritycolourField); }
        }

        /// <remarks />
        public string prioritycolor
        {
            get { return prioritycolourField; }
            set { prioritycolourField = value; }
        }

        /// <summary>
        /// Returns the colour for ignored messages.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color IgnoredColour
        {
            set
            {
                ignoredcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(ignoredcolourField); }
        }

        /// <remarks />
        public string ignoredcolor
        {
            get { return ignoredcolourField; }
            set { ignoredcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for draft messages.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color DraftColour
        {
            set
            {
                draftcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(draftcolourField); }
        }

        /// <remarks />
        public string draftcolor
        {
            get { return draftcolourField; }
            set { draftcolourField = value; }
        }

        /// <remarks />
        public string level1quotecolor
        {
            get { return level1quotecolourField; }
            set { level1quotecolourField = value; }
        }

        /// <summary>
        /// Returns the text colour for the first quote indent.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color Level1QuoteColour
        {
            set
            {
                level1quotecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(level1quotecolourField); }
        }

        /// <remarks />
        public string level2quotecolor
        {
            get { return level2quotecolourField; }
            set { level2quotecolourField = value; }
        }

        /// <summary>
        /// Returns the text colour for the second quote indent.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color Level2QuoteColour
        {
            set
            {
                level2quotecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(level2quotecolourField); }
        }

        /// <remarks />
        public string level3quotecolor
        {
            get { return level3quotecolourField; }
            set { level3quotecolourField = value; }
        }

        /// <summary>
        /// Returns the text colour for the third quote indent.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color Level3QuoteColour
        {
            set
            {
                level3quotecolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(level3quotecolourField); }
        }

        /// <remarks />
        [XmlElement("selectioncolor", Form = XmlSchemaForm.Unqualified)]
        public string selectionColor
        {
            get { return selectioncolourField; }
            set { selectioncolourField = value; }
        }

        /// <summary>
        /// Returns the background colour for selected items.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color SelectionColour
        {
            set
            {
                selectioncolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(selectioncolourField); }
        }

        /// <remarks />
        [XmlElement("selectiontextcolor", Form = XmlSchemaForm.Unqualified)]
        public string selectiontextColor
        {
            get { return selectiontextcolourField; }
            set { selectiontextcolourField = value; }
        }

        /// <summary>
        /// Returns the colour for text in selected items.
        /// </summary>
        [XmlIgnoreAttribute]
        public Color SelectionTextColour
        {
            set
            {
                selectiontextcolourField = UI.ToString(value);
                UI.InvokeThemeChanged();
            }
            get { return UI.FromString(selectiontextcolourField); }
        }
    }

    /// <remarks/>
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class UIConfigKeys
    {
        private UIConfigKeysKey[] keyField;

        private UIConfigKeysKey[] customkeyField;

        /// <summary>
        ///  Merge the specified UIConfigKeys into this one.
        /// </summary>
        /// <param name="config">A UIConfigKeys to merge</param>
        public void Merge(UIConfigKeys config)
        {
            customkeyField = config.keyField;
        }

        /// <remarks/>
        [XmlElement("key", Form = XmlSchemaForm.Unqualified)]
        public UIConfigKeysKey[] key
        {
            get { return keyField; }
            set { keyField = value; }
        }

        /// <remarks/>
        [XmlIgnoreAttribute]
        public UIConfigKeysKey[] customkey
        {
            get { return customkeyField; }
            set { customkeyField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("xsd", "4.0.30319.17929")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public sealed class UIConfigKeysKey
    {
        private string codeField;

        private string nameField;

        /// <remarks/>
        [XmlAttribute]
        public string code
        {
            get { return codeField; }
            set { codeField = value; }
        }

        /// <remarks/>
        [XmlAttribute]
        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }
    }
}