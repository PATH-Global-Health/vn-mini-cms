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
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var rs = _fileService.Get(id);
                return File(rs.Data, rs.FileType);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpGet("Details/{id}")]
        public IActionResult GetDetails(Guid id)
        {
            var rs = _fileService.GetDetails(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Add([FromForm] FileAddModel model)
        {
            var rs = await _fileService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update([FromForm] FileUpdateModel model)
        {
            var rs = await _fileService.Update(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Delete(Guid id)
        {
            var rs = _fileService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
