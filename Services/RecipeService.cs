namespace RecipeFinderApp.Services
{
	public class RecipeService : IRecipeService
    {
        private readonly AppDBContext _db;
        public RecipeService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            var recipes = await _db.Recipes.Find(_ => true).ToListAsync();
            return recipes;
        }
        public async Task<Recipe> GetRecipeById(ObjectId Id)
        {
            var recipe = await _db.Recipes
                            .Find(R => R.Id == Id).FirstOrDefaultAsync();
            return recipe;
        }
        public async Task AddRecipe(Recipe recipe)
        {
            await _db.Recipes.InsertOneAsync(recipe);
        }
        public async Task UpdateRecipe(ObjectId Id, Recipe recipe)
        {
            var old_recipe = await GetRecipeById(Id);
            var new_recipe = recipe; 
            new_recipe.Id = old_recipe.Id;

            await _db.Recipes.ReplaceOneAsync(r => r.Id == Id, new_recipe);
        }
        public async Task DeleteRecipe(ObjectId Id)
        {
            await _db.Recipes.DeleteOneAsync(r => r.Id == Id);
        }
        public async Task<List<Recipe>> Filter(string name, List<string> ingredients,
            string category, double calories, int cooking_time ,
            RecipeSortingOptions sortBy = RecipeSortingOptions.Name, bool asc = true )
        {
            // Validations 

            cooking_time = cooking_time == 0 ?  int.MaxValue : cooking_time;
            calories = calories == 0 ?  double.MaxValue : calories ;
			ingredients = ingredients is null ?  [] : ingredients;
            name = name is null ? "" : name;
			category = category is null ? "" : category;

			// Filter
			var filter_builder = Builders<Recipe>.Filter;

            var filter =
                            filter_builder.Regex(r => r.Name, new BsonRegularExpression(name, "i")) &
                            filter_builder.Lte(r => r.Calories, calories) &
                            filter_builder.Lte(r => r.Cookingtime, cooking_time) &
                            (category == ""? filter_builder.Empty : filter_builder.Eq(r => r.Category, category)  );
            // if there is no ingredients input ignore filtering the ingredients
            if (ingredients.Count > 0)
            {
				filter = filter & (filter_builder.ElemMatch(r => r.Ingredients, i => ingredients.Contains(i!.Name!)));
			}

            //Sort
            SortDefinition<Recipe>? sort;
            var sort_builder = Builders<Recipe>.Sort;
            
            if (sortBy == RecipeSortingOptions.Calories)
            {
                sort = asc == true ? sort_builder.Ascending(r => r.Calories) : sort_builder.Descending(r => r.Calories);
            }
            else if (sortBy == RecipeSortingOptions.Cookingtime)
            {
                sort = asc == true ? sort_builder.Ascending(r => r.Cookingtime) : sort_builder.Descending(r => r.Cookingtime);
            }
            else if (sortBy == RecipeSortingOptions.IngredientsCount)
            {
				// this is done in a different way because monogodb hates me :(
				// this doesn't work ( sort_builder.Ascending(r => r.Ingredients.Count) )
                // it has something to do with .Count

				var recipes =  await _db.Recipes.Find(filter).ToListAsync();
                return asc == true ? recipes.OrderBy(r => r.Ingredients!.Count).ToList() :
                                    recipes.OrderByDescending(r => r.Ingredients!.Count).ToList();
                                       
            }
            // sort by name if nothing else or wrong input (somehow)
            else
            {
				sort = asc == true ? sort_builder.Ascending(r => r.Name) : sort_builder.Descending(r => r.Name);
			}

			return await _db.Recipes.Find(filter).Sort(sort).ToListAsync();
        }
	}
}
