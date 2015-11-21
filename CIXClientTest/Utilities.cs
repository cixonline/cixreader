// *****************************************************
// CIXReader
// Utilities.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 22/04/2015 12:21
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.IO;

namespace CIXClientTest
{
    public static class Utilities
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}