// ViewModels/ProductListViewModel.cs
using System.Collections.ObjectModel;
using System.Windows.Input;
using Steady_Management_App.DTOs;

public class ProductListViewModel : BaseViewModel
{
    private readonly IProductService _svc;
    public ObservableCollection<ProductDTO> Products { get; }
      = new ObservableCollection<ProductDTO>();

    private ProductDTO? _selected;
    public ProductDTO? SelectedProduct
    {
        get => _selected;
        set { _selected = value; Raise(); }
    }

    public ICommand LoadCommand { get; }
    public ICommand NewCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public ProductListViewModel(IProductService svc)
    {
        _svc = svc;
        LoadCommand = new RelayCommand(async _ => await LoadAsync());
        NewCommand = new RelayCommand(_ => OpenDetail(0));
        EditCommand = new RelayCommand(_ => OpenDetail(SelectedProduct!.ProductId),
                                         _ => SelectedProduct != null);
        DeleteCommand = new RelayCommand(async _ => await DeleteAsync(),
                                         _ => SelectedProduct != null);
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Products.Clear();
        var list = await _svc.GetAllAsync();
        foreach (var p in list)
            Products.Add(p);
    }

    private void OpenDetail(int id)
    {
        // falta
    }

    private async Task DeleteAsync()
    {
        if (SelectedProduct == null) return;
        if (await _svc.DeleteAsync(SelectedProduct.ProductId))
            Products.Remove(SelectedProduct);
    }
}
