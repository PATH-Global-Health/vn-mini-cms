using AutoMapper;
using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Constant;

namespace Services.Core
{
    public interface IQuestionTemplateService
    {
        ResultModel Get(Guid id);
        ResultModel Add(QuestionTemplateAddModel model);
        ResultModel Update(Guid id, QuestionTemplateUpdateModel model);
        ResultModel Delete(Guid id);
        Task<ResultModel> Filter(string userId, int pageIndex, int pageSize);
        ResultModel AddQuestion(QuestionTemplateQuestionModel model);
        ResultModel RemoveQuestion(QuestionTemplateQuestionModel model);
        ResultModel AddSurveyResult(QuestionTemplateSuveyResultAddModel model);
        ResultModel RemoveSurveyResult(QuestionTemplateSuveyResultDeleteModel model);
    }
    public class QuestionTemplateService : IQuestionTemplateService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public QuestionTemplateService(AppDbContext dbContext, IMapper mapper, ICacheService cache)
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
                var questionTemplate = _dbContext.QuestionTemplates.Find(f => f.Id == id).FirstOrDefault();

                if (questionTemplate == null)
                {
                    throw new Exception("Invalid id");
                }

                var listQuestionOrder = new List<QuestionOrder>();
                foreach (var qs in questionTemplate.Questions)
                {
                    var qstmp = _dbContext.Questions.Find(x => x.Id == qs.Id && x.IsDeleted == false).FirstOrDefault();
                    if (qstmp == null) continue;
                    var ans = qstmp.Answers.FindAll(x => x.IsDeleted == false);
                    qstmp.Answers = ans;
                    var qsOrder = _mapper.Map<Question, QuestionOrder>(qstmp);
                    qsOrder.Order = qs.Order;
                    listQuestionOrder.Add(qsOrder);
                }
                questionTemplate.Questions = listQuestionOrder;
                _dbContext.QuestionTemplates.FindOneAndReplace(f => f.Id == questionTemplate.Id, questionTemplate);

                result.Data = _mapper.Map<QuestionTemplate, QuestionTemplateViewModel>(questionTemplate);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public async Task<ResultModel> Filter(string userId, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var questionDB = await _dbContext.QuestionTemplates.FindAsync(f => !f.IsDeleted);
                var questionTemplate = questionDB.ToList();
                questionTemplate = questionTemplate.OrderByDescending(o => o.DateCreated).ToList();
                var paging = questionTemplate.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var data = new List<QuestionTemplateUserModel>();

                foreach (var item in paging)
                {
                    var dataItem = _mapper.Map<QuestionTemplate, QuestionTemplateUserModel>(item);

                    if (!string.IsNullOrEmpty(userId))
                    {
                        dataItem.IsCompleted = _dbContext.SurveySessions.Find(f => f.UserId == userId && f.QuestionTemplateId == item.Id).Any();
                    }
                    data.Add(dataItem);
                }

                var pagingData = new PagingModel()
                {
                    Data = data,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalSize = questionTemplate.Count
                };

                result.Data = pagingData;
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
                //                var questions = _dbContext.Questions.Find(f => model.Questions.Contains(f.Id)).ToList();
                var questionOrders = new List<QuestionOrder>();
                foreach (var ques in model.Questions)
                {
                    var qs = _dbContext.Questions.Find(x => x.Id == ques.QuestionId).FirstOrDefault();
                    if (qs == null) throw new Exception("Invalid question Id");

                    var questionOrder = _mapper.Map<Question, QuestionOrder>(qs);
                    questionOrder.Order = ques.Order;
                    questionOrders.Add(questionOrder);
                }
                var surveyResults = _mapper.Map<List<SurveyResultAddModel>, List<SurveyResult>>(model.SurveyResults);
                questionTemplate.Questions = questionOrders;
                questionTemplate.SurveyResults = surveyResults;
                questionTemplate.QuestionTemplateTypeId = model.QuestionTemplateTypeId;

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



                //               var questions = _dbContext.Questions.Find(f => model.Questions.Contains(f.Id)).ToList();

                var questionOrders = new List<QuestionOrder>();
                foreach (var ques in model.Questions)
                {
                    var qs = _dbContext.Questions.Find(x => x.Id == ques.QuestionId).FirstOrDefault();
                    if (qs == null) throw new Exception("Invalid question Id");

                    var questionOrder = _mapper.Map<Question, QuestionOrder>(qs);
                    questionOrder.Order = ques.Order;
                    questionOrders.Add(questionOrder);
                }
                questionTemplate.Questions = questionOrders;
                questionTemplate.DateUpdated = DateTime.Now;
                questionTemplate.Description = model.Description;
                questionTemplate.Title = model.Title;
                questionTemplate.QuestionTemplateTypeId = model.QuestionTemplateTypeId;

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

                var questionOrders = new List<QuestionOrder>();
                foreach (var ques in model.Questions)
                {
                    var qs = _dbContext.Questions.Find(x => x.Id == ques.QuestionId).FirstOrDefault();
                    if (qs == null) throw new Exception("Invalid question Id");

                    var questionOrder = _mapper.Map<Question, QuestionOrder>(qs);
                    questionOrder.Order = ques.Order;
                    questionOrders.Add(questionOrder);
                }

                questionTemplate.DateUpdated = DateTime.Now;
                var questionsOrder = questionOrders;
                questionTemplate.Questions.AddRange(questionsOrder);

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

                foreach (var item in model.Questions)
                {
                    var thisQues = questionTemplate.Questions.Find(x => x.Id == item.QuestionId);
                    questionTemplate.Questions.Remove(thisQues);
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
