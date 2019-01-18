namespace SalesStatisticsSystem.Core.Contracts.Models
{
    public class ProductCoreModel
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