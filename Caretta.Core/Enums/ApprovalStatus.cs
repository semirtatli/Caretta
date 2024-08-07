using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Core.Enums
{
    public enum ApprovalStatus
    {
        [Description("Draft Status")]
        Draft,

        [Description("Pending Approval")]
        Pending,

        [Description("Approved by Editor")]
        EditorApproved,

        [Description("Approved by Admin")]
        AdminApproved,

        [Description("Rejected")]
        Rejected
    }
}
