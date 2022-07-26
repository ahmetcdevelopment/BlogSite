﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService=articleService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var result = await _articleService.GetAllByNonDeleted();
            //if (result.ResultStatus == ResultStatus.Success)
            //{
            //    return View(result.Data);
            //}
            ////return View(result.Data);
            //return NotFound();
            var result = await _articleService.GetAllByNonDeleted();
            return View(result.Data);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        public async Task<JsonResult> GetAll()
        {
            var articles = await _articleService.GetAllByNonDeleted();
            var result = articles.Data.Articles.ToList();
            //var result = _context.Categories.ToList<Category>();
            return Json(new { rows = result, page = 1 }, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
