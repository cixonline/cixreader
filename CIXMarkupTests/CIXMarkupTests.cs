// *****************************************************
// CIXReader
// CIXMarkupTests.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 25/06/2015 11:35
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIXMarkupTests
{
    [TestClass]
    public sealed class CIXMarkupTests
    {
        [TestMethod]
        public void TestBasic()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "String < String"), 
                "String &lt; String");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "String > String"), 
                "String &gt; String");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "String & String"), 
                "String &amp; String");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "String\nString"), 
                "String<br />String");
        }

        [TestMethod]
        public void TestSimpleStyling()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "*String*"),
                "<b>String</b>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "/String/"),
                "<i>String</i>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "_String_"),
                "<u>String</u>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "*Unterminated bold"),
                "*Unterminated bold");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "*Terminated bold*,"),
                "<b>Terminated bold</b>,");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "/Unterminated italic"),
                "/Unterminated italic");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "_Unterminated underline"),
                "_Unterminated underline");
        }

        [TestMethod]
        public void TestEmbeddedStyles()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "/etc/pathname"),
                "/etc/pathname");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "/etc/pathname/"),
                "/etc/pathname/");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "*/etc/pathname*"),
                "<b>/etc/pathname</b>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "/etc/pathname*/etc/pathname/*"),
                "/etc/pathname*/etc/pathname/*");
        }

        [TestMethod]
        public void TestMixedStyling()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "_*String*_"),
                "<u><b>String</b></u>");
        }

        [TestMethod]
        public void TestFringeCases()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                ":__String__"),
                ":__String__");
        }

        [TestMethod]
        public void TestPassthrough()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "<font name=\"courier\">Style</font>"),
                "<font name=\"courier\">Style</font>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "<b>Bold!</b>"),
                "<b>Bold!</b>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "<i>Italic!</i>"),
                "<i>Italic!</i>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "<u>Underline!</u>"),
                "<u>Underline!</u>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "<Snork!>"),
                "&lt;Snork!&gt;");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "<Some other tag />"),
                "&lt;Some other tag /&gt;");
        }

        [TestMethod]
        public void TestSingleQuote()
        {
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "> Some Text"),
                "<blockquote>Some Text</blockquote>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "  > Text with  spaces"),
                "<blockquote>Text with  spaces</blockquote>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                ">> Second level"),
                "<blockquote><blockquote>Second level</blockquote></blockquote>");
            Assert.AreEqual(CIXMarkup.CIXMarkup.MarkupToHTML(
                "> Line 1\n >Line 2\nLine 3"),
                "<blockquote>Line 1<br />Line 2</blockquote>Line 3");
        }
    }
}