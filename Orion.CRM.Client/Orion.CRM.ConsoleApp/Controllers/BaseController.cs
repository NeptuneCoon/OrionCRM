using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Orion.CRM.ConsoleApp.Controllers
{
    public class BaseController : Controller
    {
        public readonly AppConfig _appConfig;

        public BaseController(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        public BaseController()
        {
        }
    }
}
