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
    public class SurveySessionController : ControllerBase
    {
        private readonly ISurveySessionService _surveySessionService;

        public SurveySessionController(ISurveySessionService surveySessionService)
        {
            _surveySessionService = surveySessionService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var rs = _surveySessionService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Add(SurveySessionAddModel model)
        {
            var rs = _surveySessionService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("UserChecked/{userId}/{templateId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult UserChecked(string userId, Guid templateId)
        {
            var rs = _surveySessionService.UserChecked(userId, templateId);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("Hisories/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Hisories(string userId)
        {
            var rs = _surveySessionService.GetByUser(userId);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
