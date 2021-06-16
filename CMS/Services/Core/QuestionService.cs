using AutoMapper;
using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Core
{
    public interface IQuestionService
    {
        ResultModel Get(Guid? id);
        ResultModel Add(QuestionAddModel model);
        ResultModel Update(Guid id, QuestionUpdateModel model);
        ResultModel Delete(Guid id);
    }
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public QuestionService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel Get(Guid? id)
        {
            var result = new ResultModel();
            try
            {
                var questions = _dbContext.Questions.Find(f => (id == null || f.Id == id) && f.IsDeleted == false).ToList();

                result.Data = _mapper.Map<List<Question>, List<QuestionViewModel>>(questions);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(QuestionAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var question = _mapper.Map<QuestionAddModel, Question>(model);

                question.Answers = _mapper.Map<List<AnswerAddModel>, List<Answer>>(model.Answers);

                _dbContext.Questions.InsertOne(question);

                result.Data = question.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, QuestionUpdateModel model)
        {
            var result = new ResultModel();
            try
            {
                var question = _dbContext.Questions.Find(f => f.Id == id).FirstOrDefault();
                if (question == null)
                {
                    throw new Exception("Invalid question id");
                }

                question.Description = model.Description;
                question.IsMultipleChoice = model.IsMultipleChoice;

                _dbContext.Questions.FindOneAndReplace(f => f.Id == question.Id, question);

                result.Data = question.Id;
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
                var question = _dbContext.Questions.Find(f => f.Id == id).FirstOrDefault();
                if (question == null)
                {
                    throw new Exception("Invalid question id");
                }

                question.IsDeleted = true;

                _dbContext.Questions.FindOneAndReplace(f => f.Id == question.Id, question);

                result.Data = question.Id;
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
