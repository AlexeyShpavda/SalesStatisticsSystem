using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class SaleRepository : GenericRepository<SaleDto, Sale>, ISaleRepository
    {
        public SaleRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}