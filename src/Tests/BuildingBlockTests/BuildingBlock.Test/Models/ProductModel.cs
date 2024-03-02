using BuildingBlock.Base.Models.Base;

namespace BuildingBlock.Test.Models
{
    public class ProductModel : ElasticModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
