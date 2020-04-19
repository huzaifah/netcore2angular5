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
    public class AnswerController : BaseApiController
    {
        public AnswerController(ApplicationDbContext context) : base(context)
        {
        }

        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            //var sampleAnswers = new List<AnswerViewModel>();

            //sampleAnswers.Add(new AnswerViewModel()
            //{
            //    Id = 1,
            //    QuestionId = questionId,
            //    Text = "Friends and family",
            //    CreatedDate = DateTime.Now,
            //    LastModifiedDate = DateTime.Now
            //});

            //for (int i = 2; i <= 5; i++)
            //{
            //    sampleAnswers.Add(new AnswerViewModel()
            //    {
            //        Id = i,
            //        QuestionId = questionId,
            //        Text = $"Sample Answer {i}",
            //        CreatedDate = DateTime.Now,
            //        LastModifiedDate = DateTime.Now
            //    });
            //}

            //return new JsonResult(
            //    sampleAnswers,
            //    new Newtonsoft.Json.JsonSerializerSettings()
            //    {
            //        Formatting = Newtonsoft.Json.Formatting.Indented
            //    });

            var answers = DbContext.Answers
                .Where(q => q.QuestionId == questionId)
                .ToArray();

            return new JsonResult(
                answers.Adapt<AnswerViewModel[]>(),
                JsonSettings);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = DbContext.Answers
                            .Where(i => i.Id == id)
                            .FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = $"Answer ID {id} has not been found"
                });
            }

            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                JsonSettings);
        }

        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var answer = model.Adapt<Answer>();

            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;

            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;

            DbContext.Answers.Add(answer);
            DbContext.SaveChanges();

            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            if (model == null)
                return new StatusCodeResult(500);

            var answer = DbContext.Answers
                            .Where(q => q.Id == model.Id)
                            .FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = $"Answer ID {model.Id} has not been found"
                });
            }

            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;

            answer.LastModifiedDate = answer.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = DbContext.Answers
                            .Where(i => i.Id == id)
                            .FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = $"Answer ID {id} has not been found"
                });
            }

            DbContext.Answers.Remove(answer);
            DbContext.SaveChanges();

            return new OkResult();
        }
    }
}
