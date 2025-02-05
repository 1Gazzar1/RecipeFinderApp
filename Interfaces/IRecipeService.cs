namespace RecipeFinderApp.Interfaces
{
	public interface IRecipeService
	{
		Task<List<Recipe>> GetAllRecipes();
		Task<Recipe> GetRecipeById(ObjectId Id);
		Task AddRecipe(Recipe recipe);
		Task UpdateRecipe(ObjectId Id, Recipe recipe);
		Task DeleteRecipe(ObjectId Id);
		Task<List<Recipe>> Filter(string name, List<string> ingredients,
			string category, double calories, int cooking_time,
			RecipeSortingOptions sortBy = RecipeSortingOptions.Name, bool asc = true);
		Task<string> EncodeBase64toByte(string ImgBase64);
	}
}
