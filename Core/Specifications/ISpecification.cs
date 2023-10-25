using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria {get;}
        List<Expression<Func<T, object>>> Includes { get; }

        //FILTERINGS SPECIFICATIONS
        Expression<Func<T, object>> OrderBy {get;}
        Expression<Func<T, object>> OrderByDescending {get;}


        //PAGINATION SPECIFICATIONS
        int Take {get;}
        int Skip {get;}
        bool IsPagingEnabled {get;}
    }
}