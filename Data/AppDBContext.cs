namespace RecipeFinderApp.Data
{
	public class AppDBContext
	{
		private IMongoDatabase? _db { get; set; }

		public AppDBContext(IOptions<MongoDBSettings> options)
		{
			var client = new MongoClient(options.Value.ConnectionString); // Use the connection string from options
			_db = client.GetDatabase(options.Value.DatabaseName); // Assign to the class field, not a local variable
			Console.WriteLine("Connected to MongoDB.");

		}

		public IMongoCollection<Recipe> Recipes => _db!.GetCollection<Recipe>("Recipes");

	}
}
