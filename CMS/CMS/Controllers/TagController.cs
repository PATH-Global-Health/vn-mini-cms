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
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Guid? id)
        {
            var rs = await _tagService.GetAsync(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Add(TagAddModel model)
        {
            var rs = _tagService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Update(Guid id, [FromBody] TagAddModel model)
        {
            var rs = _tagService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Delete(Guid id)
        {
            var rs = _tagService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
