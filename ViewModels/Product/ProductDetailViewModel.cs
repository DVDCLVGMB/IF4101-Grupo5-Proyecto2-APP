using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Steady_Management_App.DTOs;
using Steady_Management_App.Models;
using Steady_Management_App.Services;

namespace Steady_Management_App.ViewModels
{
    public class ProductDetailViewModel : BaseViewModel
    {
        private readonly IProductService _productSvc;
        private readonly ICategoryService _categorySvc;

        private ProductDTO _product = new ProductDTO();
        public ProductDTO Product
        {
            get => _product;
            private set
            {
                _product = value;
                Raise(nameof(Product));
            }
        }

        public ObservableCollection<Category> Categories { get; }
            = new ObservableCollection<Category>();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? RequestClose;
        public event Action<ProductDTO>? ProductSaved;

        public ProductDetailViewModel(
            IProductService productSvc,
            ICategoryService categorySvc,
            int productId = 0)
        {
            _productSvc = productSvc ?? throw new ArgumentNullException(nameof(productSvc));
            _categorySvc = categorySvc ?? throw new ArgumentNullException(nameof(categorySvc));

            // Mantengo comandos siempre habilitados
            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ => OnCancel());

            // Cargo categorías para el ComboBox
            _ = LoadCategoriesAsync();

            // Si viene un ID, cargo el producto; si no, dejo Product con valores por defecto
            if (productId > 0)
                _ = LoadProductAsync(productId);
        }

        private async Task LoadCategoriesAsync()
        {
            var list = await _categorySvc.GetCategoriesAsync();
            Categories.Clear();
            foreach (var c in list)
                Categories.Add(c);
        }

        private async Task LoadProductAsync(int id)
        {
            var dto = await _productSvc.GetProductByIdAsync(id);
            if (dto != null)
                Product = dto;
        }

        private async Task SaveAsync()
        {
            ProductDTO? result;
            if (Product.ProductId == 0)
            {
                // Crear
                var createDto = new ProductCreateDto
                {
                    ProductName = Product.ProductName,
                    Price = Product.Price,
                    CategoryId = Product.CategoryId
                };
                result = await _productSvc.AddProductAsync(
                    createDto.ProductName,
                    createDto.CategoryId,
                    createDto.Price
                );
            }
            else
            {
                // Actualizar
                var updateDto = new ProductUpdateDto
                {
                    ProductId = Product.ProductId,
                    ProductName = Product.ProductName,
                    Price = Product.Price,
                    CategoryId = Product.CategoryId
                };
                var ok = await _productSvc.UpdateProductAsync(
                    new Models.Product
                    {
                        ProductId = updateDto.ProductId,
                        ProductName = updateDto.ProductName,
                        Price = updateDto.Price,
                        CategoryId = updateDto.CategoryId
                    });
                result = ok ? Product : null;
            }

            if (result != null)
                ProductSaved?.Invoke(result);

            RequestClose?.Invoke();
        }

        private void OnCancel()
        {
            RequestClose?.Invoke();
        }
    }
}
