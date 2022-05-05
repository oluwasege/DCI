using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DCI.Core.Utils
{
    /// <summary>
    /// Class AppDbConcurrencyException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AppDbConcurrencyException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException" /> object.
        /// </summary>
        public AppDbConcurrencyException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="AbpException" /> object.
        /// </summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="context">The context.</param>
        protected AppDbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException" /> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AppDbConcurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException" /> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AppDbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}