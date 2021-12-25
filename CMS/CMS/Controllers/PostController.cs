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
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _postService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string searchText, [FromQuery] List<Guid> categories, [FromQuery] List<Guid> tags, bool? orderByCreatedDate, int? pageIndex = 0, int? pageSize = 10)
        {
            var rs = await _postService.Filter(searchText, categories, tags, orderByCreatedDate, pageIndex.Value, pageSize.Value);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Add(PostAddModel model)
        {
            var rs = _postService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost("Parts")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddParts(AddPartToPostModel model)
        {
            var rs = _postService.AddPartToPost(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Update(Guid id, PostUpdateModel model)
        {
            var rs = _postService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Delete(Guid id)
        {
            var rs = _postService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
