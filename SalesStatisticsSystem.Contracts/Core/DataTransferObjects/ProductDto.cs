using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.Core.DataTransferObjects
{
    public class ProductDto : DataTransferObject
    {
        public string Name { get; set; }

        public ProductDto()
        {
        }

        public ProductDto(string name)
        {
            Name = name;
        }
    }
}