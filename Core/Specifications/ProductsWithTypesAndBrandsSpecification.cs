using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // parameter-less constructor
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
                    : base(x => 
                            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&  //Search
                            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&  // Filtering
                            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) 
                        )//execute in Base class
        {
            // executing Lazy loading
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);

            // executing sorting by Product Name
            AddOrderBy(x => x.Name);

            // executing Pagination
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            // executing Filtering by Product Price
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }

        //this is the Criteria "base(x => x.Id == id)" passed to the baseSpecificaiton.cs to be executed
        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)  
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}