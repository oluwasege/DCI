using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DCI.Core.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using ReflectionHelper = DCI.Core.Reflection.ReflectionHelper;

namespace DCI.Entities.DataAccess.EfCore.Context
{
    /// <summary>
    /// Base class for all DbContext classes in the application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class
        BaseDbContext : IdentityDbContext<DCIUser, DCIRole, Guid, DCIUserClaim, DCIUserRole, DCIUserLogin, DCIRoleClaim, DCIUserToken>
    {
        /// <summary>
        /// The configure global filters method information
        /// </summary>
        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo =
            typeof(BaseDbContext).GetMethod(nameof(ConfigureGlobalFilters),
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />.</param>
        protected BaseDbContext(DbContextOptions options)
            : base(options)
        {
            InitializeDbContext();
        }

        /// <summary>
        /// Reference to GUID generator.
        /// </summary>
        /// <value>The unique identifier generator.</value>
        private IGuidGenerator GuidGenerator { get; set; }

        /// <summary>
        /// Initializes the database context.
        /// </summary>
        private void InitializeDbContext()
        {
            SetNullsForInjectedProperties();
        }

        /// <summary>
        /// Sets the nulls for injected properties.
        /// </summary>
        private void SetNullsForInjectedProperties()
        {
            GuidGenerator = SequentialGuidGenerator.Instance;
        }

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { builder, entityType });
        }

        /// <summary>
        /// Configures the global filters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="entityType">Type of the entity.</param>
        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType != null || !ShouldFilterEntity<TEntity>(entityType)) return;
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null) modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
        }

        /// <summary>
        /// Shoulds the filter entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity))) return true;

            return false;
        }

        /// <summary>
        /// Creates the filter expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.Boolean&gt;&gt;.</returns>
        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (!typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity))) return expression;
            /* This condition should normally be defined as below:
                    * !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted
                    * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                    * So, we made a workaround to make it working. It works same as above.
                    */

            Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted;
            expression = softDeleteFilter;

            return expression;
        }

        /// <summary>
        /// <para>
        /// Saves all changes made in this context to the database.
        /// </para>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="AppDbConcurrencyException"></exception>
        public override int SaveChanges()
        {
            try
            {
                ApplyAbpConcepts();
                var result = base.SaveChanges();
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AppDbConcurrencyException(ex.Message, ex);
            }
        }

        /// <summary>
        /// save changes as an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
        /// <exception cref="AppDbConcurrencyException"></exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ApplyAbpConcepts();
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AppDbConcurrencyException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Applies the abp concepts.
        /// </summary>
        protected virtual void ApplyAbpConcepts()
        {
            foreach (var entry in ChangeTracker.Entries().ToList()) ApplyAbpConcepts(entry, null);
        }

        /// <summary>
        /// Applies the abp concepts.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void ApplyAbpConcepts(EntityEntry entry, long? userId)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyAbpConceptsForAddedEntity(entry, userId);
                    break;

                case EntityState.Modified:
                    ApplyAbpConceptsForModifiedEntity(entry, userId);
                    break;

                case EntityState.Deleted:
                    ApplyAbpConceptsForDeletedEntity(entry, userId);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Applies the abp concepts for added entity.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry, long? userId)
        {
            CheckAndSetId(entry);
            SetCreationAuditProperties(entry.Entity, userId);
        }

        /// <summary>
        /// Applies the abp concepts for modified entity.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry, long? userId)
        {
            SetModificationAuditProperties(entry.Entity, userId);
            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                SetDeletionAuditProperties(entry.Entity, userId);
        }

        /// <summary>
        /// Applies the abp concepts for deleted entity.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry, long? userId)
        {
            CancelDeletionForSoftDelete(entry);
            SetDeletionAuditProperties(entry.Entity, userId);
        }

        /// <summary>
        /// Checks the and set identifier.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            //Set GUID Ids
            var entity = entry.Entity as IEntity<Guid>;
            if (entity == null || entity.Id != Guid.Empty) return;
            var dbGeneratedAttr = ReflectionHelper
                .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                    entry.Property("Id").Metadata.PropertyInfo
                );

            if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                entity.Id = GuidGenerator.Create();
        }

        /// <summary>
        /// Sets the creation audit properties.
        /// </summary>
        /// <param name="entityAsObj">The entity as object.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
        {
            EntityAuditingHelper.SetCreationAuditProperties(entityAsObj, userId);
        }

        /// <summary>
        /// Sets the modification audit properties.
        /// </summary>
        /// <param name="entityAsObj">The entity as object.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void SetModificationAuditProperties(object entityAsObj, long? userId)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entityAsObj, userId);
        }

        /// <summary>
        /// Cancels the deletion for soft delete.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete)) return;

            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
        }

        /// <summary>
        /// Sets the deletion audit properties.
        /// </summary>
        /// <param name="entityAsObj">The entity as object.</param>
        /// <param name="userId">The user identifier.</param>
        protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                var entity = entityAsObj.As<IHasDeletionTime>();

                entity.DeletionTime ??= Clock.Now;
            }

            if (!(entityAsObj is IDeletionAudited)) return;

            var delEntity = entityAsObj.As<IDeletionAudited>();

            if (delEntity.DeletedUserId != null) return;
            delEntity.DeletedUserId = userId;
        }

        /// <summary>
        /// Combines the expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression1">The expression1.</param>
        /// <param name="expression2">The expression2.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1,
            Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// Class ReplaceExpressionVisitor.
        /// Implements the <see cref="System.Linq.Expressions.ExpressionVisitor" />
        /// </summary>
        /// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            /// <summary>
            /// The new value
            /// </summary>
            private readonly Expression _newValue;
            /// <summary>
            /// The old value
            /// </summary>
            private readonly Expression _oldValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplaceExpressionVisitor"/> class.
            /// </summary>
            /// <param name="oldValue">The old value.</param>
            /// <param name="newValue">The new value.</param>
            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            /// <summary>
            /// Visits the specified node.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns>Expression.</returns>
            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }

    //public interface ISoftDelete
    //{
    //    /// <summary>
    //    /// Gets or sets a value indicating whether this instance is deleted.
    //    /// </summary>
    //    /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
    //    bool IsDeleted { get; set; }
    //}
}