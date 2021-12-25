using Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using System;
using Microsoft.AspNetCore.Authorization;


namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var rs = _answerService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Add(QuestionAddAnswerModel model)
        {
            var rs = _answerService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Update(Guid id, [FromBody] AnswerAddModel model)
        {
            var rs = _answerService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Delete(Guid id)
        {
            var rs = _answerService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
