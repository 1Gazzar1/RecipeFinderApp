namespace RecipeFinderApp.Models.DTOs
{
	public class RecipeDTO
	{
		public string? Name { get; set; }
		public double Calories { get; set; }
		public int Cookingtime { get; set; }
		public string? Category { get; set; }
		public List<string>? Ingredients { get; set; }
		public RecipeSortingOptions SortBy { get; set; }
		public bool Asc { get; set; }
	}
}