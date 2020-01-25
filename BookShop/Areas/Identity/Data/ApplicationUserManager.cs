using BookShop.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Areas.Identity.Data
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        private readonly ApplicationIdentityErrorDescriber _identityErrorDescriber;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly ILogger<ApplicationUserManager> _logger;
        private readonly IOptions<IdentityOptions> _options;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IEnumerable<IPasswordValidator<ApplicationUser>> _passwordValidators;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IEnumerable<IUserValidator<ApplicationUser>> _userValidators;


        public ApplicationUserManager(
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptions<IdentityOptions> options,
            ILogger<ApplicationUserManager> logger,
            ApplicationIdentityErrorDescriber identityErrorDescriber,
            ILookupNormalizer lookupNormalizer,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            IServiceProvider serviceProvider,
            IUserStore<ApplicationUser> userStore,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators
            )
            : base(userStore, options, passwordHasher, userValidators, passwordValidators, lookupNormalizer, identityErrorDescriber, serviceProvider, logger)
        {
            _identityErrorDescriber = identityErrorDescriber;
            _lookupNormalizer = lookupNormalizer;
            _logger = logger;
            _options = options;
            _passwordHasher = passwordHasher;
            _passwordValidators = passwordValidators;
            _serviceProvider = serviceProvider;
            _userStore = userStore;
            _userValidators = userValidators;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task<List<UsersViewModel>> GetAllUsersWithRolesAsync()
        {
            return await Users.Select(a => new UsersViewModel
            {
                UserID = a.Id,
                UserName = a.UserName,
                BirthDate = a.BirthDate,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                ImagePath = a.Image,
                IsActive = a.IsActive,
                LastVisit = a.LastVisitDateTime,
                Mobile = a.PhoneNumber,
                RegisterDate = a.RegisterDate,
                Roles = a.UserRoles.Select(u => u.Role.Name),
            }).AsNoTracking().ToListAsync();
        }
        public async Task<UsersViewModel> FindUserWithRoleByIdAsync(string id)
        {
            return await Users.Where(a=>a.Id==id).Select(a => new UsersViewModel
            {
                UserID=a.Id,
                UserName=a.UserName,
                BirthDate=a.BirthDate,
                Email=a.Email,
                FirstName=a.FirstName,
                LastName=a.LastName,
                ImagePath=a.Image,
                IsActive=a.IsActive,
                LastVisit=a.LastVisitDateTime,
                Mobile=a.PhoneNumber,
                RegisterDate=a.RegisterDate,
                Roles = a.UserRoles.Select(u => u.Role.Name),
                ConfrmEmail=a.EmailConfirmed,
                ConfrmMobile=a.PhoneNumberConfirmed,
                EndLockOut=a.LockoutEnd,
                IsLock=a.LockoutEnabled,
                AccessFailedCount = a.AccessFailedCount,
                TwoFactorRegister=a.TwoFactorEnabled,
                
            }).AsNoTracking().FirstOrDefaultAsync();
}

        public async Task<string> GetFullName(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUserAsync(claimsPrincipal);
            return user.FirstName + " " + user.LastName;
        }
        public async Task<string> GetImage(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUserAsync(claimsPrincipal);
            return user.Image;
        }

    }
}
