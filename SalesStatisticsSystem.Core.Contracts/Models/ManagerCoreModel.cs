using SalesStatisticsSystem.Core.Contracts.Models.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models
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