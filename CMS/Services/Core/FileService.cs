using Data.DataAccess;
using Data.MongoCollections;
using Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core
{
    public interface IFileService
    {
        FileViewModel Get(Guid id);
        ResultModel GetDetails(Guid id);
        Task<ResultModel> Add(FileAddModel model);
        Task<ResultModel> Update(FileUpdateModel model);
        ResultModel Delete(Guid id);
    }
    public class FileService : IFileService
    {
        private readonly AppDbContext _dbContext;

        public FileService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FileViewModel Get(Guid id)
        {
            var fileData = _dbContext.FileData.Find(f => f.Id == id).FirstOrDefault();
            if (fileData == null)
            {
                throw new Exception("Invalid id");
            }

            return new FileViewModel()
            {
                Data = fileData.Data,
                FileName = fileData.Name,
                FileType = fileData.Type
            };
        }
        public ResultModel GetDetails(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var fileData = _dbContext.FileData.Find(f => f.Id == id).FirstOrDefault();
                if (fileData == null)
                {
                    throw new Exception("Invalid id");
                }

                result.Data = new FileDetailsViewModel()
                {
                    Id = fileData.Id,
                    Description = fileData.Description,
                    FileName = fileData.Name,
                    FileType = fileData.Type
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public async Task<ResultModel> Add(FileAddModel model)
        {
            var result = new ResultModel();
            try
            {
                FileData fileData = new FileData()
                {
                    Name = model.File.FileName,
                    Type = model.File.ContentType,
                    Description = model.Description,
                    Data = await FileToArray(model.File)
                };

                _dbContext.FileData.InsertOne(fileData);

                result.Data = fileData.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public async Task<ResultModel> Update(FileUpdateModel model)
        {
            var result = new ResultModel();
            try
            {
                var fileData = _dbContext.FileData.Find(f => f.Id == model.Id).FirstOrDefault();
                if (fileData == null)
                {
                    throw new Exception("Invalid id");
                }

                fileData.Name = model.File.FileName;
                fileData.Type = model.File.ContentType;
                fileData.Description = model.Description;
                fileData.Data = await FileToArray(model.File);

                _dbContext.FileData.FindOneAndReplace(f => f.Id == fileData.Id, fileData);

                result.Data = fileData.Id;
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
                var fileData = _dbContext.FileData.Find(f => f.Id == id).FirstOrDefault();
                if (fileData == null)
                {
                    throw new Exception("Invalid id");
                }

                _dbContext.FileData.DeleteOne(f => f.Id == fileData.Id);

                result.Data = fileData.Id;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        #region Helper
        public async Task<byte[]> FileToArray(IFormFile file)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        public IFormFile BytesToFile(byte[] data, string fileName)
        {
            var stream = new MemoryStream(data);
            return new FormFile(stream, 0, data.Length, "name", fileName);
        }
        #endregion
    }
}
