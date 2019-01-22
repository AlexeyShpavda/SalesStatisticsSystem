using SalesStatisticsSystem.Core.Contracts.Models.Sales.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Sales
{
    public class ManagerCoreModel : CoreModel
    {
        public string LastName { get; set; }

        public ManagerCoreModel()
        {
        }

        public ManagerCoreModel(string lastName)
        {
            LastName = lastName;
        }
    }
}