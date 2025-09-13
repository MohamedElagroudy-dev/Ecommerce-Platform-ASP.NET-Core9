using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Sharing
{
    public class ProductParams
    {
        public ProductSort? Sort { get; set; } = ProductSort.Id;
        public int? CategoryId { get; set; }

        public int? TotatlCount { get; set; }

        public string? Search { get; set; }
        public int MaxPageSize { get; set; } = 6;
        private int _pageSize = 3;

        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageNumber { get; set; } = 1;

    }
}
