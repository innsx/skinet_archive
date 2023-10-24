using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, 
                                IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        // Product
        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var specsToQuery = new ProductsWithTypesAndBrandsSpecification();
            
            var products = await _productRepo.ListAsync(specsToQuery);
            
            var productToReturnDtoListAsAutoMapper = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(productToReturnDtoListAsAutoMapper);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
             var specsToQuery = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepo.GetEntityWithSpec(specsToQuery);

            var productToReturnAsAutoMapper = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(productToReturnAsAutoMapper);
        }

        // productBrand
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrandRepo.ListAllAsync();

            return Ok(productBrands);
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        // {
        //     var productBrand = await _productBrandRepo.GetByIdAsync(id);

        //     return Ok(productBrand);
        // }

        // ProductTypes
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await _productTypeRepo.ListAllAsync();

            return Ok(productTypes);
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ProductType>> GetProductType(int id)
        // {
        //     var ProductType = await _productTypeRepo.GetByIdAsync(id);

        //     return Ok(ProductType);
        // }
    }
}





