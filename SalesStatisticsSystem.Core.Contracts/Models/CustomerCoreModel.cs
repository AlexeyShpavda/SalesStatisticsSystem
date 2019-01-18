namespace SalesStatisticsSystem.Core.Contracts.Models
{
    public class CustomerCoreModel
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