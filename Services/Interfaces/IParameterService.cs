using Steady_Management_App.Models;
using System.Threading.Tasks;

namespace Steady_Management_App.Services.Interfaces
{
    public interface IParameterService
    {
        Task<Parameter> GetParameterAsync();
        Task<bool> UpdateParameterAsync(Parameter parameter);
    }
}