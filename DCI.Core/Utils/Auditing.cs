using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FSDH.Entities.Common;

namespace DCI.Core.Utils
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedEntity{TPrimaryKey}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntity : FullAuditedEntity<int>, IEntity
    {
    }
    public interface IEntity<Key>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        /// <value>The identifier.</value>
        Key Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="Id" />).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();
    }

    /// <summary>
    /// A shortcut of <see cref="IEntity{Key}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    public interface IEntity : IEntity<int>
    {
    }

    /// <summary>
    /// A shortcut of <see cref="Entity{TPrimaryKey}" /> for most used primary key type (<see cref="int" />).
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<int>, IEntity
    {
    }

    /// <summary>
    /// Basic implementation of IEntity interface.
    /// An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        /// <value>The identifier.</value>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default)) return true;

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int)) return Convert.ToInt32(Id) <= 0;

            if (typeof(TPrimaryKey) == typeof(long)) return Convert.ToInt64(Id) <= 0;

            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TPrimaryKey>)) return false;

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj)) return true;

            //Transient objects are not considered as equal
            var other = (Entity<TPrimaryKey>)obj;
            if (IsTransient() && other.IsTransient()) return false;

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) &&
                !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis)) return false;

            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (Id == null) return 0;

            return Id.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
    /// <summary>
    /// Implements <see cref="IFullAudited" /> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        /// <value>The deleter user identifier.</value>
        public virtual long? DeletedUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        /// <value>The deletion time.</value>
        public virtual DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAudited" /> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId">Type of the UserId</typeparam>
    [Serializable]
    public abstract class GenericFullAuditedEntity<TPrimaryKey, TUserId> : GenericAuditedEntity<TPrimaryKey, TUserId>,
        IGenericFullAudited<TUserId>
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        /// <value>The deleter user identifier.</value>
        public virtual TUserId DeletedUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        /// <value>The deletion time.</value>
        public virtual DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}" /> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey, TUser>, IFullAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Reference to the deleter user of this entity.
        /// </summary>
        /// <value>The deleter user.</value>
        [ForeignKey("DeleterUserId")]
        public virtual TUser DeletedUser { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        /// <value>The deleter user identifier.</value>
        public virtual long? DeletedUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        /// <value>The deletion time.</value>
        public virtual DateTime? DeletionTime { get; set; }
    }
}