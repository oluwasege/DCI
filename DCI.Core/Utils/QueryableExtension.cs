﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCI.Core.Utils
{
        public static class QueryableExtensions
        {
            public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize, int total)
            {
                var list = query.Paginate(pageIndex, pageSize).ToList();
                return new PaginatedList<T>(list, pageIndex, pageSize, total);
            }

            public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize)
            {
                var entities = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return entities;
            }

            public static PaginatedList<T> ToPaginatedList<T>(this IList<T> list, int pageIndex, int pageSize, int total)
            {
                return new PaginatedList<T>(list, pageIndex, pageSize, total);
            }
            public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
            {
                int total = query.Count();
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return new PaginatedList<T>(list, pageIndex, pageSize, total);
            }
        }
}
