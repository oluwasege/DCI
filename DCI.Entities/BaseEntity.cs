//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace DCI.Entities
//{
//    public interface IEntity
//    {
//        Guid Id { get; set; }
//    }

//    public interface IEntityState
//    {
//        bool IsActive { get; set; }
//    }
//    public interface IDateAudit
//    {
//        DateTime CreatedOn { get; set; }
//        DateTime? ModifiedOn { get; set; }
//    }

//    public interface IActorAudit
//    {
//        Guid? CreatedBy { get; set; }
//        Guid? ModifiedBy { get; set; }
//    }

//    //public interface ISoftDelete
//    //{
//    //    bool IsDeleted { get; set; }
//    //    DateTime? DeletedOn { get; set; }
//    //}

//    //public interface IDeleteActorAudit : ISoftDelete
//    //{
//    //    Guid? DeletedBy { get; set; }
//    //}

//    public interface IAudit : IDateAudit, IActorAudit
//    {
//    }

//    //public interface IFullAudit : IAudit, IDeleteActorAudit
//    //{
//    //}

//    public abstract class Entity : IEntity
//    {
//        public virtual Guid Id { get; set; }
//    }

//    public abstract class AuditedEntity : Entity, IAudit
//    {
//        public AuditedEntity()
//        {
//            Id = Guid.NewGuid();
//            CreatedOn = DateTime.Now;
//        }

//        public DateTime CreatedOn { get; set; }
//        public DateTime? ModifiedOn { get; set; }

//        public Guid? CreatedBy { get; set; }
//        public Guid? ModifiedBy { get; set; }
//    }

//    public abstract class BaseEntity : AuditedEntity, IFullAudit, ISoftDelete, IEntityState
//    {
//        public BaseEntity()
//        {
//            Id = Guid.NewGuid();
//            CreatedOn = DateTime.Now;
//        }
//        public Guid? DeletedBy { get; set; }
//        public bool IsDeleted { get; set; }
//        public bool IsActive { get; set; }
//        public DateTime? DeletedOn { get; set; }
//    }
//}
