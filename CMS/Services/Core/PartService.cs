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
    public interface IPartService
    {
        ResultModel GetByPost(Guid postId);
        ResultModel Update(Guid id, PartUpdateModel model);
        ResultModel Delete(Guid id);
    }
    public class PartService : IPartService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PartService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel GetByPost(Guid postId)
        {
            var result = new ResultModel();
            try
            {
                var post = _dbContext.Posts.Find(f => f.Id == postId).FirstOrDefault();
                if (post == null) throw new Exception("Invalid post id");

                var parts = post.Parts.Where(f => f.IsDeleted == false).ToList();

                result.Data = _mapper.Map<List<Part>, List<PartViewModel>>(parts);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, PartUpdateModel model)
        {
            var result = new ResultModel();
            try
            {
                //Find post contain part
                var post = _dbContext.Posts.Find(f => f.Parts.Any(p => p.Id == id)).FirstOrDefault();
                if (post == null) throw new Exception("Invalid id");

                //Find part in post
                var currentPart = post.Parts.FirstOrDefault(f => f.Id == id);
                if (currentPart == null) throw new Exception("Invalid id");

                //Update part in post
                post.Parts.Remove(currentPart);
                //currentPart = _mapper.Map<PartUpdateModel, Part>(model);
                currentPart.DateUpdated = DateTime.Now;
                currentPart.Content = model.Content;
                currentPart.Order = model.Order;
                currentPart.Type = model.Type;

                post.Parts.Add(currentPart);

                _dbContext.Posts.FindOneAndReplace(f => f.Id == post.Id, post);

                result.Data = currentPart.Id;
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
                //Find post contain part
                var post = _dbContext.Posts.Find(f => f.Parts.Any(p => p.Id == id)).FirstOrDefault();
                if (post == null) throw new Exception("Invalid id");

                //Find part in post
                var currentPart = post.Parts.FirstOrDefault(f => f.Id == id);
                if (currentPart == null) throw new Exception("Invalid id");

                //Update part in post
                post.Parts.Remove(currentPart);
                currentPart.IsDeleted = true;
                post.Parts.Add(currentPart);

                _dbContext.Posts.FindOneAndReplace(f => f.Id == post.Id, post);

                result.Data = currentPart.Id;
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
