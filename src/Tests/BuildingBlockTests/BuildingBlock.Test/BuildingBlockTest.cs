namespace BuildingBlock.Test
{
    public class BuildingBlockTest
    {

        private ServiceCollection services;

        public BuildingBlockTest()
        {
            services = new ServiceCollection();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ElasticServiceTest()
        {
            services.AddSingleton<IElastic<ProductModel>>(sp =>
            {
                return ElasticFactory<ProductModel>.CreateForClass(new() { Connection = "http://localhost:9200", SearchBaseType = Base.Enums.SearchType.ElasticSearch }, sp);
            });

            var sp = services.BuildServiceProvider();

            var elasticLibrary = sp.GetRequiredService<IElastic<ProductModel>>();

            await elasticLibrary.CreateIndexAsync("ProdutcModel");

            await elasticLibrary.InsertAsync(new() { Id = Guid.NewGuid(), Name = "Apple", Price = 12.2 });
            await elasticLibrary.InsertAsync(new() { Id = Guid.NewGuid(), Name = "Plum", Price = 13.3 });
            await elasticLibrary.InsertAsync(new() { Id = Guid.NewGuid(), Name = "Peach", Price = 14.4 });
            await elasticLibrary.InsertAsync(new() { Id = Guid.NewGuid(), Name = "Apricot", Price = 15.5 });

            var productModelList = await elasticLibrary.GetAllAsync();

            foreach (var productModel in productModelList.ToList())
                await Console.Out.WriteLineAsync($"Product Name : [" + productModel.Name + "] and Price : [" + productModel.Price + "]");
        }

        [Test]
        public async Task ElasticServiceTest2()
        {
            services.AddSingleton<ICompleteService<ProductModel>>(sp =>
            {
                return ElasticFactory<ProductModel>.CreateForComplete(new() { Connection = "http://localhost:9200", SearchBaseType = Base.Enums.SearchType.ElasticSearch }, sp);
            });

            var sp = services.BuildServiceProvider();

            var elasticLibrary = sp.GetRequiredService<ICompleteService<ProductModel>>();

            var elasticDatas = await elasticLibrary.AutoAnalyzeWithLike("Name", "a");

            foreach (var elasticData in elasticDatas)
                await Console.Out.WriteLineAsync("Data : " + elasticData.Name + " - " + elasticData.Price);
        }
    }
}