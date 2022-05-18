using BusinessLayer.Contracts;
using DataAccess.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserService: IUserService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._passwordHasher = passwordHasher;
        }

        public async Task<UserModel> Register(RegisterUserModel newUser)
        {
            try
            {
                foreach (var role in newUser.Roles)
                {
                    var roleFindResult = await _roleManager.FindByNameAsync(role.Name);
                    if (roleFindResult == null) return null;
                }

                if (newUser != null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = newUser.Username,

                    };
                    var result = await _userManager.CreateAsync(user, newUser.Password);
                    if (result != IdentityResult.Success)
                    {
                        return null;
                    }

                    var registeredUser = await _userManager.FindByNameAsync(user.UserName);
                    foreach (var role in newUser.Roles)
                    {
                        var roleAddResult = await _userManager.AddToRoleAsync(registeredUser, role.Name);
                        if (roleAddResult != IdentityResult.Success)
                        {
                            return null;
                        }
                    }

                    var createdUser = new UserModel
                    {
                        Username = newUser.Username
                    };
                    return createdUser;
                }
                else
                {

                    return null;

                }
            }
            catch(Exception e)
            {
                return null;
            }

        }
        public async Task<UserModel> Login(LoginUserModel user)
        {
            try
            {

                if (user != null)
                {
                    var existingUser = await _userManager.FindByNameAsync(user.Username);

                    if (existingUser != null)
                    {
                        var result = await signInManager.PasswordSignInAsync(existingUser, user.Password, false, false);

                        if (result.Succeeded)
                        {

                            var userRoles = await _userManager.GetRolesAsync(existingUser);
                            var rolesList = new List<RoleModel>();
                            foreach (var role in userRoles)
                            {
                                var roleEntity = await _roleManager.FindByNameAsync(role);
                                rolesList.Add(new RoleModel
                                {
                                    Id = roleEntity.Id,
                                    RoleName = roleEntity.Name
                                });
                            }

                            return new UserModel
                            {
                                Username = user.Username,
                                Roles = rolesList
                            };

                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserModel> Update(Guid id, String email, String password)
        {
            System.Diagnostics.Debug.WriteLine(id.ToString());
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    System.Diagnostics.Debug.WriteLine("Fail");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                else
                    System.Diagnostics.Debug.WriteLine("Fail");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        System.Diagnostics.Debug.WriteLine("Succes");
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var rolesList = new List<RoleModel>();
                        foreach (var role in userRoles)
                        {
                            var roleEntity = await _roleManager.FindByNameAsync(role);
                            rolesList.Add(new RoleModel
                            {
                                Id = roleEntity.Id,
                                RoleName = roleEntity.Name
                            });
                        }

                        return new UserModel
                        {

                            Roles = rolesList
                        };
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Succes");
                        return null;
                    }
                }
                System.Diagnostics.Debug.WriteLine("Username or password null");
                return null;
            }
            System.Diagnostics.Debug.WriteLine("User not found null");
            return null;
        }



        public async Task<List<UserModel>> GetUsers()
        {
            return _userManager.Users.Select(u => new UserModel
            {
                Id = Guid.Parse(u.Id),
                Username = u.UserName
            }).ToList();
        }

        public bool ExistsUser(Guid id)
        {
            return _userManager.Users.Any(u => u.Id.Equals(id.ToString()));
        }

        public async Task<ApplicationUser> Delete(Guid id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    System.Diagnostics.Debug.WriteLine("Succes");
                    return user;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Fail");
                    return null;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("User Not Found");
                return null;
            }
        }

        public async Task<UserModel> GetUserAsync(Guid id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            System.Diagnostics.Debug.WriteLine("Succes");
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesList = new List<RoleModel>();
            foreach (var role in userRoles)
            {
                var roleEntity = await _roleManager.FindByNameAsync(role);
                rolesList.Add(new RoleModel
                {
                    Id = roleEntity.Id,
                    RoleName = roleEntity.Name
                });
            }

            return new UserModel
            {
                Roles = rolesList
            };

        }


    }
}
