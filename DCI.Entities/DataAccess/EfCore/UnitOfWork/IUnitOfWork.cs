// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="IUnitOfWork.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using FSDH.Core.DataAccess.Repository;
using FSDH.Entities.Auditing;
using System;
using System.Threading.Tasks;

namespace FSDH.Core.DataAccess.EfCore.UnitOfWork
{
    /// <summary>
    /// Interface IUnitOfWork
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();
        //IRepository<TEntity> Repository<TEntity>() where TEntity : AuditedEntity;

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits this instance.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        void Rollback();
    }
}