using DDOX.API.Dtos.File;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Data;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators
{
    public class ExtensionsValidator : ActionFilterAttribute
    {
        private readonly DataContext _context;

        public ExtensionsValidator(DataContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var systemExtensions = _context.Extension.Where(w => w.IsAllowed == true).Select(w => w.Name.ToLower()).ToList();
            try
            {
                var files = context.HttpContext.Request.Form.Files.ToList();
                var filesExtentions = files.Select(e => Path.GetExtension(e.FileName).ToLower()).ToList();

                var isValidExtention = filesExtentions.All(filesExt => systemExtensions.Any(systemExt => filesExt == systemExt));
                if (!isValidExtention)
                    context.Result = new BadRequestObjectResult("Your Request Contain ' UnAllowed Extentions' ");
            }
            catch
            {
                context.Result = new BadRequestObjectResult("Can't Upload This Files ");
            }
        }
    }
}
