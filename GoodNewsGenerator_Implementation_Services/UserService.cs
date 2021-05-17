using AutoMapper;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;



namespace GoodNewsGenerator_Implementation_Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork DbContext;
        private readonly IRoleService RoleService;
        private readonly IConfiguration Configuration;
        private readonly IMapper _Mapper;

        public UserService(IUnitOfWork dbContext , IMapper mapper, IRoleService roleService, IConfiguration configuration)
        {
            DbContext = dbContext;
            RoleService = roleService;
            Configuration = configuration;
            _Mapper = mapper;
        }

        public IEnumerable<UserModelDTO> GetAllUser()
        {
            IQueryable<User> AllUser = DbContext.User.GetAllEntity(el=>el.Roles != null , elm=>elm.Roles);
            IEnumerable<UserModelDTO> user = AllUser.Select(el => _Mapper.Map<UserModelDTO>(el));
            return user;
        }

        public UserModelDTO GetUserBy(string Email)
        {
            IQueryable<User> User = DbContext.User.GetAllEntity(x => x.Email == Email, i => i.Roles);
            List<User> Users = User.ToList();
            UserModelDTO user = _Mapper.Map<UserModelDTO>(Users.FirstOrDefault());
            return user;
        }

        public async Task RemoveUser(Guid id)
        {
            User user = DbContext.User.GetById(id).FirstOrDefault();
            await DbContext.User.Remove(user.Id);
            await DbContext.SaveChangesAsync();
        }

        public async Task AdminInitialization()
        {
        
            var Email = Configuration.GetValue<string>("Admin:Email");

            IEnumerable<RoleModelDTO> UserRole = RoleService.GetAllUserWithRole().Result;
            RoleModelDTO Admin = UserRole.FirstOrDefault(el => el.NameRole == "Admin");

            UserModelDTO UserWitRepositori = GetUserBy(Email);
            UserWitRepositori.RoleId = Admin.Id;
            

            if (UserWitRepositori != null) 
            {
                
                User ContextUser = _Mapper.Map<User>(UserWitRepositori);

                DbContext.User.UpdateUser(ContextUser);
                await DbContext.SaveChangesAsync();
            }
            
        }

        public async Task СreateUser(UserModelDTO user)
        {
                    User ContextUser = _Mapper.Map<User>(user);
                    await DbContext.User.Add(ContextUser);
                    await DbContext.SaveChangesAsync();
        }

        public string EncryptionPassword(string password)
        {
            string result;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            return result = Convert.ToBase64String(encode);
        }
    }
}
