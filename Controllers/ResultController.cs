using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Data.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        private ApplicationDbContext DbContext;

        public ResultController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        //[HttpGet("All/{quizId}")]
        //public IActionResult All(int quizId)
        //{
        //    var sampleResults = new List<ResultViewModel>();

        //    sampleResults.Add(new ResultViewModel()
        //    {
        //        Id = 1,
        //        QuizId = quizId,
        //        Text = "What do you value most in your life?",
        //        CreatedDate = DateTime.Now,
        //        LastModifiedDate = DateTime.Now
        //    });

        //    for (int i = 2; i <= 5; i++)
        //    {
        //        sampleResults.Add(new ResultViewModel()
        //        {
        //            Id = i,
        //            QuizId = quizId,
        //            Text = $"Sample Question {i}",
        //            CreatedDate = DateTime.Now,
        //            LastModifiedDate = DateTime.Now
        //        });
        //    }

        //    return new JsonResult(
        //        sampleResults,
        //        new Newtonsoft.Json.JsonSerializerSettings()
        //        {
        //            Formatting = Newtonsoft.Json.Formatting.Indented
        //        });
        //}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = DbContext.Results
                            .Where(i => i.Id == id)
                            .FirstOrDefault();

            if (result == null)
            {
                return NotFound(new
                {
                    Error = $"Result ID {id} has not been found"
                });
            }

            return new JsonResult
            (result.Adapt<ResultViewModel>(),
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
        }

        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var result = model.Adapt<Result>();

            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;

            DbContext.Results.Add(result);
            DbContext.SaveChanges();

            return new JsonResult(
                result.Adapt<ResultViewModel>(),
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
        }

        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var result = DbContext.Results
                .Where(q => q.Id == model.Id)
                .FirstOrDefault();

            if (result == null)
            {
                return NotFound(new
                {
                    Error = $"Result ID {model.Id} has not been found"
                });
            }

            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;

            result.LastModifiedDate = result.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(
                result.Adapt<ResultViewModel>(),
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = DbContext.Results.Where(i => i.Id == id)
                .FirstOrDefault();

            if (result == null)
            {
                return NotFound(new
                {
                    Error = $"Result ID {id} has not been found"
                });
            }

            DbContext.Results.Remove(result);
            DbContext.SaveChanges();

            return new OkResult();
        }

        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var results = DbContext.Results
                .Where(q => q.QuizId == quizId)
                .ToArray();

            return new JsonResult(
                results.Adapt<ResultViewModel[]>(),
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
        }
    }
}
