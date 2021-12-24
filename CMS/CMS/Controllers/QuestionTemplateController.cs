using Data.MongoCollections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public IActionResult Add(QuestionTemplateAddModel model)
        {
            var rs = _questionTemplateService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(Guid id, [FromBody] QuestionTemplateUpdateModel model)
        {
            var rs = _questionTemplateService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            var rs = _questionTemplateService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("Questions")]
        [Authorize]
        public IActionResult AddQuestion(QuestionTemplateQuestionModel model)
        {
            var rs = _questionTemplateService.AddQuestion(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("Questions")]
        [Authorize]
        public IActionResult RemoveQuestion(QuestionTemplateQuestionModel model)
        {
            var rs = _questionTemplateService.RemoveQuestion(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("SurveyResult")]
        [Authorize]
        public IActionResult AddSurveyResult(QuestionTemplateSuveyResultAddModel model)
        {
            var rs = _questionTemplateService.AddSurveyResult(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("SurveyResult")]
        [Authorize]
        public IActionResult RemoveSurveyResult(QuestionTemplateSuveyResultDeleteModel model)
        {
            var rs = _questionTemplateService.RemoveSurveyResult(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
