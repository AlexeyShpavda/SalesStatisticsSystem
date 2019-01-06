using System.Collections.Generic;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.Core.Services
{
    public interface IGenericService<TDto> where TDto : DataTransferObject
    {
        Task<IEnumerable<TDto>> GetAllAsync();

        TDto GetAsync(int id);

        void Add(params TDto[] models);

        void Update(params TDto[] models);

        void Delete(params TDto[] models);

        void Delete(int id);
    }
}