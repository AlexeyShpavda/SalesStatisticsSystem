using SalesStatisticsSystem.Core.Contracts.Models.Sales.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Sales
{
    public class CustomerCoreModel : CoreModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public CustomerCoreModel()
        {
        }

        public CustomerCoreModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}