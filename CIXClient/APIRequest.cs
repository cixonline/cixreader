// *****************************************************
// CIXReader
// APIRequest.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 01/09/2013 10:19 AM
// 
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace CIXClient
{
    /// <summary>
    /// The APIRequest class encapsulates the functionality required to make a call
    /// to the CIX API server.
    /// </summary>
    public static class APIRequest
    {
        private const string BetaAPIBaseURL = "https://betaapi.cixonline.com/v2.0/cix.svc/";
        private const string APIBaseURL = "https://api.cixonline.com/v2.0/cix.svc/";

        private static string _apiBase;
        private static bool _useBetaAPI;

        /// <summary>
        /// Specifies the format of the API data.
        /// </summary>
        internal enum APIFormat
        {
            /// <summary>
            /// Specifies that the request and response data are JSON format.
            /// </summary>
            JSON,

            /// <summary>
            /// Specifies that the request and response data is XML format.
            /// </summary>
            XML
        }

        /// <summary>
        /// Specifies the API method being used.
        /// </summary>
        private enum APIMethod
        {
            /// <summary>
            /// A server GET request
            /// </summary>
            GET,

            /// <summary>
            /// A server POST request
            /// </summary>
            POST
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Beta API is being used.
        /// </summary>
        public static bool UseBetaAPI
        {
            get
            {
                return _useBetaAPI;
            }
            set
            {
                _useBetaAPI = value;
                _apiBase = null;
            }
        }

        /// <summary>
        /// Gets the base API address.
        /// </summary>
        private static string APIBase
        {
            get
            {
                if (string.IsNullOrEmpty(_apiBase))
                {
                    _apiBase = UseBetaAPI ? BetaAPIBaseURL : APIBaseURL;
                    if (string.IsNullOrEmpty(_apiBase))
                    {
                        _apiBase = "https://api.cixonline.com/v2.0/cix.svc/";
                    }
                    LogFile.WriteLine("APIBase={0}", _apiBase);
                }
                return _apiBase;
            }
        }

        /// <summary>
        /// Construct an HttpWebRequest object using the GET method to call the specified
        /// CIXAPI function. The function name should be specified as documented without
        /// the format prefix. Thus "user.getprofile" rather than "user.getprofile.xml".
        /// </summary>
        /// <param name="apiFunction">The API function name</param>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <returns>A constructed HttpWebRequest</returns>
        internal static HttpWebRequest Get(string apiFunction, APIFormat format)
        {
            return Create(apiFunction, CIX.Username, CIX.Password, format, APIMethod.GET, null, null);
        }

        /// <summary>
        /// Construct an HttpWebRequest object using the GET method to call the specified
        /// CIXAPI function. The function name should be specified as documented without
        /// the format prefix. Thus "user.getprofile" rather than "user.getprofile.xml".
        /// </summary>
        /// <param name="apiFunction">The API function name</param>
        /// <param name="username">CIX username</param>
        /// <param name="password">CIX password</param>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <returns>A constructed HttpWebRequest</returns>
        internal static HttpWebRequest GetWithCredentials(string apiFunction, string username, string password, APIFormat format)
        {
            return Create(apiFunction, username, password, format, APIMethod.GET, null, null);
        }

        /// <summary>
        /// Construct an HttpWebRequest object using the GET method to call the specified
        /// CIXAPI function. The function name should be specified as documented without
        /// the format prefix. Thus "user.getprofile" rather than "user.getprofile.xml".
        /// </summary>
        /// <param name="apiFunction">The API function name</param>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <param name="queryString">Query string for the URL</param>
        /// <returns>A constructed HttpWebRequest</returns>
        internal static HttpWebRequest GetWithQuery(string apiFunction, APIFormat format, string queryString)
        {
            return Create(apiFunction, CIX.Username, CIX.Password, format, APIMethod.GET, null, queryString);
        }

        /// <summary>
        /// Construct an HttpWebRequest object using the POST method to call the specified
        /// CIXAPI function. The function name should be specified as documented without
        /// the format prefix. Thus "forum.postmessage" rather than "forum.postmessage.xml".
        /// </summary>
        /// <param name="apiFunction">The API function name</param>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <param name="postObject">The object to be posted</param>
        /// <returns>A constructed HttpWebRequest</returns>
        internal static HttpWebRequest Post(string apiFunction, APIFormat format, object postObject)
        {
            return Create(apiFunction, CIX.Username, CIX.Password, format, APIMethod.POST, postObject, null);
        }

        /// <summary>
        /// Read an XML response string from the server. Note that this function will throw
        /// an exception if an HTTP error occurs.
        /// </summary>
        /// <param name="request">The web request handle</param>
        /// <returns>A response string, which may be empty</returns>
        internal static string ReadResponseString(HttpWebRequest request)
        {
            string responseString = string.Empty;

            Stream objStream = ReadResponse(request);
            if (objStream != null)
            {
                using (TextReader reader = new StreamReader(objStream))
                {
                    string xmlText = reader.ReadToEnd();
                    XmlDocument doc = new XmlDocument { InnerXml = xmlText };

                    if (doc.DocumentElement != null)
                    {
                        responseString = doc.DocumentElement.InnerText;
                    }
                }
            }
            return responseString;
        }

        /// <summary>
        /// Read a response from the server.
        /// </summary>
        /// <param name="request">The web request handle</param>
        /// <returns>The response stream</returns>
        internal static Stream ReadResponse(WebRequest request)
        {
            if (request == null)
            {
                return null;
            }

            DateTime apiTimer = DateTime.Now;
            Stream objStream = request.GetResponse().GetResponseStream();
            TimeSpan apiDuration = DateTime.Now - apiTimer;

            // Report API calls that take a second or more.
            if (apiDuration.TotalSeconds >= 1)
            {
                LogFile.WriteLine("{0} : {1} seconds", request.RequestUri, apiDuration.TotalSeconds);
            }
            return objStream;
        }

        /// <summary>
        /// Construct an HttpWebRequest object using the specified CIXAPI function, format
        /// and method. Any authentication rider is attached to the header as required and
        /// the appropriate content type set.
        /// </summary>
        /// <param name="apiFunction">The API function name</param>
        /// <param name="username">Authentication username</param>
        /// <param name="password">Authentication password</param>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <param name="method">The API method required (GET or POST)</param>
        /// <param name="postObject">For POST, this is the object to be posted</param>
        /// <param name="queryString">Optional query string for the URL</param>
        /// <returns>A constructed HttpWebRequest</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static HttpWebRequest Create(string apiFunction, string username, string password, APIFormat format, APIMethod method, object postObject, string queryString)
        {
            HttpWebRequest request;
            byte[] postMessageBytes;

            if (username == null || password == null)
            {
                return null;
            }

            if (method == APIMethod.POST)
            {
                var o = postObject as Image;
                if (o != null)
                {
                    Image postImage = o;

                    ImageConverter converter = new ImageConverter();
                    postMessageBytes = (byte[])converter.ConvertTo(postImage, typeof(byte[]));

                    if (postMessageBytes == null)
                    {
                        return null;
                    }

                    request = (HttpWebRequest)WebRequest.Create(MakeURL(apiFunction, format, queryString));
                    request.Method = APIMethodToString(method);

                    if (ImageFormat.Jpeg.Equals(postImage.RawFormat))
                    {
                        request.ContentType = "image/jpeg";
                    }
                    if (ImageFormat.Gif.Equals(postImage.RawFormat))
                    {
                        request.ContentType = "image/gif";
                    }
                    if (ImageFormat.Png.Equals(postImage.RawFormat))
                    {
                        request.ContentType = "image/png";
                    }
                    if (ImageFormat.Bmp.Equals(postImage.RawFormat))
                    {
                        request.ContentType = "image/bitmap";
                    }
                    request.ContentLength = postMessageBytes.Length;
                }
                else
                {
                    var s = postObject as string;
                    if (s != null)
                    {
                        string postString = s;

                        ASCIIEncoding encoder = new ASCIIEncoding();
                        postMessageBytes = encoder.GetBytes(postString);

                        request = (HttpWebRequest)WebRequest.Create(MakeURL(apiFunction, format, queryString));
                        request.Method = APIMethodToString(method);

                        request.ContentLength = postMessageBytes.Length;
                        request.ContentType = "application/text";
                    }
                    else
                    {
                        string postMessageXml;

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (StreamReader reader = new StreamReader(memoryStream))
                            {
                                DataContractSerializer serializer = new DataContractSerializer(postObject.GetType());
                                serializer.WriteObject(memoryStream, postObject);
                                memoryStream.Position = 0;
                                postMessageXml = reader.ReadToEnd();
                            }
                        }

                        // Messages are posted as 7-bit ASCII.
                        UTF8Encoding encoder = new UTF8Encoding();
                        postMessageBytes = encoder.GetBytes(postMessageXml);

                        request = (HttpWebRequest)WebRequest.Create(MakeURL(apiFunction, format, queryString));
                        request.Method = APIMethodToString(method);

                        request.ContentLength = encoder.GetByteCount(postMessageXml);
                        request.ContentType = "application/xml; charset=utf-8";
                        request.Accept = "application/xml; charset=utf-8";
                    }
                }
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(MakeURL(apiFunction, format, queryString));
                request.Method = APIMethodToString(method);

                postMessageBytes = null;

                if (format == APIFormat.XML)
                {
                    request.ContentType = "application/xml; charset=utf-8";
                    request.Accept = "application/xml; charset=utf-8";
                }
                else
                {
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                }
            }

            string authInfo = username + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.PreAuthenticate = true;

            if (postMessageBytes != null)
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(postMessageBytes, 0, postMessageBytes.Length);
                dataStream.Close();
            }

            return request;
        }

        /// <summary>
        /// Create an API URL combining the function name, base and format.
        /// </summary>
        /// <param name="apiFunction">The API function name</param>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <param name="queryString">Optional query string</param>
        /// <returns>A string containing the requested URL</returns>
        private static string MakeURL(string apiFunction, APIFormat format, string queryString)
        {
            StringBuilder url = new StringBuilder();
            url.AppendFormat("{0}{1}.{2}", APIBase, apiFunction, APIFormatToString(format));
            if (queryString != null)
            {
                url.AppendFormat("?{0}", queryString);
            }
            return url.ToString();
        }

        /// <summary>
        /// Converts the specified APIFormat to a string.
        /// </summary>
        /// <param name="format">The API format requested (JSON or XML)</param>
        /// <returns>The string representing the API format</returns>
        private static string APIFormatToString(APIFormat format)
        {
            return format == APIFormat.JSON ? "json" : "xml";
        }

        /// <summary>
        /// Converts the specified APIMethod to a string.
        /// </summary>
        /// <param name="method">The API method required (GET or POST)</param>
        /// <returns>The string representing the API method</returns>
        private static string APIMethodToString(APIMethod method)
        {
            return method == APIMethod.GET ? "GET" : "POST";
        }
    }
}