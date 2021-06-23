using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
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
    public class UserCQRService : IUserCQRService
    {

        private readonly IMediator Mediator;
        private readonly IMapper _Mapper;

        public UserCQRService(IMapper mapper, IMediator mediator)
        {
            Mediator = mediator;
            _Mapper = mapper;
        }

     

        public async Task СreateUser(UserModelDTO user)
        {
            AddUserCommand request = new AddUserCommand() { User = user };
            await Mediator.Send(request);
        }

        public string EncryptionPassword(string password)
        {
            string result;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            return result = Convert.ToBase64String(encode);
        }

        public async Task<UserModelDTO> GetUserBy(string Email)
        {
            GetUserByQueriy request = new GetUserByQueriy() { Email = Email };
             var user = await Mediator.Send(request);
            return user;
        }

        public async Task AdminInitialization()
        {

            throw new Exception();

        }

        public async Task<string> GetUserEmailByTokenKey(string Key)
        {
            GetEmailUserByRefresgTokenKeyQueriy request = new GetEmailUserByRefresgTokenKeyQueriy() { Key = Key };
            string email = await Mediator.Send(request);
            return email;
        }
    }
}
