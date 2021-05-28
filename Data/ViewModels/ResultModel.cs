using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class ResultModel
    {
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool Succeed { get; set; }
    }
}
