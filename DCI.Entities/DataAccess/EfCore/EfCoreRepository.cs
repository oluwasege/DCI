// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="EfCoreRepository.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FSDH.Core.Collections;
using FSDH.Core.DataAccess.EfCore.Context;
using FSDH.Core.DataAccess.Repository;
using FSDH.Entities.Common;

namespace FSDH.Core.DataAccess.EfCore
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext which contains <typeparamref name="TEntity" />.</typeparam>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    [ExcludeFromCodeCoverage]
    public class EfCoreRepository<TDbContext, TEntity, TPrimaryKey> :
        BaseRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        /// <summary>
        /// The database context provider
        /// </summary>
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider">The database context provider.</param>
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        /// <value>The context.</value>
        public virtual TDbContext Context => _dbContextProvider.GetDbContext();

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        /// <value>The table.</value>
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public virtual DbConnection Connection
        {
            get
            {
                var connection = Context.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open) connection.Open();

                return connection;
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        /// <summary>
        /// Gets all including.
        /// </summary>
        /// <param name="propertySelectors">The property selectors.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public override IQueryable<TEntity> GetAllIncluding(
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (!propertySelectors.IsNullOrEmpty())
                foreach (var propertySelector in propertySelectors)
                    query = query.Include(propertySelector);

            return query;
        }

        /// <summary>
        /// get all list as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        /// <summary>
        /// get all list as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// single as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A Task&lt;TEntity&gt; representing the asynchronous operation.</returns>
        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        /// <summary>
        /// first or default as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Task&lt;TEntity&gt; representing the asynchronous operation.</returns>
        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// first or default as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A Task&lt;TEntity&gt; representing the asynchronous operation.</returns>
        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Exists the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public override Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().AnyAsync(predicate);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        /// <summary>
        /// Inserts the and get identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TPrimaryKey.</returns>
        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (entity.IsTransient()) Context.SaveChanges();

            return entity.Id;
        }

        /// <summary>
        /// insert and get identifier as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task&lt;TPrimaryKey&gt; representing the asynchronous operation.</returns>
        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (entity.IsTransient()) await Context.SaveChangesAsync();

            return entity.Id;
        }

        /// <summary>
        /// Inserts the or update and get identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TPrimaryKey.</returns>
        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (entity.IsTransient()) Context.SaveChanges();

            return entity.Id;
        }

        /// <summary>
        /// insert or update and get identifier as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task&lt;TPrimaryKey&gt; representing the asynchronous operation.</returns>
        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (entity.IsTransient()) await Context.SaveChangesAsync();

            return entity.Id;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null) Delete(entity);

            //Could not found the entity, do nothing.
        }

        /// <summary>
        /// count as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        /// <summary>
        /// count as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        /// <summary>
        /// long count as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;System.Int64&gt; representing the asynchronous operation.</returns>
        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        /// <summary>
        /// long count as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A Task&lt;System.Int64&gt; representing the asynchronous operation.</returns>
        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        /// <summary>
        /// Attaches if not.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null) return;

            Table.Attach(entity);
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <returns>DbContext.</returns>
        public DbContext GetDbContext()
        {
            return Context;
        }

        /// <summary>
        /// Ensures the collection loaded asynchronous.
        /// </summary>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="collectionExpression">The collection expression.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task.</returns>
        public Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        }

        /// <summary>
        /// Ensures the property loaded asynchronous.
        /// </summary>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task.</returns>
        public Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return Context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        /// <summary>
        /// Gets from change tracker or null.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TEntity.</returns>
        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }
    }

    /// <summary>
    /// Class EfCoreRepository.
    /// Implements the <see cref="FSDH.Core.DataAccess.EfCore.EfCoreRepository{TDbContext, TEntity, System.Int32}" />
    /// Implements the <see cref="FSDH.Core.DataAccess.Repository.IRepository{TEntity}" />
    /// </summary>
    /// <typeparam name="TDbContext">The type of the t database context.</typeparam>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="FSDH.Core.DataAccess.EfCore.EfCoreRepository{TDbContext, TEntity, System.Int32}" />
    /// <seealso cref="FSDH.Core.DataAccess.Repository.IRepository{TEntity}" />
    [ExcludeFromCodeCoverage]
    public class EfCoreRepository<TDbContext, TEntity> : EfCoreRepository<TDbContext, TEntity, int>,
        IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TDbContext, TEntity}"/> class.
        /// </summary>
        /// <param name="dbContextProvider">The database context provider.</param>
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}