using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class ListAccountsRequest
    {
        public int PageNumber { get; set; } = 1;  
        public int PageSize { get; set; } = 10;   
        public string? Role { get; set; } = null; 
        public string? FullName { get; set; } = null; 
    }
}
