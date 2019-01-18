namespace SalesStatisticsSystem.Core.Contracts.Models
{
    public class ManagerCoreModel
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