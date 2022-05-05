// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="EfCoreUnitOfWork.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FSDH.Core.DataAccess.EfCore.UnitOfWork;

namespace FSDH.Core.DataAccess.EfCore
{
    /// <summary>
    /// Class EfCoreUnitOfWork.
    /// Implements the <see cref="FSDH.Core.DataAccess.EfCore.UnitOfWork.IUnitOfWork" />
    /// </summary>
    /// <seealso cref="FSDH.Core.DataAccess.EfCore.UnitOfWork.IUnitOfWork" />
    [ExcludeFromCodeCoverage]
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The context
        /// </summary>
        public readonly DbContext _context;
        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreUnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EfCoreUnitOfWork(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                _context.Database.OpenConnection();

            _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            _context.ChangeTracker.DetectChanges();

            SaveChanges();
            _context.Database.CommitTransaction();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// PerfoFSDH application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        public void Rollback()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }

        /// <summary>
        /// Gets the or create database context.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the t database context.</typeparam>
        /// <returns>TDbContext.</returns>
        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return (TDbContext) _context;
        }
    }
}