using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Steady_Management_App.DTOs;
using Steady_Management_App.Services;

public class ProductListViewModel : BaseViewModel
{
    private readonly IProductService _svc;
    public ObservableCollection<ProductDTO> Products { get; }
      = new ObservableCollection<ProductDTO>();

    private ProductDTO? _selected;
    private ProductService productService;

    public ProductDTO? SelectedProduct
    {
        get => _selected;
        set { _selected = value; Raise(); }
    }

    public ICommand LoadCommand { get; }
    public ICommand NewCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteProductCommand { get; }
    public ICommand DeleteCommand { get; }

    public ProductListViewModel()
    : this(new ProductService())
    { }

    public ProductListViewModel(IProductService svc)
    {
        _svc = svc ?? throw new ArgumentNullException(nameof(svc));
        LoadCommand = new RelayCommand(async _ => await LoadAsync());
        NewCommand = new RelayCommand(_ => OpenDetail(0));
        EditCommand = new RelayCommand(_ => OpenDetail(SelectedProduct!.ProductId),
                                         _ => SelectedProduct != null);
        DeleteCommand = new RelayCommand(async _ => await DeleteAsync(),
                                         _ => SelectedProduct != null);
        DeleteProductCommand = DeleteCommand;
        _ = LoadAsync();
    }

    public ProductListViewModel(ProductService productService)
    {
        this.productService = productService;
    }

    private async Task LoadAsync()
    {
        try
        {
            Products.Clear();
            var list = await _svc.GetProductsAsync();
            foreach (var p in list) Products.Add(p);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar productos: {ex.Message}");
        }
    }


    private void OpenDetail(int id)
    {
        // falta
    }

    private async Task DeleteAsync()
    {
        if (SelectedProduct == null) return;
        if (await _svc.DeleteProductAsync(SelectedProduct.ProductId))
            Products.Remove(SelectedProduct);
    }
}
