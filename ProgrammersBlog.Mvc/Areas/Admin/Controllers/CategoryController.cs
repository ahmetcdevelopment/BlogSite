using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllByNonDeleted();
            return View(result.Data);

        }
        [HttpGet]
        public IActionResult Add()//parçalı view döneceğimiz için asenkron olmasına gerek yok
        {
            return PartialView("_CategoryAddPartial");
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)//parçalı view döneceğimiz için asenkron olmasına gerek yok
        {
            if (ModelState.IsValid)//model içinde bilgiler doğru olarak gelmiş mi
            {
                var result = await _categoryService.Add(categoryAddDto, "Ahmet Çiftçi");
                if(result.ResultStatus == ResultStatus.Success)
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto=result.Data,
                        CategoryAddPartial= await this.RenderViewToStringAsync("_CategoryAddPartial",categoryAddDto)//string parse edip verme
                    });
                    return Json(categoryAddAjaxModel);//javascriptin anlayabilmesi için
                }
            }
            var categoryAddAjaxErorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                //partial içindeki inputlarla ilgili hata mesajları
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)//string parse edip verme
            });
            return Json(categoryAddAjaxErorModel);
        }

        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllByNonDeleted();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryService.Delete(categoryId, "Ahmet Çiftçi");
            var deletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(deletedCategory);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int categoryId)//parçalı view döneceğimiz için asenkron olmasına gerek yok
        {
            var result = await _categoryService.GetCategoryUpdateDto(categoryId);
            if (result.ResultStatus==ResultStatus.Success)
            {
                return PartialView("_CategoryUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)//parçalı view döneceğimiz için asenkron olmasına gerek yok
        {
            if (ModelState.IsValid)//model içinde bilgiler doğru olarak gelmiş mi
            {
                var result = await _categoryService.Update(categoryUpdateDto, "Ahmet Çiftçi");
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryUpdateAjaxModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)//string parse edip verme
                    });
                    return Json(categoryUpdateAjaxModel);//javascriptin anlayabilmesi için
                }
            }
            var categoryUpdateAjaxErorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
            {
                CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)//string parse edip verme
            });
            return Json(categoryUpdateAjaxErorModel);
        }
    }
}
