using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class JwtTokenViewModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public double Expires { get; set; }
        /// <summary>
        /// token类型
        /// </summary>
        public string? TokenType { get; set; }
    }
}
