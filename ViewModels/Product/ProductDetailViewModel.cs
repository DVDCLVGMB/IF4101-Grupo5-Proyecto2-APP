using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;

namespace Steady_Management_App.ViewModels
{
    public class ProductDetailViewModel : BaseViewModel
    {
        private readonly IProductService _svc;
        private ProductDTO productDTO;

        /// <summary>
        /// El DTO que se enlaza en la vista.
        /// </summary>
        public ProductDTO Product
        {
            get => productDTO;
            private set
            {
                productDTO = value;
                Raise(nameof(Product));
            }
        }

        /// <summary>
        /// Se dispara cuando se va a cerrar el diálogo.
        /// La vista debe suscribirse a este evento y cerrarse.
        /// </summary>
        public event Action? RequestClose;

        /// <summary>
        /// Se dispara después de guardar correctamente, 
        /// con el DTO resultante (nuevo o actualizado).
        /// </summary>
        public event Action<ProductDTO>? ProductSaved;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        /// Si es mayor que cero, carga el producto existente; 
        /// si es 0, prepara uno nuevo para crear.
        public ProductDetailViewModel(IProductService svc, int productId = 0)
        {
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

            Product = new ProductDTO();

            if (productId > 0)
                _ = LoadAsync(productId);
        }

        private async Task LoadAsync(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
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
                    CategoryId = Product.CategoryId,
                    ProductName = Product.ProductName,
                    Price = Product.Price
                };
                result = await _svc.CreateAsync(createDto);
            }
            else
            {
                // Actualizar
                var updateDto = new ProductUpdateDto
                {
                    ProductId = Product.ProductId,
                    CategoryId = Product.CategoryId,
                    ProductName = Product.ProductName,
                    Price = Product.Price
                };
                result = await _svc.UpdateAsync(updateDto);
            }

            if (result != null)
                ProductSaved?.Invoke(result);

            RequestClose?.Invoke();
        }

        private void OnCancel()
        {
            // Simplemente cerramos sin guardar
            RequestClose?.Invoke();
        }
    }
}
