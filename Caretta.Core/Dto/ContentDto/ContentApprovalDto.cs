using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caretta.Core.Enums;

namespace Caretta.Core.Dto.ContentDto
{
    public class ContentApprovalDto
    {
        public Guid ContentId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
