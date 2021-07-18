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
        ResultModel UserChecked(string userId, Guid templateId);
        ResultModel GetByUser(string id);
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
        public ResultModel GetByUser(string id)
        {
            var result = new ResultModel();
            try
            {
                var surveySessions = _dbContext.SurveySessions.Find(f => f.UserId == id).ToList();

                if (surveySessions == null)
                {
                    throw new Exception("Invalid id");
                }

                result.Data = _mapper.Map<List<SurveySession>, List<SurveySessionViewModel>>(surveySessions);
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

                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == model.QuestionTemplateId).FirstOrDefault();

                List<Guid> answerIds = model.SurveySessionResults.Select(s => s.AnswerId).ToList();

                double userScore = 0;
                foreach (var question in questionTemplate.Questions)
                {
                    var ans = question.Answers.FirstOrDefault(f => answerIds.Contains(f.Id));
                    userScore += ans.Score;
                }

                var data = new UserTestResult()
                {
                    UserScore = userScore,
                    SurveyResult = _mapper.Map<SurveyResult, SurveyResultViewModel>(questionTemplate.SurveyResults.FirstOrDefault(f => userScore >= f.FromScore && userScore <= f.ToScore))
                };

                result.Data = data;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel UserChecked(string userId, Guid templateId)
        {
            var result = new ResultModel();
            try
            {
                var data = _dbContext.SurveySessions.Find(f => f.UserId == userId && f.QuestionTemplateId == templateId).FirstOrDefault();

                result.Data = _mapper.Map<SurveySession, SurveySessionViewModel>(data);

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
