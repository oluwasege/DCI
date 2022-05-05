// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="EntityTypeInfo.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;

namespace FSDH.Core.DataAccess.EfCore
{
    /// <summary>
    /// Class EntityTypeInfo.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EntityTypeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeInfo"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        public EntityTypeInfo(Type entityType, Type declaringType)
        {
            EntityType = entityType;
            DeclaringType = declaringType;
        }

        /// <summary>
        /// Type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; }

        /// <summary>
        /// DbContext type that has DbSet property.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public Type DeclaringType { get; }
    }
}