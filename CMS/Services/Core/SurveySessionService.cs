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
    public interface ISurveySessionService
    {
        ResultModel Get(Guid id);
        ResultModel Add(SurveySessionAddModel model);
    }
    public class SurveySessionService : ISurveySessionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SurveySessionService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel Get(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var surveySession = _dbContext.SurveySessions.Find(f => f.Id == id).FirstOrDefault();

                if (surveySession == null)
                {
                    throw new Exception("Invalid id");
                }

                result.Data = _mapper.Map<SurveySession, SurveySessionViewModel>(surveySession);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(SurveySessionAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var surveySession = _mapper.Map<SurveySessionAddModel, SurveySession>(model);

                var sessionResults = new List<SurveySessionResult>();
                foreach (var item in model.SurveySessionResults)
                {
                    var question = _dbContext.Questions.Find(f => f.Id == item.QuestionId).FirstOrDefault();
                    if (question == null) throw new Exception("Invalid question id: " + item.QuestionId);

                    var answer = question.Answers.FirstOrDefault(f => f.Id == item.AnswerId);
                    if (answer == null) throw new Exception("Invalid answer id: " + item.AnswerId);

                    var sessionResult = new SurveySessionResult() { Answer = answer, Question = question };
                    sessionResults.Add(sessionResult);
                }

                surveySession.SurveySessionResults = sessionResults;

                _dbContext.SurveySessions.InsertOne(surveySession);

                result.Data = surveySession.Id;
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
