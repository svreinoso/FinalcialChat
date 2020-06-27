using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Interfaces
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset ModifiedDate { get; set; }
    }
}
