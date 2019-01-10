using System.Collections.Generic;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.Core.Services
{
    public interface IGenericService<TDto> where TDto : DataTransferObject
    {
        Task<IEnumerable<TDto>> GetAllAsync();

        Task<TDto> GetAsync(int id);

        Task<TDto> AddAsync(TDto model);

        Task<TDto> UpdateAsync(TDto model);

        Task DeleteAsync(int id);
    }
}