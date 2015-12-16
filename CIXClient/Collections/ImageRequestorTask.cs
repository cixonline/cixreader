// *****************************************************
// CIXReader
// ImageRequestorTask.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 17/10/2013 11:25 AM
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace CIXClient.Collections
{
    /// <summary>
    /// Class that handles image requests from a remote server and caches them
    /// </summary>
    public sealed class ImageRequestorTask
    {
        /// <summary>
        /// Internal class that defines a single image request
        /// </summary>
        internal sealed class SingleImageRequest
        {
            public string URL { get; set; }

            public int MaxWidth { get; set; }

            public int MaxHeight { get; set; }

            public ImageRetrieved Event { get; set; }

            public object Parameter { get; set; }
        }

        /// <summary>
        /// Class that holds a single cache entry which comprises the image and
        /// the timestamp when the image was retrieved.
        /// </summary>
        internal sealed class ImageCacheEntry
        {
            public Image Image { get; set; }

            public DateTime Timestamp { get; set; }
        }

        private readonly Dictionary<string, ImageCacheEntry> _imageCache = new Dictionary<string, ImageCacheEntry>();
        private Timer _timer;

        private const int ImageCachePurgeFrequency = 60;  // Frequency of cache purge checks in seconds.
        private const int ImageCacheExpiryTime = 30;      // Maximum time in minutes an image is cached

        /// <summary>
        /// Defines the delegate for the callback when the image is retrieved.
        /// </summary>
        /// <param name="image">The image retrieved</param>
        /// <param name="parameter">A user specified parameter</param>
        public delegate void ImageRetrieved(Image image, object parameter);

        /// <summary>
        /// Create a timer that runs every minute to purge old items from the cache.
        /// </summary>
        internal void Load()
        {
            _timer = new Timer(ImageCacheCleanup, null, ImageCachePurgeFrequency * 1000, ImageCachePurgeFrequency * 1000);
        }

        /// <summary>
        /// Stop the timed task running and dispose of it.
        /// </summary>
        internal void Unload()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// Purge all images from the cache that are older than ImageCacheExpiryTime minutes.
        /// </summary>
        /// <param name="obj">The timer object</param>
        private void ImageCacheCleanup(object obj)
        {
            DateTime expiryPoint = DateTime.Now.AddMinutes(-ImageCacheExpiryTime);
            foreach (string url in from url in _imageCache.Keys.ToList() let entry = _imageCache[url] where entry.Timestamp < expiryPoint select url)
            {
                _imageCache.Remove(url);
                LogFile.WriteLine("Image {0} removed from cache", url);
            }
        }

        /// <summary>
        /// Requests a new image and return it via the callback. If the image is in the cache, it is
        /// returned from there. Otherwise we queue up the request and spin up a thread to retrieve the
        /// image and invoke the callback from there.
        /// </summary>
        /// <param name="imageURL">The URL of the image</param>
        /// <param name="maxWidth">The maximum width of the requested image</param>
        /// <param name="maxHeight">The maximum height of the requested image</param>
        /// <param name="callback">The callback</param>
        /// <param name="callbackParameter">A user specified parameter for the callback</param>
        /// <returns>An Image object that represents the specified image or null if it is not yet available</returns>
        public Image NewRequest(string imageURL, int maxWidth, int maxHeight, ImageRetrieved callback, object callbackParameter)
        {
            if (_imageCache.ContainsKey(imageURL))
            {
                LogFile.WriteLine("Image {0} retrieved from cache", imageURL);

                return (_imageCache[imageURL].Image != null) ? ScaleImage(_imageCache[imageURL].Image, maxWidth, maxHeight) : null;
            }

            // Spin up a new thread to obtain any images
            Thread t = new Thread(() =>
            {
                try
                {
                    LogFile.WriteLine("Requesting image {0}", imageURL);

                    // Convert Dropbox share URLs into raw links
                    string realImageURL = imageURL;
                    if (realImageURL.StartsWith("https://www.dropbox.com", StringComparison.Ordinal))
                    {
                        realImageURL += "?raw=1";
                    }

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(realImageURL);
                    webRequest.AllowWriteStreamBuffering = true;

                    using (WebResponse _WebResponse = webRequest.GetResponse())
                    {
                        Stream stream = _WebResponse.GetResponseStream();
                        if (stream != null)
                        {
                            Image _tmpImage = Image.FromStream(stream);
                            _imageCache[imageURL] = new ImageCacheEntry {Image = _tmpImage, Timestamp = DateTime.Now};

                            Image image = ScaleImage(_tmpImage, maxWidth, maxHeight);
                            callback(image, callbackParameter);

                            LogFile.WriteLine("Image {0} retrieved from server and cached", imageURL);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogFile.WriteLine("Failed to load {0}: {1}", imageURL, e.Message);

                    // Don't try and request this image again for a while.
                    _imageCache[imageURL] = new ImageCacheEntry {Image = null, Timestamp = DateTime.Now};
                }
            });
            t.Start();
            return null;
        }

        /// <summary>
        /// Scale the specified image so that it fits within the maximum width and height.
        /// </summary>
        /// <param name="image">The image to scale</param>
        /// <param name="maxWidth">The maximum width</param>
        /// <param name="maxHeight">The maximum height</param>
        /// <returns>The image scaled to fit</returns>
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double) maxWidth / image.Width;
            var ratioY = (double) maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}