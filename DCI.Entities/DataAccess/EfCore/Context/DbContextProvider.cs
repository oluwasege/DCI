// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="DbContextProvider.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using FSDH.Core.DataAccess.EfCore.UnitOfWork;

namespace FSDH.Core.DataAccess.EfCore.Context
{
    /// <summary>
    /// Interface IDbContextProvider
    /// </summary>
    /// <typeparam name="TDbContext">The type of the t database context.</typeparam>
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <returns>TDbContext.</returns>
        TDbContext GetDbContext();
    }

    /// <summary>
    /// Class UnitOfWorkDbContextProvider. This class cannot be inherited.
    /// Implements the <see cref="FSDH.Core.DataAccess.EfCore.Context.IDbContextProvider{TDbContext}" />
    /// </summary>
    /// <typeparam name="TDbContext">The type of the t database context.</typeparam>
    /// <seealso cref="FSDH.Core.DataAccess.EfCore.Context.IDbContextProvider{TDbContext}" />
    [ExcludeFromCodeCoverage]
    public sealed class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkDbContextProvider{TDbContext}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UnitOfWorkDbContextProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <returns>TDbContext.</returns>
        public TDbContext GetDbContext()
        {
            return _unitOfWork.GetDbContext<TDbContext>();
        }
    }
}