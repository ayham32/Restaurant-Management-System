
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.ProductsDTOs;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using RestaurantShared.Results;
using RestuarantDataAccess.UintOfWork;

namespace RestaurantBusiness.Services
{
    public class ProductsService: IProductsService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices _fileServices;


        public ProductsService(IUnitOfWork unitOfWork, IFileServices fileServices)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
        }

        public async Task<Result<int>> AddNewProduct(CreateProductDto newProduct)
        {
            
            string ImageUrl = null;
            if (newProduct.Image != null)
            {
                ImageUrl = await _fileServices.SaveFileAsync(newProduct.Image, "products");
            }


            var Product = new Product
            {
                CategoryId = newProduct.CategoryId,
                Name = newProduct.Name,
                ImageUrl = ImageUrl,
                Quantity = newProduct.Quantity,
                Price = newProduct.Price,
                PreparationTime = newProduct.PreparationTime,
                IsAvailable =(bool) newProduct.IsAvailable,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.productRepository.AddAsync(Product);

            return await _unitOfWork.SaveChangesAsync() >0?Result<int>.Success(Product.ProductId):Result<int>.Failure("Something is wrong");

        }

        public async Task<Result> DeleteProduct(int ProductId)
        {
            if (ProductId <= 0)
                return Result.Failure("Id must be a postive");

            var Product = await _unitOfWork.productRepository.GetByIdAsync(ProductId);

            if (Product == null)
                return Result.Failure("No product with this id");

            _unitOfWork.productRepository.DeleteAsync(Product);

            return await _unitOfWork.SaveChangesAsync() >0? Result.Success():Result.Failure("Something is wrong");
        }

        public async Task<Result<ReadProductsDto>>? GetProductAsync(int ProductId)
        {
            if (ProductId <= 0)
                return Result<ReadProductsDto>.Failure("id must be a postive");

            var Product = await _unitOfWork.productRepository.GetProductAsync(ProductId);

            if (Product == null)
                return Result<ReadProductsDto>.Failure("No product with this id");

            var ProductDto = new ReadProductsDto
            {
                ProductId = ProductId,
                PreparationTime = Product.PreparationTime,
                IsAvailable = Product.IsAvailable,
                CategoryName = Product.Category.CategoryName,
                CreatedAt = Product.CreatedAt,
                ImageUrl = Product.ImageUrl == null ? "" : Product.ImageUrl,
                Name = Product.Name,
                Price = Product.Price,
                Quantity = Product.Quantity
            };

            return Result<ReadProductsDto>.Success(ProductDto);
        }

        public async Task<Result<PagedResult<ReadProductsDto>>> GetProductsAsync(ProductParameter parameter)
        {
            var Products = await _unitOfWork.productRepository.GetProductsAsync(parameter);

            if (Products == null)
                return Result<PagedResult<ReadProductsDto>>.Failure("There are no products");

            var ProductsDto = Products.Items.Select(p => new ReadProductsDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                CategoryName = p.Category.CategoryName,
                Price = p.Price,
                PreparationTime = p.PreparationTime,
                ImageUrl = p.ImageUrl == null ? "" : p.ImageUrl,
                CreatedAt = p.CreatedAt,
                IsAvailable = p.IsAvailable,
                Quantity = p.Quantity
            }).ToList();


            var pagedResult = new PagedResult<ReadProductsDto>
            {
                Items = ProductsDto,
                TotalCount = Products.TotalCount,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize 
            };

            return Result<PagedResult<ReadProductsDto>>.Success(pagedResult);

        }

        public async Task<bool> isProductExist(int ProductId)
        {
            if (ProductId <= 0)
                return false;

            return await _unitOfWork.productRepository.isExistAsync(ProductId);
        }

        public async Task<Result> UpdateProduct(int ProductId, UpdateProductDto product)
        {
            if (ProductId <= 0)
                return Result.Failure("Id must be a postive");

            var ProductToUpdate = await _unitOfWork.productRepository.GetByIdAsync(ProductId);



            if(ProductToUpdate == null) 
                return Result.Failure("No product with this id");

            string ImageUrl = "";
            if (product.Image != null)
            {

                if (!string.IsNullOrEmpty(ProductToUpdate.ImageUrl))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        ProductToUpdate.ImageUrl.TrimStart('/')
                    );

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                ImageUrl = await _fileServices.SaveFileAsync(product.Image, "products");

            }

            ProductToUpdate.Name = product.Name;
            ProductToUpdate.CategoryId = product.CategoryId;
            ProductToUpdate.Quantity = product.Quantity;
            ProductToUpdate.Price= product.Price;
            ProductToUpdate.ImageUrl= ImageUrl=="" ? null:ImageUrl;
            ProductToUpdate.IsAvailable = product.IsAvailable;
            ProductToUpdate.PreparationTime= product.PreparationTime;
            ProductToUpdate.Quantity= product.Quantity;

            return await _unitOfWork.SaveChangesAsync() >0?Result.Success():Result.Failure("Something is wrong");
        
        }
    }
}
