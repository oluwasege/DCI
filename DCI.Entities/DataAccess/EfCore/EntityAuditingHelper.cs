// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="EntityAuditingHelper.cs" company="SBSC">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Diagnostics.CodeAnalysis;
using FSDH.Core.Timing;
using FSDH.Entities.Auditing;
using FSDH.Entities.Extensions;

namespace FSDH.Core.DataAccess.EfCore
{
    /// <summary>
    /// Class EntityAuditingHelper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EntityAuditingHelper
    {
        /// <summary>
        /// Sets the creation audit properties.
        /// </summary>
        /// <param name="entityAsObj">The entity as object.</param>
        /// <param name="userId">The user identifier.</param>
        public static void SetCreationAuditProperties(
            object entityAsObj,
            long? userId)
        {
            if (!(entityAsObj is IHasCreationTime entityWithCreationTime))
                //Object does not implement IHasCreationTime
                return;

            if (entityWithCreationTime.CreationTime == default) entityWithCreationTime.CreationTime = Clock.Now;

            if (!(entityAsObj is ICreationAudited))
                //Object does not implement ICreationAudited
                return;

            if (!userId.HasValue)
                //Unknown user
                return;

            var entity = entityAsObj as ICreationAudited;
            if (entity.CreatorUserId != null)
                //CreatorUserId is already set
                return;

            //Finally, set CreatorUserId!
            entity.CreatorUserId = userId;
        }

        /// <summary>
        /// Sets the modification audit properties.
        /// </summary>
        /// <param name="entityAsObj">The entity as object.</param>
        /// <param name="userId">The user identifier.</param>
        public static void SetModificationAuditProperties(
            object entityAsObj,
            long? userId)
        {
            if (entityAsObj is IHasModificationTime)
                entityAsObj.As<IHasModificationTime>().LastModificationTime = Clock.Now;

            if (!(entityAsObj is IModificationAudited))
                //Entity does not implement IModificationAudited
                return;

            var entity = entityAsObj.As<IModificationAudited>();

            if (userId == null)
            {
                //Unknown user
                entity.LastModifierUserId = null;
                return;
            }

            //Finally, set LastModifierUserId!
            entity.LastModifierUserId = userId;
        }
    }
}