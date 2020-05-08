using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Services
{
    public class ServiceContainer : IServices
    {
		private readonly IServiceProvider _provider;

		private readonly Lazy<IUserService> userService;
		private readonly Lazy<IMissionService> missionService;
		private readonly Lazy<IOperationService> operationService;
		private readonly Lazy<IForumPostService> forumPostService;
		private readonly Lazy<IViewService> viewService;
		public ServiceContainer(IServiceProvider provider)
		{
			_provider = provider;

			userService = new Lazy<IUserService>(() => Resolve<IUserService>());
			forumPostService = new Lazy<IForumPostService>(() => Resolve<IForumPostService>());
			missionService = new Lazy<IMissionService>(() => Resolve<IMissionService>());
			operationService = new Lazy<IOperationService>(() => Resolve<IOperationService>());
			viewService = new Lazy<IViewService>(() => Resolve<IViewService>());
		}
		private TService Resolve<TService>()
		{
			return (TService)_provider.GetService(typeof(TService));
		}
		
		public IUserService UserService => userService.Value;
		public IMissionService MissionService => missionService.Value;
		public IOperationService OperationService => operationService.Value;
		public IForumPostService ForumPostService => forumPostService.Value;
		public IViewService ViewService => viewService.Value;
	}
}
