using BuildingBlock.Base.Models.Base;

namespace ProductService.Application.Abstractions
{
    public interface IDatabaseSubscription
    {
        void Configure(string connString, string tableName);
    }
}
