// *****************************************************
// CIXReader
// BackTrackArray.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 26/02/2015 21:35
//  
// Copyright (C) 2013-2014 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Collections.Generic;

namespace CIXReader.Utilities
{
    public sealed class BackTrackArray
    {
        private readonly List<Address> _array;
        private readonly int _maxItems;
        private int _queueIndex;

        /// <summary>
        /// Initialises a new BackTrackArray with the specified maximum number of items.
        /// </summary>
        /// <param name="theMax">Maximum number of items</param>
        public BackTrackArray(int theMax)
        {
            _maxItems = theMax;
            _queueIndex = -1;
            _array = new List<Address>(_maxItems);
        }

        /// <summary>
        /// Returns true if we're at the start of the queue.
        /// </summary>
        public bool IsAtStartOfQueue
        {
            get { return _queueIndex <= 0; }
        }

        /// <summary>
        /// Returns true if we're at the end of the queue.
        /// </summary>
        public bool IsAtEndOfQueue
        {
            get { return _queueIndex >= _array.Count - 1; }
        }

        /// <summary>
        /// Removes an item from the tail of the queue as long as the queue is not empty and returns the backtrack data.
        /// </summary>
        /// <param name="item">Ref of the string to be set with the backtrack data</param>
        /// <returns>True if an item was returned, false otherwise</returns>
        public bool PreviousItemAtQueue(ref Address item)
        {
            if (_queueIndex > 0)
            {
                item = _array[--_queueIndex];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes an item from the tail of the queue as long as the queue is not empty and returns the backtrack data.
        /// </summary>
        /// <param name="item">Ref of the string to be set with the backtrack data</param>
        /// <returns>True if an item was returned, false otherwise</returns>
        public bool NextItemAtQueue(ref Address item)
        {
            if (_queueIndex < _array.Count - 1)
            {
                item = _array[++_queueIndex];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds an item to the queue. The new item is added at queueIndex
        /// which is the most recent position to which the user has tracked
        /// (usually the end of the array if no tracking has occurred). If
        /// queueIndex is in the middle of the array, we remove all items
        /// to the right (from queueIndex+1 onwards) in order to define a
        /// new 'head' position. This produces the expected results when tracking
        /// from the new item inserted back to the most recent item.
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddToQueue(Address item)
        {
            while (_queueIndex + 1 < _array.Count)
            {
                _array.RemoveAt(_queueIndex + 1);
            }
            if (_array.Count == _maxItems)
            {
                _array.RemoveAt(0);
                --_queueIndex;
            }
            if (_array.Count > 0)
            {
                Address itemAddress = _array[_array.Count - 1];
                if (itemAddress == item)
                {
                    return;
                }
            }
            _array.Add(item);
            ++_queueIndex;
        }
    }
}