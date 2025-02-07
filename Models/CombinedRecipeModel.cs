namespace RecipeFinderApp.Models
{
	public class CombinedRecipeModel
	{
		public List<Recipe>? Recipes { get; set; }

		public RecipeDTO? RecipeDTO { get; set; }
		public string? Ingredient { get; set; }
	}
}
