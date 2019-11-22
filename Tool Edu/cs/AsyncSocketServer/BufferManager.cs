using System.Collections.Generic;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    /// <summary>
    /// This class creates a single large buffer which can be divided up and assigned to SocketAsyncEventArgs objects for use
    /// with each socket I/O operation.  This enables bufffers to be easily reused and gaurds against fragmenting heap memory.
    ///
    /// The operations exposed on the BufferManager class are not thread safe.
    /// </summary>
    internal class BufferManager
    {
        private readonly int _mNumBytes;                 // the total number of bytes controlled by the buffer pool
        private byte[] _mBuffer;                // the underlying byte array maintained by the Buffer Manager
        private readonly Stack<int> _mFreeIndexPool;     // giai index con thua
        private int _mCurrentIndex; //index hien tai
        private readonly int _mBufferSize; //kich thuoc = byte bo dem .
        private readonly int _mAftermaxindex;

        public BufferManager(int totalBytes, int bufferSize)
        {
            _mNumBytes = totalBytes;
            _mCurrentIndex = 0;
            _mBufferSize = bufferSize;
            _mFreeIndexPool = new Stack<int>();
            _mAftermaxindex = _mNumBytes - _mBufferSize;
        }

        /// <summary>
        /// Allocates buffer space used by the buffer pool
        /// </summary>
        public void InitBuffer()
        {
            // create one big large buffer and divide that out to each SocketAsyncEventArg object
            _mBuffer = new byte[_mNumBytes];
        }

        /// <summary>
        /// Assigns a buffer from the buffer pool to the specified SocketAsyncEventArgs object
        /// </summary>
        /// <returns>true if the buffer was successfully set, else false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (_mFreeIndexPool.Count > 0)
            {
                args.SetBuffer(_mBuffer, _mFreeIndexPool.Pop(), _mBufferSize);
            }
            else
            {
                if (_mAftermaxindex < _mCurrentIndex) //index = maximum index.
                {
                    return false;
                }
                args.SetBuffer(_mBuffer, _mCurrentIndex, _mBufferSize);
                _mCurrentIndex += _mBufferSize;
            }
            return true;
        }

        /// <summary>
        /// Removes the buffer from a SocketAsyncEventArg object. This frees the buffer back to the
        /// buffer pool
        /// </summary>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            _mFreeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}