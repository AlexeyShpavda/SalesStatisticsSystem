using SalesStatisticsSystem.Core.Contracts.Models;
using SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter.Generic;

namespace SalesStatisticsSystem.DataAccessLayer.Contracts.ReaderWriter
{
    public interface IManagerDbReaderWriter : IGenericDbReaderWriter<ManagerCoreModel>
    {
    }
}