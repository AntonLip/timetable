using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TimetibleMicroservices.Models.DTOModels;

namespace TimetibleMicroservices.WebApi
{
    public class HttpModelResultFilterAttribute: Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var result = context.Result as ObjectResult;
            if(result !=  null)
                result.Value = new ResultDto<object>()
                {
                    Value = result.Value, 
                    Errors = new List<string>()
                };
            await next();
        }
    }
}
