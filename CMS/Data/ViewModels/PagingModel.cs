using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels
{
    public class PagingModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalSize { get; set; }
        public object Data { get; set; }
    }
}
