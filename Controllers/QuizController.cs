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
    public class QuizController : BaseApiController
    {
        public QuizController(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// GET: api/quiz/{}id
        /// Retrieves the Quiz with the given {id}
        /// </summary>
        /// <param name="id">The ID of an existing Quiz</param>
        /// <returns>the Quiz with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = DbContext.Quizzes
                .Where(q => q.Id == id).FirstOrDefault();

            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {id} has not been found"
                });
            }

            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                JsonSettings);
        }

        /// <summary>
        /// GET: api/quiz/latest
        /// Retrieves the {num} latest Quizzes
        /// </summary>
        /// <param name="num">The number of quizzes to retrieve</param>
        /// <returns>the {num} latest Quizzes</returns>
        [HttpGet("Latest/{num}")]
        public IActionResult Latest(int num = 10)
        {
            var latest = DbContext.Quizzes
                .OrderByDescending(q => q.CreatedDate)
                .Take(num)
                .ToArray();

            return new JsonResult(
                latest.Adapt<QuizViewModel[]>(),
                JsonSettings);
        }

        /// <summary>
        /// GET: api/quiz/ByTitle
        /// Retrieves the {num} Quizzes sorted by Title (A to Z)
        /// </summary>
        /// <param name="num">The number of quizzes to retrieve</param>
        /// <returns>{num} Quizzes sorted by Title</returns>
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = DbContext.Quizzes
                .OrderBy(q => q.Title)
                .Take(num)
                .ToArray();

            return new JsonResult(
                byTitle.Adapt<QuizViewModel[]>(),
                JsonSettings);
        }

        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = DbContext.Quizzes
                .OrderBy(q => Guid.NewGuid())
                .Take(num)
                .ToArray();

            return new JsonResult(
                random.Adapt<QuizViewModel[]>(),
                JsonSettings);

        }

        [HttpPut]
        public IActionResult Put([FromBody]QuizViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var quiz = new Quiz();

            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;

            quiz.UserId = DbContext.Users
                .Where(u => u.UserName == "Admin")
                .FirstOrDefault().Id;

            DbContext.Quizzes.Add(quiz);
            DbContext.SaveChanges();

            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuizViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var quiz = DbContext.Quizzes
                .Where(q => q.Id == model.Id)
                .FirstOrDefault();

            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {model.Id} has not been found"
                });
            }

            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            quiz.LastModifiedDate = quiz.CreatedDate;
            DbContext.SaveChanges();

            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quiz = DbContext.Quizzes
                .Where(q => q.Id == id)
                .FirstOrDefault();

            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {id} has not been found"
                });
            }

            DbContext.Quizzes.Remove(quiz);
            DbContext.SaveChanges();

            return new OkResult();
        }
    }
}
