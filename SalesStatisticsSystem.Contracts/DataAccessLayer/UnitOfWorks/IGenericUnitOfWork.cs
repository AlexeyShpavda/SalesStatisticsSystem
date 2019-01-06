using System.Collections.Generic;
using System.Threading.Tasks;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface IGenericUnitOfWork<TDto> where TDto :DataTransferObject
    {
        void Add(params TDto[] models);

        void Update(params TDto[] models);

        Task<IEnumerable<TDto>> GetAllAsync();

        TDto GetAsync(int id);
    }
}