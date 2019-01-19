using SalesStatisticsSystem.Core.Contracts.Models.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models
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