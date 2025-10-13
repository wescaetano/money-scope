using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Token;
using MoneyScope.Application.Models.User;
using MoneyScope.Core.Enums.SendEmail;
using MoneyScope.Core.Enums.User;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IBlobService _blobService;
        private readonly ISendEmailService _sendEmailService;
        public UserService(IRepositoryFactory repositoryFactory, ITokenService tokenService, IBlobService blobService, ISendEmailService sendEmailService) : base(repositoryFactory)
        {
            _tokenService = tokenService;
            _blobService = blobService;
            _sendEmailService = sendEmailService;
        }
        public async Task<ResponseModel<dynamic>> Add(CreateUserModel model)
        {
            var emailExists = await _repository<User>().Get(x => x.Email.ToLower() == model.Email.ToLower());
            if(emailExists != null) return FactoryResponse<dynamic>.Conflict("Já existe um usuário cadastrado com este e-mail!");

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Status = EUserStatus.Ativo,
                CreationDate = DateTime.UtcNow
            };

            // Save the image to blob
            if (!string.IsNullOrWhiteSpace(model.ImageBase64))
            {
                var fileName = $"{Guid.NewGuid()}_{model.Name.Replace(" ", "_")}.png";
                var imageUrl = await _blobService.UploadBase64Async(model.ImageBase64, fileName);
                user.ImageUrl = imageUrl;
            }

            user.ProfilesUsers.Add(new Domain.AccessProfile.ProfileUser { ProfileId = model.AccessProfile });

            try
            {
                await _repository<User>().Create(user);
                await _sendEmailService.SendEmailResetPassword(user.Email, ERedefinitionEmailType.RequestToResetPassword);

                return FactoryResponse<dynamic>.SuccessfulCreation("Usuário cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno(ex.Message);
            }
        }
        public async Task<ResponseModel<dynamic>> Update(UpdateUserModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<dynamic>> ChangeStatus(ChangeUserStatusModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<dynamic>> GetById(long id)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(UserFilterModel filter)
        {
            throw new NotImplementedException();
        }


        // User Authentication Methods
        public async Task<ResponseModel<dynamic>> AuthenticateSocialUser(LoginModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<dynamic>> AuthenticateUser(LoginModel model)
        {
            throw new NotImplementedException();
        }   
        public async Task<ResponseModel<dynamic>> SocialLogin(SocialLoginModel model)
        {
            throw new NotImplementedException();
        }
      
    }
}
