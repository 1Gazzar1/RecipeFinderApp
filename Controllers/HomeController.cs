using System.Reflection.Metadata;

namespace RecipeFinderApp.Controllers
{
	public class HomeController : Controller
    {
		private readonly IRecipeService _recipeService;
		public HomeController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		// the api requests 

		/*
		[HttpGet]
		public async Task<IActionResult> GetAllRecipe()
		{
			var recipes = await _recipeService.GetAllRecipes();
			if (recipes == null || recipes.Count == 0)
			{
				return NotFound("No recipes Found");
			}
			return Ok(recipes);
		}
		[HttpGet("{Id}")]
		public async Task<IActionResult> GetAllRecipe(string Id)
		{
			var objectId = ObjectId.Parse(Id);
			var recipe = await _recipeService.GetRecipeById(objectId);
			if (recipe == null)
			{
				return NotFound("No recipe Found");
			}
			return Ok(recipe);
		}
		[HttpPost]
		public async Task<IActionResult> AddRecipe(Recipe recipe)
		{
			if (recipe == null)
			{
				return BadRequest("Invalid Format");
			}
			await _recipeService.AddRecipe(recipe);
			return Created();
		}
		[HttpPut]
		public async Task<IActionResult> UpdateRecipe(string Id, Recipe recipe)
		{
			if (recipe == null)
			{
				return BadRequest("Invalid Format");
			}
			var objectId = ObjectId.Parse(Id);
			await _recipeService.UpdateRecipe(objectId, recipe);
			return NoContent();
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteRecipe(string Id)
		{

			var objectId = ObjectId.Parse(Id);
			await _recipeService.DeleteRecipe(objectId);
			return NoContent();
		}
		*/
		[HttpPost("Filter&Sort")]
		public async Task<IActionResult> FilterRecipes(CombinedRecipeModel ViewModel)
		{
			var recipes = await _recipeService.Filter(ViewModel.RecipeDTO.Name,
										ViewModel.RecipeDTO.Ingredients,
										ViewModel.RecipeDTO.Category,
										ViewModel.RecipeDTO.Calories,
										ViewModel.RecipeDTO.Cookingtime,
										ViewModel.RecipeDTO.SortBy,
										ViewModel.RecipeDTO.Asc);
			ViewModel.Recipes = recipes;

			return View("Index", ViewModel);
		}
		[HttpPost("Ingredient")]
		public  IActionResult AddIngredients(CombinedRecipeModel ViewModel)
		{
			if (ViewModel.RecipeDTO is null || ViewModel.RecipeDTO.Ingredients is null)
			{
				ViewModel.RecipeDTO = new RecipeDTO();
				ViewModel.RecipeDTO.Ingredients = [];
			}
			var ing = ViewModel.Ingredient ?? "";
			ViewModel.RecipeDTO.Ingredients.Add(ing);
			
			return View("Index", ViewModel);
			
		}
		public IActionResult DeleteIngredients(CombinedRecipeModel ViewModel)
		{
			if (ViewModel.RecipeDTO is null || ViewModel.RecipeDTO.Ingredients is null)
			{
				ViewModel.RecipeDTO = new RecipeDTO();
				ViewModel.RecipeDTO.Ingredients = [];
			}

			return View("Index",ViewModel);
		}


		// view methods 

		public IActionResult Index()
        {

			var combinedRecipeModel = new CombinedRecipeModel()
			{
				Recipes = [],
				RecipeDTO = new RecipeDTO()
				{
					Ingredients = []	
				}
			};
		
            return View(combinedRecipeModel);
        }
    }
}
