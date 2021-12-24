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
    public class SurveyResultController : ControllerBase
    {
        private readonly ISurveyResultService _surveyResultService;

        public SurveyResultController(ISurveyResultService surveyResultService)
        {
            _surveyResultService = surveyResultService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var rs = _surveyResultService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Add(SurveyResultViewModel model)
        {
            var rs = _surveyResultService.Update(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
