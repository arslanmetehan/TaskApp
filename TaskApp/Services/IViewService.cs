using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IViewService
    {
        TModel CreateViewModel<TModel>(HttpContext httpContext, string pageTitle) where TModel : BaseViewModel, new();
    }
}
