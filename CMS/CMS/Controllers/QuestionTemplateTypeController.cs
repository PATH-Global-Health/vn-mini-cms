﻿using Data.ViewModels;
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
    public class QuestionTemplateTypeController : ControllerBase
    {
        private readonly IQuestionTemplateTypeService _templateTypeService;

        public QuestionTemplateTypeController(IQuestionTemplateTypeService templateTypeService)
        {
            _templateTypeService = templateTypeService;
        }

        [HttpGet]
        public IActionResult Get(Guid? id)
        {
            var rs = _templateTypeService.Get(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost]
        public IActionResult Add(QuestionTemplateTypeAddModel model)
        {
            var rs = _templateTypeService.Add(model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] QuestionTemplateTypeAddModel model)
        {
            var rs = _templateTypeService.Update(id, model);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var rs = _templateTypeService.Delete(id);
            if (rs.Succeed) return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }
    }
}
