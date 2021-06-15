using AutoMapper;
using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tag = Data.MongoCollections.Tag;

namespace Services.Core
{
    public interface IPostService
    {
        ResultModel GetAll(bool? orderByDescending, int pageIndex, int pageSize);
        ResultModel Get(Guid id);
        ResultModel Add(PostAddModel model);
        ResultModel AddPartToPost(AddPartToPostModel model);
        ResultModel Update(Guid id, PostUpdateModel model);
        ResultModel Delete(Guid id);
    }
    public class PostService : IPostService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PostService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel GetAll(bool? orderByDescending, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var posts = _dbContext.Posts.Find(f => f.IsDeleted == false).ToList();
                if (orderByDescending == true)
                {
                    posts = posts.OrderByDescending(o => o.DateCreated).ToList();
                }
                else if (orderByDescending == false)
                {
                    posts = posts.OrderBy(o => o.DateCreated).ToList();
                }
                var data = new PagingModel()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalSize = posts.Count,
                    Data = _mapper.Map<List<Post>, List<PostViewModel>>(posts.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList())
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
        public ResultModel Get(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var post = _dbContext.Posts.Find(f => f.Id == id).FirstOrDefault();
                if (post == null) throw new Exception("Invalid id");

                result.Data = _mapper.Map<Post, PostViewModel>(post);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(PostAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var post = _mapper.Map<PostAddModel, Post>(model);

                //Add tag
                foreach (var item in model.Tags)
                {
                    var tag = _dbContext.Tags.Find(f => f.Id == item).FirstOrDefault();

                    post.Tags.Add(tag);
                }

                //Add category
                foreach (var item in model.Categories)
                {
                    var category = _dbContext.Categories.Find(f => f.Id == item).FirstOrDefault();
                    post.Categories.Add(category);
                }

                _dbContext.Posts.InsertOne(post);

                result.Data = post.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel AddPartToPost(AddPartToPostModel model)
        {
            var result = new ResultModel();
            try
            {
                var parts = _mapper.Map<List<PartAddModel>, List<Part>>(model.Parts);

                var post = _dbContext.Posts.Find(f => f.Id == model.PostId).FirstOrDefault();
                if (post == null) throw new Exception("Invalid post id");

                post.Parts.AddRange(parts);

                _dbContext.Posts.FindOneAndReplace(f => f.Id == model.PostId, post);

                result.Data = post.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, PostUpdateModel model)
        {
            var result = new ResultModel();
            try
            {
                var post = _dbContext.Posts.Find(f => f.Id == id).FirstOrDefault();

                if (post == null) throw new Exception("Invalid id");

                post.Name = model.Name;
                post.Writter = model.Writter;
                post.Categories = new List<Category>();
                post.Tags = new List<Tag>();
                post.Description = model.Description;

                //Update tag
                foreach (var item in model.Tags)
                {
                    var tag = _dbContext.Tags.Find(f => f.Id == item).FirstOrDefault();
                    post.Tags.Add(tag);
                }

                //Update category
                foreach (var item in model.Categories)
                {
                    var category = _dbContext.Categories.Find(f => f.Id == item).FirstOrDefault();
                    post.Categories.Add(category);
                }

                _dbContext.Posts.FindOneAndReplace(f => f.Id == post.Id, post);

                result.Data = post.Id;
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
                var post = _dbContext.Posts.Find(f => f.Id == id).FirstOrDefault();
                if (post == null) throw new Exception("Invalid id");

                post.IsDeleted = true;

                _dbContext.Posts.FindOneAndReplace(f => f.Id == post.Id, post);

                result.Data = post.Id;
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
