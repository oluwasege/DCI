﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace DCI.Core.Utils
{
    /// <summary>
    /// Used to generate Ids.
    /// </summary>
    public interface IGuidGenerator
    {
        /// <summary>
        /// Creates a GUID.
        /// </summary>
        /// <returns>Guid.</returns>
        Guid Create();
    }

    /// <summary>
    /// Implements <see cref="IGuidGenerator" /> by creating sequential Guids.
    /// This code is taken from
    /// https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SequentialGuidGenerator : IGuidGenerator
    {
        /// <summary>
        /// Database type to generate GUIDs.
        /// </summary>
        public enum SequentialGuidDatabaseType
        {
            /// <summary>
            /// The SQL server
            /// </summary>
            SqlServer,

            /// <summary>
            /// The oracle
            /// </summary>
            Oracle,

            /// <summary>
            /// My SQL
            /// </summary>
            MySql,

            /// <summary>
            /// The postgre SQL
            /// </summary>
            PostgreSql
        }

        /// <summary>
        /// Describes the type of a sequential GUID value.
        /// </summary>
        public enum SequentialGuidType
        {
            /// <summary>
            /// The GUID should be sequential when formatted using the
            /// <see cref="Guid.ToString()" /> method.
            /// </summary>
            SequentialAsString,

            /// <summary>
            /// The GUID should be sequential when formatted using the
            /// <see cref="Guid.ToByteArray" /> method.
            /// </summary>
            SequentialAsBinary,

            /// <summary>
            /// The sequential portion of the GUID should be located at the end
            /// of the Data4 block.
            /// </summary>
            SequentialAtEnd
        }

        /// <summary>
        /// The RNG
        /// </summary>
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Prevents a default instance of the <see cref="SequentialGuidGenerator" /> class from being created.
        /// Use <see cref="Instance" />.
        /// </summary>
        private SequentialGuidGenerator()
        {
            DatabaseType = SequentialGuidDatabaseType.SqlServer;
        }

        /// <summary>
        /// Gets the singleton <see cref="SequentialGuidGenerator" /> instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SequentialGuidGenerator Instance { get; } = new SequentialGuidGenerator();

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
        public SequentialGuidDatabaseType DatabaseType { get; set; }

        /// <summary>
        /// Creates a GUID.
        /// </summary>
        /// <returns>Guid.</returns>
        public Guid Create()
        {
            return Create(DatabaseType);
        }

        /// <summary>
        /// Creates the specified database type.
        /// </summary>
        /// <param name="databaseType">Type of the database.</param>
        /// <returns>Guid.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Guid Create(SequentialGuidDatabaseType databaseType)
        {
            switch (databaseType)
            {
                case SequentialGuidDatabaseType.SqlServer:
                    return Create(SequentialGuidType.SequentialAtEnd);

                case SequentialGuidDatabaseType.Oracle:
                    return Create(SequentialGuidType.SequentialAsBinary);

                case SequentialGuidDatabaseType.MySql:
                    return Create(SequentialGuidType.SequentialAsString);

                case SequentialGuidDatabaseType.PostgreSql:
                    return Create(SequentialGuidType.SequentialAsString);

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates the specified unique identifier type.
        /// </summary>
        /// <param name="guidType">Type of the unique identifier.</param>
        /// <returns>Guid.</returns>
        public Guid Create(SequentialGuidType guidType)
        {
            // We start with 16 bytes of cryptographically strong random data.
            var randomBytes = new byte[10];
            Rng.Locking(r => r.GetBytes(randomBytes));

            // An alternate method: use a normally-created GUID to get our initial
            // random data:
            // This is faster than using RNGCryptoServiceProvider, but I don't
            // recommend it because the .NET Framework makes no guarantee of the
            // randomness of GUID data, and future versions (or different
            // implementations like Mono) might use a different method.

            // Now we have the random basis for our GUID.  Next, we need to
            // create the six-byte block which will be our timestamp.

            // We start with the number of milliseconds that have elapsed since
            // DateTime.MinValue.  This will form the timestamp.  There's no use
            // being more specific than milliseconds, since DateTime.Now has
            // limited resolution.

            // Using millisecond resolution for our 48-bit timestamp gives us
            // about 5900 years before the timestamp overflows and cycles.
            // Hopefully this should be sufficient for most purposes. :)
            var timestamp = DateTime.UtcNow.Ticks / 10000L;

            // Then get the bytes
            var timestampBytes = BitConverter.GetBytes(timestamp);

            // Since we're converting from an Int64, we have to reverse on
            // little-endian systems.
            if (BitConverter.IsLittleEndian) Array.Reverse(timestampBytes);

            var guidBytes = new byte[16];

            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:

                    // For string and byte-array version, we copy the timestamp first, followed
                    // by the random data.
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to compensate for the fact
                    // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
                    // respectively.  That means that it switches the order on little-endian
                    // systems.  So again, we have to reverse.
                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;

                case SequentialGuidType.SequentialAtEnd:

                    // For sequential-at-the-end versions, we copy the random data first,
                    // followed by the timestamp.
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
                default:
                    break;
            }

            return new Guid(guidBytes);
        }
    }
}