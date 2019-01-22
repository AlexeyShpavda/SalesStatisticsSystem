using SalesStatisticsSystem.Core.Contracts.Models.Sales.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Sales
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