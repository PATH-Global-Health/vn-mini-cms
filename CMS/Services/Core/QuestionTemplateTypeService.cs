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
    public interface IQuestionTemplateTypeService
    {
        Task<ResultModel> Get(Guid? id);
        ResultModel Add(QuestionTemplateTypeAddModel model);
        ResultModel Update(Guid id, QuestionTemplateTypeAddModel model);
        ResultModel Delete(Guid id);
    }
    public class QuestionTemplateTypeService : IQuestionTemplateTypeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public QuestionTemplateTypeService(AppDbContext dbContext, IMapper mapper, ICacheService cache)
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
                var listType = new List<QuestionTemplateTypeViewModel>();
                listType = await _cache.GetCache<List<QuestionTemplateTypeViewModel>>(RedisKey
                    .QUESTION_TEMPLATE_TYPE_VIEW);
                if (listType == null)
                {
                    var type = _dbContext.QuestionTemplateTypes.Find(f =>!f.IsDeleted).ToList();
                    listType = _mapper.Map<List<QuestionTemplateType>, List<QuestionTemplateTypeViewModel>>(type);
                    _cache.SetDefautCache(RedisKey.QUESTION_TEMPLATE_TYPE_VIEW, listType);
                }

                result.Data = listType;
                if (id.HasValue)
                {
                    result.Data = listType.FirstOrDefault(x => x.Id == id);
                }

                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(QuestionTemplateTypeAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var type = _mapper.Map<QuestionTemplateTypeAddModel, QuestionTemplateType>(model);

                _dbContext.QuestionTemplateTypes.InsertOne(type);

                result.Data = type.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.QUESTION_TEMPLATE_TYPE_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, QuestionTemplateTypeAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var tag = _dbContext.QuestionTemplateTypes.Find(f => f.Id == id).FirstOrDefault();
                if (tag == null) throw new Exception("Invalid Id");

                tag.Description = model.Description;
                tag.DateUpdated = DateTime.Now;

                _dbContext.QuestionTemplateTypes.FindOneAndReplace(f => f.Id == id, tag);

                result.Data = tag.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.QUESTION_TEMPLATE_TYPE_VIEW);
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
                var tag = _dbContext.QuestionTemplateTypes.Find(f => f.Id == id).FirstOrDefault();

                if (tag == null) throw new Exception("Invalid id");
                
                tag.IsDeleted = true;

                _dbContext.QuestionTemplateTypes.FindOneAndReplace(f => f.Id == id, tag);

                result.Data = tag.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.QUESTION_TEMPLATE_TYPE_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}
