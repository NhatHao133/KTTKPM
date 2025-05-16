using ASC.DataAccess;
using ASC.DataAccess.Interfaces;
using ASC.Web.Configuration;
using ASC.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace ASC.Web.Areas.Accounts.Models
{
    public class ServiceEngineerViewModel
    {
        public List<IdentityUser>? SericeEngineers { get; set; }
        public ServiceEngineerRegistrationViewModel Registration { get; set; }
    }
}