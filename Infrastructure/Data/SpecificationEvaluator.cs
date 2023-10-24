using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specifications)
        {
            var query = inputQuery;
            
            if (specifications.Criteria != null)
            {
                query = query.Where(specifications.Criteria);   //Criteria can be anything like "p => p.ProductTypeId == id"
            }

            query = specifications.Includes.Aggregate(query, 
                                                      (current, include) => current.Include(include)
                                                    );

            return query;
        
        }
    }
}