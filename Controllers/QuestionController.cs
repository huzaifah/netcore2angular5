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
    public class QuestionController : BaseApiController
    {
        public QuestionController(ApplicationDbContext context) : base(context)
        {
        }

        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = DbContext.Questions
                .Where(q => q.QuizId == quizId)
                .ToArray();

            return new JsonResult(
                questions.Adapt<QuestionViewModel[]>(),
                JsonSettings
            );
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions
                .Where(i => i.Id == id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {id} has not been found"
                });
            }

            return new JsonResult(
                question.Adapt<QuestionViewModel>(),
                JsonSettings
            );
        }

        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var question = model.Adapt<Question>();

            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;

            DbContext.Questions.Add(question);
            DbContext.SaveChanges();

            return new JsonResult(
                question.Adapt<QuestionViewModel>(),
                JsonSettings
                );
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var question = DbContext.Questions
                .Where(q => q.Id == model.Id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {model.Id} has not been found"
                });
            }

            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            question.LastModifiedDate = question.CreatedDate;
            DbContext.SaveChanges();

            return new JsonResult(
                question.Adapt<QuestionViewModel>(),
                JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var question = DbContext.Questions
                .Where(i => i.Id == id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {id} has not been found"
                });
            }

            DbContext.Questions.Remove(question);
            DbContext.SaveChanges();

            return new OkResult();
        }
    }
}
