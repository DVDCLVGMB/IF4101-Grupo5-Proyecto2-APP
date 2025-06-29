using System.Collections.Generic;
using System.Threading.Tasks;
using Steady_Management_App.Models;

namespace Steady_Management_App.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
    }
}