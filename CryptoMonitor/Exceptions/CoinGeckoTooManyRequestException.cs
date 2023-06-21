using System;
using System.Runtime.Serialization;

namespace CryptoMonitor.Exceptions
{
    [Serializable]
    public class CoinGeckoTooManyRequestException : Exception
    {
        public CoinGeckoTooManyRequestException()
        {
        }

        public CoinGeckoTooManyRequestException(string? message) : base(message)
        {
        }

        public CoinGeckoTooManyRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CoinGeckoTooManyRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}