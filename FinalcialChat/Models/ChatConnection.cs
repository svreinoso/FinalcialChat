using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Models
{
    public class ChatConnection
    {
        public int Id { get; set; }
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}