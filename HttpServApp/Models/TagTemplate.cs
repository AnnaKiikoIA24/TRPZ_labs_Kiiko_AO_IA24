using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServApp.Models
{
    internal class TagTemplate
    {
        public long IdTag { get; set; }
        public string TagSource { get; set; } = string.Empty;
        public string HtmlSource { get; set; } = string.Empty;
    }
}
