using AutoMapper;
using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Constant;

namespace Services.Core
{
    public interface IAnswerService
    {
        ResultModel Get(Guid id);
        ResultModel Add(QuestionAddAnswerModel model);
        ResultModel Update(Guid id, AnswerAddModel model);
        ResultModel Delete(Guid id);
    }
    public class AnswerService : IAnswerService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public AnswerService(AppDbContext dbContext, IMapper mapper, ICacheService cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cache = cache;
        }
        public ResultModel Get(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var question = _dbContext.Questions.Find(f => f.Answers.Any(a => a.Id == id)).FirstOrDefault();
                if (question == null)
                {
                    throw new Exception("Cant find any question contains this answer!");
                }

                var answer = question.Answers.FirstOrDefault(f => f.Id == id);
                if (answer == null)
                {
                    throw new Exception("Invalid id");
                }

                if (answer.IsDeleted)
                {
                    throw new Exception("The answer has been deleted");
                }

                result.Data = answer;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(QuestionAddAnswerModel model)
        {
            var result = new ResultModel();
            try
            {
                var question = _dbContext.Questions.Find(f => f.Id == model.QuestionId).FirstOrDefault();
                if (question == null)
                {
                    throw new Exception("Invalid Question Id");
                }

                var newAnswers = _mapper.Map<List<AnswerAddModel>, List<Answer>>(model.Answers);

                question.Answers.AddRange(newAnswers);
                _dbContext.Questions.FindOneAndReplace(f => f.Id == question.Id, question);

                result.Data = question.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.QUESTION_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, AnswerAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var question = _dbContext.Questions.Find(f => f.Answers.Any(a => a.Id == id)).FirstOrDefault();
                if (question == null)
                {
                    throw new Exception("Cant find any question contains this answer!");
                }

                var answer = question.Answers.FirstOrDefault(f => f.Id == id);
                if (answer == null)
                {
                    throw new Exception("Invalid id");
                }

                question.Answers.Remove(answer);

                answer.Description = model.Description;
                answer.Score = model.Score;

                question.Answers.Add(answer);

                _dbContext.Questions.FindOneAndReplace(f => f.Id == question.Id, question);

                result.Data = answer.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.QUESTION_VIEW);
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
                var question = _dbContext.Questions.Find(f => f.Answers.Any(a => a.Id == id)).FirstOrDefault();
                if (question == null)
                {
                    throw new Exception("Cant find any question contains this answer!");
                }

                var answer = question.Answers.FirstOrDefault(f => f.Id == id);
                if (answer == null)
                {
                    throw new Exception("Invalid id");
                }

                question.Answers.Remove(answer);

                answer.IsDeleted = true;

                question.Answers.Add(answer);

                _dbContext.Questions.FindOneAndReplace(f => f.Id == question.Id, question);

                result.Data = answer.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.QUESTION_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}
