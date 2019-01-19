using SalesStatisticsSystem.Core.Contracts.Models.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models
{
    public class ProductCoreModel : CoreModel
    {
        public string Name { get; set; }

        public ProductCoreModel()
        {
        }

        public ProductCoreModel(string name)
        {
            Name = name;
        }
    }
}