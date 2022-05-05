// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="UnitOfWorkExtensions.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace FSDH.Core.DataAccess.EfCore.UnitOfWork
{
    /// <summary>
    /// Class UnitOfWorkExtensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the t database context.</typeparam>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <returns>TDbContext.</returns>
        /// <exception cref="ArgumentNullException">unitOfWork</exception>
        /// <exception cref="ArgumentException">unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName - unitOfWork</exception>
        public static TDbContext GetDbContext<TDbContext>(this IUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            if (!(unitOfWork is EfCoreUnitOfWork))
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfCoreUnitOfWork).FullName,
                    nameof(unitOfWork));

            return (unitOfWork as EfCoreUnitOfWork).GetOrCreateDbContext<TDbContext>();
        }
    }
}