using System;
using System.Runtime.Serialization;

namespace CryptoMonitor.Exceptions
{
    [Serializable]
    public class UnsupportedCoinException : Exception
    {
        public UnsupportedCoinException()
        {
        }

        public UnsupportedCoinException(string? message) : base(message)
        {
        }

        public UnsupportedCoinException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnsupportedCoinException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}