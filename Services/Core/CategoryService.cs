﻿using AutoMapper;
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
    public interface ICategoryService
    {
        public ResultModel Get(Guid? id);
        public ResultModel Add(CategoryAddModel model);
        public ResultModel Update(Guid id, CategoryAddModel model);
        public ResultModel Delete(Guid id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel Get(Guid? id)
        {
            var result = new ResultModel();
            try
            {
                var categories = _dbContext.Categories.Find(f => (id == null && f.IsDeleted == false) || f.Id == id).ToList();

                result.Data = _mapper.Map<List<Category>, List<CategoryViewModel>>(categories);
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Add(CategoryAddModel model)
        {
            var result = new ResultModel();
            try
            {
                var category = _mapper.Map<CategoryAddModel, Category>(model);

                _dbContext.Categories.InsertOne(category);

                result.Data = category.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public ResultModel Update(Guid id, CategoryAddModel model)
        {
            var result = new ResultModel();

            var transaction = _dbContext.StartSession();
            transaction.StartTransaction();
            try
            {
                var category = _dbContext.Categories.Find(f => f.Id == id).FirstOrDefault();
                if (category == null) throw new Exception("Invalid Id");

                category.Description = model.Description;
                category.DateUpdated = DateTime.Now;

                _dbContext.Categories.FindOneAndReplace(f => f.Id == id, category);
                UpdateAllReferences(category);

                transaction.CommitTransaction();

                result.Data = category.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                transaction.AbortTransaction();
            }
            finally
            {
                transaction.Dispose();
            }
            return result;
        }
        public void UpdateAllReferences(Category newCategory)
        {
            var posts = _dbContext.Posts.Find(f => f.Categories.Any(c => c.Id == newCategory.Id)).ToList();
            if (posts.Any())
            {
                foreach (var post in posts)
                {
                    var reference = post.Categories.FirstOrDefault(i => i.Id == newCategory.Id);
                    reference = _mapper.Map(newCategory, reference);
                    _dbContext.Posts.FindOneAndReplace(i => i.Id == post.Id, post);
                }
            }
        }
        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var category = _dbContext.Categories.Find(f => f.Id == id).FirstOrDefault();

                if (category == null) throw new Exception("Invalid id");

                category.IsDeleted = true;

                _dbContext.Categories.FindOneAndReplace(f => f.Id == category.Id,category);

                result.Data = category.Id;
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
