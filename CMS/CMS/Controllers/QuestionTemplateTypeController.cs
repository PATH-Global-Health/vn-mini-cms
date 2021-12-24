using Data.ViewModels;
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
    public class QuestionTemplateTypeController : ControllerBase
    {
        private readonly IQuestionTemplateTypeService _templateTypeService;

        public QuestionTemplateTypeController(IQuestionTemplateTypeService templateTypeService)
        {
            _templateTypeService = templateTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Guid? id)
        {
            var rs = await _templateTypeService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(QuestionTemplateTypeAddModel model)
        {
            var rs = _templateTypeService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(Guid id, [FromBody] QuestionTemplateTypeAddModel model)
        {
            var rs = _templateTypeService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            var rs = _templateTypeService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
