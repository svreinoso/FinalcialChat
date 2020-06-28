using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FinalcialChat.Tests
{
    public class MockHttpContext : HttpContextBase
    {
        private readonly IPrincipal user;

        public MockHttpContext(IPrincipal principal)
        {
            this.user = principal;
        }

        public override IPrincipal User
        {
            get
            {
                return user;
            }
            set
            {
                base.User = value;
            }
        }
    }

    public class CustomPrinpipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CustomPrinpipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string role)
        {
            return true;
        }


    }
}
