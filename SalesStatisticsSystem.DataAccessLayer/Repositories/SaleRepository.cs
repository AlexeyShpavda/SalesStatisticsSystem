using AutoMapper;
using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.DataAccessLayer.Contracts.Repository;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class SaleRepository : GenericRepository<SaleCoreModel, Sale>, ISaleRepository
    {
        public SaleRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}