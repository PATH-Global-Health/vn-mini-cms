using Data.MongoCollections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionTemplateController : ControllerBase
    {
        private readonly IQuestionTemplateService _questionTemplateService;

        public QuestionTemplateController(IQuestionTemplateService questionTemplateService)
        {
            _questionTemplateService = questionTemplateService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var rs = _questionTemplateService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> Filter(string userId, int? pageIndex = 1, int? pageSize = 10)
        {
            var rs = await _questionTemplateService.Filter(userId, pageIndex.Value, pageSize.Value);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        public IActionResult Add(QuestionTemplateAddModel model)
        {
            var rs = _questionTemplateService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] QuestionTemplateUpdateModel model)
        {
            var rs = _questionTemplateService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var rs = _questionTemplateService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("Questions")]
        public IActionResult AddQuestion(QuestionTemplateQuestionModel model)
        {
            var rs = _questionTemplateService.AddQuestion(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("Questions")]
        public IActionResult RemoveQuestion(QuestionTemplateQuestionModel model)
        {
            var rs = _questionTemplateService.RemoveQuestion(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("SurveyResult")]
        public IActionResult AddSurveyResult(QuestionTemplateSuveyResultAddModel model)
        {
            var rs = _questionTemplateService.AddSurveyResult(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("SurveyResult")]
        public IActionResult RemoveSurveyResult(QuestionTemplateSuveyResultDeleteModel model)
        {
            var rs = _questionTemplateService.RemoveSurveyResult(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
