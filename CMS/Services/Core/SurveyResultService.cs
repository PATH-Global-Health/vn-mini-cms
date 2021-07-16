using AutoMapper;
using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Core
{
    public interface ISurveyResultService
    {
        ResultModel Get(Guid id);
        ResultModel Update(SurveyResultViewModel model);
    }
    public class SurveyResultService : ISurveyResultService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SurveyResultService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel Get(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => true).ToList().Where(f => f.SurveyResults.Select(s => s.Id).Contains(id)).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("There is no question template contains this survey result id");
                }

                var surveyResult = questionTemplate.SurveyResults.FirstOrDefault(s => s.Id == id);

                result.Data = _mapper.Map<SurveyResult, SurveyResultViewModel>(surveyResult);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(SurveyResultViewModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.SurveyResults.Select(s => s.Id).Contains(model.Id)).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("There is no question template contains this survey result id");
                }

                var surveyResult = questionTemplate.SurveyResults.FirstOrDefault(s => s.Id == model.Id);

                questionTemplate.SurveyResults.Remove(surveyResult);

                surveyResult.DateUpdated = DateTime.Now;
                surveyResult.Description = model.Description;
                surveyResult.ToScore = model.ToScore;
                surveyResult.FromScore = model.FromScore;

                questionTemplate.SurveyResults.Add(surveyResult);

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = surveyResult.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}
