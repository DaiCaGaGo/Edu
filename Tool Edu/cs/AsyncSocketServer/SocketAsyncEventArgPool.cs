using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    /// <summary>
    /// Represents a collection of resusable SocketAsyncEventArgs objects.
    /// </summary>
    internal class SocketAsyncEventArgsPool
    {
        private readonly Stack<SocketAsyncEventArgs> _mPool;

        /// <summary>
        /// Initializes the object pool to the specified size
        /// </summary>
        /// <param name="capacity">The maximum number of SocketAsyncEventArgs objects the pool can hold</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            _mPool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// Add a SocketAsyncEventArg instance to the pool
        /// </summary>
        /// <param name="item">The SocketAsyncEventArgs instance to add to the pool</param>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException(@"Items added to a " + @"SocketAsyncEventArgsPool cannot be null"); }
            lock (_mPool)
            {
                _mPool.Push(item);
            }
        }

        /// <summary>
        /// Removes a SocketAsyncEventArgs instance from the pool
        /// </summary>
        /// <returns>The object removed from the pool</returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (_mPool)
            {
                return _mPool.Pop();
            }
        }

        /// <summary>
        /// The number of SocketAsyncEventArgs instances in the pool
        /// </summary>
        public int Count
        {
            get { return _mPool.Count; }
        }
    }
}