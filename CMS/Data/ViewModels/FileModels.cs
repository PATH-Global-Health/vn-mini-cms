using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.ViewModels
{
    public class FileAddModel
    {
        [Required]
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }

    public class FileUpdateModel
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }

    public class FileViewModel
    {
        public string FileType { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
    }

    public class FileDetailsViewModel
    {
        public Guid Id { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
    }
}
