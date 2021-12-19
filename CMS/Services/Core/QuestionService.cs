using AutoMapper;
using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Constant;


namespace Services.Core
{
    public interface IQuestionService
    {
        Task<ResultModel> Get(Guid? id);
        ResultModel Add(QuestionAddModel model);
        ResultModel Update(Guid id, QuestionUpdateModel model);
        ResultModel Delete(Guid id);
        
    }
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public QuestionService(AppDbContext dbContext, IMapper mapper,ICacheService cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ResultModel> Get(Guid? id)
        {
            var result = new ResultModel();
            try
            {
                var listQuestionView = new List<QuestionViewModel>();
                listQuestionView = await _cache.GetCache<List<QuestionViewModel>>(RedisKey.QUESTION_VIEW);
                if (listQuestionView == null)
                {
                    var questions = _dbContext.Questions.Find(f =>!f.IsDeleted ).ToList();
                    foreach (var question in questions)
                    {
                        var ans = question.Answers.FindAll(x => x.IsDeleted == false).ToList();
                        question.Answers = ans;
                    }
                    listQuestionView = _mapper.Map<List<Question>, List<QuestionViewModel>>(questions);
                    _cache.SetDefautCache(RedisKey.QUESTION_VIEW, listQuestionView);
                }
                result.Data = listQuestionView;
                if (id.HasValue)
                {
                    var question =listQuestionView.FirstOrDefault(x => x.Id == id);
                    if(question == null) throw new Exception("QuestionId does not exist");
                    result.Data = question;
                }
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

                _cache.DeleteKey(RedisKey.QUESTION_VIEW);
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
//                UpdateAllReferences(question);

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
                _cache.DeleteKey(RedisKey.QUESTION_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public void UpdateAllReferences(Question question)
        {
            var questionTemplates = _dbContext.QuestionTemplates.Find(f => f.Questions.Any(c => c.Id == question.Id)).ToList();
            if (questionTemplates.Any())
            {
                foreach (var questionTemplate in questionTemplates)
                {
                    var reference = questionTemplate.Questions.FirstOrDefault(i => i.Id == questionTemplate.Id);
                    reference = _mapper.Map(questionTemplate, reference);
                    _dbContext.QuestionTemplates.FindOneAndReplace(i => i.Id == questionTemplate.Id, questionTemplate);
                }
            }
        }
    }
}
