// *****************************************************
// CIXReader
// Address.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 09/06/2015 12:03
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;

namespace CIXReader.Utilities
{
    public sealed class Address
    {
        private string _query;

        /// <summary>
        /// Initialises a new instance of the <see cref="Address"/> class..
        /// </summary>
        /// <param name="address">A CIX-style address URI</param>
        public Address(string address)
        {
            if (address != null)
            {
                int index = address.IndexOf(':');
                if (index == -1)
                {
                    Data = address;
                    Scheme = address;
                    SchemeAndQuery = address;
                }
                else
                {
                    Scheme = address.Substring(0, index).ToLower();

                    // Locate optional data that follows query
                    string queryPart = address.Substring(index + 1);
                    index = queryPart.IndexOf(':');
                    if (index == -1)
                    {
                        int value;
                        if (Int32.TryParse(queryPart, out value))
                        {
                            Data = queryPart;
                        }
                        else
                        {
                            Query = queryPart;
                        }
                    }
                    else
                    {
                        Data = queryPart.Substring(index + 1);
                        Query = queryPart.Substring(0, index);
                    }

                    SchemeAndQuery = string.Format("{0}:{1}", Scheme, Query);
                }
            }
        }

        /// <summary>
        /// Get or set the scheme portion of the address (cix:, file:, etc)
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Get or set the query portion of the address (cix_beta/test)
        /// </summary>
        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                SchemeAndQuery = string.Format("{0}:{1}", Scheme, _query);
            }
        }

        /// <summary>
        /// Return the scheme portion of the address (cix:, file:, etc)
        /// plus the query.
        /// </summary>
        public string SchemeAndQuery { get; private set; }

        /// <summary>
        /// Get or set the data portion (to the right of any ':'') in the address
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Flag indicating whether message at address should be read or unread.
        /// </summary>
        public bool Unread { get; set; }
    }
}