using BookShop.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Areas.Identity.Data
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole> , IApplicationRoleManager
    {
        private readonly IdentityErrorDescriber _identityErrorDescriber;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly ILogger<ApplicationRoleManager> _logger;
        private readonly IEnumerable<IRoleValidator<ApplicationRole>> _roleValidators;
        private readonly IRoleStore<ApplicationRole> _roleStore;

        public ApplicationRoleManager(IRoleStore<ApplicationRole> roleStore,IEnumerable<IRoleValidator<ApplicationRole>> roleValidators
            ,ILogger<ApplicationRoleManager> logger,IdentityErrorDescriber identityErrorDescriber, ILookupNormalizer lookupNormalizer)
            :base(roleStore,roleValidators,lookupNormalizer,identityErrorDescriber,logger)
        {
            _identityErrorDescriber = identityErrorDescriber;
            _lookupNormalizer = lookupNormalizer;
            _logger = logger;
            _roleValidators = roleValidators;
            _roleStore = roleStore;
        }

        public IEnumerable<ApplicationRole> GetAllRoles()
        {
            return  Roles.AsNoTracking().ToList();
        }

        public IEnumerable<RolesViewModel> GetAllRolesAndUsersCount()
        {
            return Roles.Select(a => new RolesViewModel
            {
                RoleID = a.Id,
                RoleName = a.Name,
                Description = a.Description,
                UserCount = a.UserRoles.Count()
            }).ToList();
        }


    }
}
