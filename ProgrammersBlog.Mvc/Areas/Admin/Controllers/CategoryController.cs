using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System.Text.Json;
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
            var result = await _categoryService.GetAll();
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
    }
}
