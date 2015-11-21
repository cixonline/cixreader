// *****************************************************
// CIXReader
// Scanner.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 16/05/2015 19:00
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

namespace CIXReader.Utilities
{
    public sealed class Scanner
    {
        private readonly string _stringToScan;
        private int _index;

        public Scanner(string stringToScan)
        {
            _stringToScan = stringToScan;
            _index = 0;
        }

        /// <summary>
        /// Return true if we're at the end of the scan string, false otherwise.
        /// </summary>
        public bool IsAtEnd
        {
            get { return _index == _stringToScan.Length; }
        }

        /// <summary>
        /// Search for and scan up to the specified string and store everything that
        /// precedes it to the output buffer.
        /// </summary>
        public bool ScanUpToString(string stopString, ref string buffer)
        {
            int stopIndex = _stringToScan.IndexOf(stopString, _index, System.StringComparison.Ordinal);
            if (stopIndex < 0)
            {
                buffer = _stringToScan.Substring(_index);
                _index = _stringToScan.Length;
                return true;
            }
            if (stopIndex > 0)
            {
                buffer = _stringToScan.Substring(_index, stopIndex - _index);
            }
            _index = stopIndex;
            return stopIndex > 0;
        }

        /// <summary>
        /// Scans a given string, returning an equivalent string object by reference if a match is found.
        /// If string is present at the current scan location, then the current scan location is advanced 
        /// to after the string; otherwise the scan location does not change.
        /// Invoke this method with NULL as stringValue to simply scan past a given string.
        /// </summary>
        public bool ScanString(string stopString, ref string buffer)
        {
            if (_stringToScan.IndexOf(stopString, _index, System.StringComparison.Ordinal) == _index)
            {
                buffer = _stringToScan.Substring(_index, stopString.Length);
                _index += stopString.Length;
                return true;
            }
            return false;
        }
    }
}