using Data.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Extensions
{
    public static class StartupExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnserService, AnserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IQuestionTemplateService, QuestionTemplateService>();
            services.AddScoped<ISurveyResultService, SurveyResultService>();
            services.AddScoped<ISurveySessionService, SurveySessionService>();
            services.AddScoped<IQuestionTemplateTypeService, QuestionTemplateTypeService>();
        }

        public static void ConfigCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
        }

        public static void ConfigMongoDb(this IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IMongoClient>(s => new MongoClient(connectionString));
            services.AddScoped(s => new AppDbContext(s.GetRequiredService<IMongoClient>(), databaseName));
        }
    }
}
