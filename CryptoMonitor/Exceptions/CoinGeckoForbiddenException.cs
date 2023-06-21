using System;
using System.Runtime.Serialization;

namespace CryptoMonitor.Exceptions
{
    [Serializable]
    public class CoinGeckoForbiddenException : Exception
    {
        public CoinGeckoForbiddenException()
        {
        }

        public CoinGeckoForbiddenException(string? message) : base(message)
        {
        }

        public CoinGeckoForbiddenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CoinGeckoForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}