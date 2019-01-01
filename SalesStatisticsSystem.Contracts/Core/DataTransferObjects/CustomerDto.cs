using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.Core.DataTransferObjects
{
    public class CustomerDto : DataTransferObject
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public CustomerDto()
        {
        }

        public CustomerDto(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}