using System.Collections.Generic;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface IGenericUnitOfWork<TDto> where TDto :DataTransferObject
    {
        Task<TDto> AddAsync(TDto model);

        Task<TDto> UpdateAsync(TDto model);

        void Delete(int id);

        Task<IEnumerable<TDto>> GetAllAsync();

        TDto GetAsync(int id);
    }
}