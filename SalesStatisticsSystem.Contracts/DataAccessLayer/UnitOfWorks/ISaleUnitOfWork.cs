using System.Collections.Generic;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;

namespace SalesStatisticsSystem.Contracts.DataAccessLayer.UnitOfWorks
{
    public interface ISaleUnitOfWork
    {
        void Add(params SaleDto[] models);

        IEnumerable<SaleDto> GetAll();
    }
}