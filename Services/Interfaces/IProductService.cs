using Steady_Management_App.DTOs;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllAsync();
    Task<ProductDTO?> GetByIdAsync(int id);
    Task<ProductDTO> CreateAsync(ProductCreateDto dto);
    Task<ProductDTO?> UpdateAsync(ProductUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
