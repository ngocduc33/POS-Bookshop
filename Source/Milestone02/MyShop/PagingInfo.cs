using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class PagingInfo
    {
        public int RowsPerPage { get; set; }//Hàng trên mỗi trang
        public int CurrentPage { get; set; }//Trang hiện tại
        public int TotalPages { get; set; }//Tổng số trang
        public int TotalItems { get; set; }//Tổng số mặt hàng
        public List<string> Pages
        {
            get
            {
                var result = new List<string>();

                for (var i = 1; i <= TotalPages; i++)
                {
                    result.Add($"{i} / {TotalPages}");
                    //result.Add(i);
                }

                return result;
            }
        }
    }
}
