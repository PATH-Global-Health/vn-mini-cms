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
    public interface ITagService
    {
        Task<ResultModel> GetAsync(Guid? id);
        ResultModel Add(TagAddModel model);
        ResultModel Update(Guid id, TagAddModel model);
        ResultModel Delete(Guid id);
    }
    public class TagService : ITagService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public TagService(AppDbContext dbContext, IMapper mapper, ICacheService cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ResultModel> GetAsync(Guid? id)
        {
            var result = new ResultModel();
            try
            {
                var listTag = new List<TagViewModel>();
                    listTag = await _cache.GetCache<List<TagViewModel>>(RedisKey.TAGS_VIEW);
                if (listTag == null)
                {
                    var tags = _dbContext.Tags.Find(f => (id == null && f.IsDeleted == false) || f.Id == id).ToList();
                    listTag = _mapper.Map<List<Data.MongoCollections.Tag>, List<TagViewModel>>(tags);
                    _cache.SetDefautCache(RedisKey.TAGS_VIEW,listTag);
                }

                result.Data = listTag;
                if (id.HasValue)
                {
                   var tag = listTag.FirstOrDefault(x => x.Id == id);
                   if(tag == null) throw new Exception("TagId does not exist");
                   result.Data = tag;
                }

                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(TagAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var tag = _mapper.Map<TagAddModel, Data.MongoCollections.Tag>(model);

                _dbContext.Tags.InsertOne(tag);

                result.Data = tag.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.TAGS_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, TagAddModel model)
        {
            var result = new ResultModel();

//            var transaction = _dbContext.StartSession();
//            transaction.StartTransaction();
            try
            {
                var tag = _dbContext.Tags.Find(f => f.Id == id).FirstOrDefault();
                if (tag == null) throw new Exception("Invalid Id");

                tag.Description = model.Description;
                tag.DateUpdated = DateTime.Now;

                _dbContext.Tags.FindOneAndReplace(f => f.Id == id, tag);
                UpdateAllReferences(tag);

  //              transaction.CommitTransaction();

                result.Data = tag.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.TAGS_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
  //              transaction.AbortTransaction();
            }
            finally
            {
 //               transaction.Dispose();
            }
            return result;
        }
        public void UpdateAllReferences(Data.MongoCollections.Tag newTag)
        {
            var posts = _dbContext.Posts.Find(f => f.Tags.Any(c => c.Id == newTag.Id)).ToList();
            if (posts.Any())
            {
                foreach (var post in posts)
                {
                    var reference = post.Tags.FirstOrDefault(i => i.Id == newTag.Id);
                    reference = _mapper.Map(newTag, reference);
                    _dbContext.Posts.FindOneAndReplace(i => i.Id == post.Id, post);
                }
            }
        }
        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var tag = _dbContext.Tags.Find(f => f.Id == id).FirstOrDefault();

                if (tag == null) throw new Exception("Invalid id");

                tag.IsDeleted = true;

                _dbContext.Tags.FindOneAndReplace(f => f.Id == id,tag);

                result.Data = tag.Id;
                result.Succeed = true;
                _cache.DeleteKey(RedisKey.TAGS_VIEW);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}
