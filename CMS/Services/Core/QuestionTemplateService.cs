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
    public interface IQuestionTemplateService
    {
        ResultModel Get(Guid id);
        ResultModel Add(QuestionTemplateAddModel model);
        ResultModel Update(Guid id, QuestionTemplateUpdateModel model);
        ResultModel Delete(Guid id);

        ResultModel AddQuestion(QuestionTemplateQuestionModel model);
        ResultModel RemoveQuestion(QuestionTemplateQuestionModel model);

        ResultModel AddSurveyResult(QuestionTemplateSuveyResultAddModel model);
        ResultModel RemoveSurveyResult(QuestionTemplateSuveyResultDeleteModel model);
    }
    public class QuestionTemplateService : IQuestionTemplateService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public QuestionTemplateService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel Get(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                result.Data = _mapper.Map<QuestionTemplate, QuestionTemplateViewModel>(questionTemplate);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(QuestionTemplateAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _mapper.Map<QuestionTemplateAddModel, QuestionTemplate>(model);

                //Get question and map survey result
                var questions = _dbContext.Questions.Find(f => model.Questions.Contains(f.Id)).ToList();
                var surveyResults = _mapper.Map<List<SurveyResultAddModel>, List<SurveyResult>>(model.SurveyResults);

                questionTemplate.Questions = questions;
                questionTemplate.SurveyResults = surveyResults;

                //Insert
                _dbContext.QuestionTemplates.InsertOne(questionTemplate);

                result.Data = questionTemplate.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, QuestionTemplateUpdateModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                var questions = _dbContext.Questions.Find(f => model.Questions.Contains(f.Id)).ToList();

                questionTemplate.Questions = questions;
                questionTemplate.DateUpdated = DateTime.Now;
                questionTemplate.Description = model.Description;
                questionTemplate.Title = model.Title;

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = questionTemplate.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                questionTemplate.IsDeleted = true;

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = questionTemplate.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel AddQuestion(QuestionTemplateQuestionModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == model.Id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                var questions = _dbContext.Questions.Find(f => model.Questions.Contains(f.Id)).ToList();

                questionTemplate.DateUpdated = DateTime.Now;
                questionTemplate.Questions.AddRange(questions);

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = questionTemplate.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel RemoveQuestion(QuestionTemplateQuestionModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == model.Id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                var questions = questionTemplate.Questions.Where(f => model.Questions.Contains(f.Id)).ToList();

                foreach (var item in questions)
                {
                    questionTemplate.Questions.Remove(item);
                }

                questionTemplate.DateUpdated = DateTime.Now;

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = questionTemplate.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel AddSurveyResult(QuestionTemplateSuveyResultAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == model.Id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }
                var surveyResults = _mapper.Map<List<SurveyResultAddModel>, List<SurveyResult>>(model.SurveyResults);

                questionTemplate.DateUpdated = DateTime.Now;
                questionTemplate.SurveyResults.AddRange(surveyResults);

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = questionTemplate.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel RemoveSurveyResult(QuestionTemplateSuveyResultDeleteModel model)
        {
            var result = new ResultModel();
            try
            {
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == model.Id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                var surveyResults = questionTemplate.SurveyResults.Where(f => model.SurveyResults.Contains(f.Id)).ToList();

                foreach (var item in surveyResults)
                {
                    questionTemplate.SurveyResults.Remove(item);
                }

                questionTemplate.DateUpdated = DateTime.Now;

                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = questionTemplate.Id;
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
