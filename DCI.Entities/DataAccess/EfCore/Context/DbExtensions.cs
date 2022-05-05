// ***********************************************************************
// Assembly         : FSDH.Core
// Author           : SBSC
// Created          : 02-15-2021
//
// Last Modified By : SBSC
// Last Modified On : 02-15-2021
// ***********************************************************************
// <copyright file="DbExtensions.cs" company="FSDH.Core">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FSDH.Entities.Common;

namespace FSDH.Core.DataAccess.EfCore.Context
{
    /// <summary>
    /// Class DbExtensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DbExtensions
    {
        /// <summary>
        /// Determines whether [is database fk delete exception] [the specified foreign key error message].
        /// </summary>
        /// <param name="updateEx">The update ex.</param>
        /// <param name="foreignKeyErrorMessage">The foreign key error message.</param>
        /// <returns><c>true</c> if [is database fk delete exception] [the specified foreign key error message]; otherwise, <c>false</c>.</returns>
        public static bool IsDatabaseFkDeleteException(this DbUpdateException updateEx,
            out string foreignKeyErrorMessage)
        {
            foreignKeyErrorMessage = null;

            if (updateEx == null || updateEx.Entries.All(e => e.State != EntityState.Deleted))
                return false;

            var exception = (updateEx.InnerException ?? updateEx.InnerException?.InnerException) as SqlException;
            var errors = exception?.Errors.Cast<SqlError>();

            var errorMessages = new StringBuilder();

            if (errors != null)
                foreach (var exceptionError in errors.Where(e => e.Number == 547))
                {
                    errorMessages.AppendLine($"Message: {exceptionError.Message}");
                    errorMessages.AppendLine($"ErrorNumber: {exceptionError.Number}");
                    errorMessages.AppendLine($"LineNumber: {exceptionError.LineNumber}");
                    errorMessages.AppendLine($"Source: {exceptionError.Source}");
                    errorMessages.AppendLine($"Procedure: {exceptionError.Procedure}");
                }

            if (errorMessages.Length == 0) return false;

            foreignKeyErrorMessage = errorMessages.ToString();

            return true;
        }

        /// <summary>
        /// Determines whether [is update concurrency exception] [the specified properties].
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="properties">The properties.</param>
        /// <returns><c>true</c> if [is update concurrency exception] [the specified properties]; otherwise, <c>false</c>.</returns>
        /// <exception cref="NotSupportedException">Don't know how to handle concurrency conflicts for "
        ///                         + entry.Metadata.Name</exception>
        public static bool IsUpdateConcurrencyException(this DbUpdateException ex, out string properties)
        {
            properties = null;

            if (ex == null || ex.Entries.All(e => e.State != EntityState.Modified))
                return false;

            var errorMessages = new StringBuilder();

            foreach (var entry in ex.Entries)
                if (entry.Entity is IEntity)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        errorMessages.AppendLine($"Entity: {entry.Metadata.Name}\tOld Value:" +
                                                 $" {databaseValue}\tNew Value{proposedValue}");
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }

            if (errorMessages.Length == 0) return false;

            properties = errorMessages.ToString();
            return true;
        }
    }
}