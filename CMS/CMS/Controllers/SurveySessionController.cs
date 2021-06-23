using Data.ViewModels;
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
        public IActionResult Add(SurveySessionAddModel model)
        {
            var rs = _surveySessionService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
