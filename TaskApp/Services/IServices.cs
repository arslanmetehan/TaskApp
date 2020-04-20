using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Services
{
    public interface IServices
    {
        IUserService UserService { get; }
        IMissionService MissionService { get; }
        IOperationService OperationService { get; }
        IViewService ViewService { get; }
    }
}
