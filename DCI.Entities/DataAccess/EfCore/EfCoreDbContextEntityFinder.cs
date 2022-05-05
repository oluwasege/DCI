// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="EfCoreDbContextEntityFinder.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using FSDH.Core.Reflection;
using FSDH.Entities.Common;

namespace FSDH.Core.DataAccess.EfCore
{
    /// <summary>
    /// Class EfCoreDbContextEntityFinder.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EfCoreDbContextEntityFinder
    {
        /// <summary>
        /// Gets the entity type infos.
        /// </summary>
        /// <param name="dbContextType">Type of the database context.</param>
        /// <returns>IEnumerable&lt;EntityTypeInfo&gt;.</returns>
        public static IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                        typeof(IEntity<>))
                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }
    }
}