using Microsoft.EntityFrameworkCore;
using MoneyScope.Application.Filters.User;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Token;
using MoneyScope.Application.Models.User;
using MoneyScope.Core.Enums.SendEmail;
using MoneyScope.Core.Enums.User;
using MoneyScope.Core.Models;
using MoneyScope.Core.Utils;
using MoneyScope.Domain;
using MoneyScope.Domain.AccessProfile;
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
        private readonly IAuthService _authService;
        private readonly IBlobService _blobService;
        public UserService(IRepositoryFactory repositoryFactory, ITokenService tokenService, IBlobService blobService, IAuthService authService) : base(repositoryFactory)
        {
            _tokenService = tokenService;
            _blobService = blobService;
            _authService = authService;
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

            var perfilExiste = await _repository<Profile>().Get(p => p.Id == model.AccessProfile);
            if (perfilExiste == null) return FactoryResponse<dynamic>.NotFound("Perfil de acesso não encontrado.");
            user.ProfilesUsers.Add(new Domain.AccessProfile.ProfileUser { ProfileId = model.AccessProfile });

            try
            {
                await _repository<User>().Create(user);
                await _authService.SendEmailResetPassword(user.Email, ERedefinitionEmailType.RequestToResetPassword);

                return FactoryResponse<dynamic>.SuccessfulCreation("Usuário cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno(ex.Message);
            }
        }
        public async Task<ResponseModel<dynamic>> Update(UpdateUserModel model)
        {
            var user = await _repository<User>().GetWithInclude(x => x.Id == model.Id, i => i.Include(pu => pu.ProfilesUsers).ThenInclude(p => p.Profile));
            if (user == null) return FactoryResponse<dynamic>.NotFound("Usuário não encontrado!");

            if (model.AccessProfile != null)
            {
                var perfilExiste = await _repository<Profile>().Get(p => p.Id == model.AccessProfile);
                if (perfilExiste == null) return FactoryResponse<dynamic>.NotFound("Perfil de acesso não encontrado.");

                var profilesToRemove = user.ProfilesUsers.ToList();

                foreach (var profile in profilesToRemove)
                {
                    await _relationRepository<ProfileUser>().Remove(profile);
                }

                user.ProfilesUsers.Add(new ProfileUser { ProfileId = model.AccessProfile.Value });
            }

            if (model.Name != null) user.Name = model.Name;
            if(model.Email != null) user.Email = model.Email;

            // Save the image to blob
            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                var fileName = $"{Guid.NewGuid()}_{model.Name.Replace(" ", "_")}.png";
                var imageUrl = await _blobService.UploadBase64Async(model.ImageUrl, fileName);
                user.ImageUrl = imageUrl;
            }

            try
            {
                await _repository<User>().Update(user);
                return FactoryResponse<dynamic>.Success("Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno(ex.Message);
            }

        }
        public async Task<ResponseModel<dynamic>> ChangeStatus(ChangeUserStatusModel model)
        {
            var user = await _repository<User>().Get(model.Id);
            if (user == null) return FactoryResponse<dynamic>.NotFound("Usuário não encontrado!");
            
            user.Status = model.Status;

            try
            {
                await _repository<User>().Update(user);
                return FactoryResponse<dynamic>.Success("Status do usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return FactoryResponse<dynamic>.BadRequestErroInterno(ex.Message);
            }
        }
        public async Task<ResponseModel<dynamic>> GetById(long id)
        {
            var user = await _repository<User>().GetWithInclude(x => x.Id == id, i => i.Include(pu => pu.ProfilesUsers));
            if (user == null) return FactoryResponse<dynamic>.NotFound("Usuário não encontrado!");

            var retrieve = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.ImageUrl,
                user.Status,
                Profiles = user.ProfilesUsers.Select(pu => new
                {
                    pu.Profile.Id,
                    pu.Profile.Name,
                })
            };

            return FactoryResponse<dynamic>.Success(retrieve);
        }
        public async Task<ResponseModel<PaginationData<dynamic>>> GetPaginated(UserFilterModel filter)
        {
            var usersQuery = _repository<User>().GetAllWithInclude(null, null);

            if (filter.Id != null) usersQuery = usersQuery.Where(x => x.Id == filter.Id);
            if (filter.Name != null) usersQuery = usersQuery.Where(x => x.Name == filter.Name);
            if (filter.Email != null) usersQuery = usersQuery.Where(x => x.Email == filter.Email);

            var filteredUsers = await usersQuery.ToListAsync();
            var total = filteredUsers.Count();

            switch (filter.SortField?.ToLower())
            {
                case "name":
                    filteredUsers = filter.SortOrder?.ToLower() != "desc"
                        ? filteredUsers.OrderBy(x => x.Name).ToList()
                        : filteredUsers.OrderByDescending(x => x.Name).ToList();
                    break;

                case "email":
                    filteredUsers = filter.SortOrder?.ToLower() != "desc"
                        ? filteredUsers.OrderBy(x => x.Email).ToList()
                        : filteredUsers.OrderByDescending(x => x.Email).ToList();
                    break;

                default:
                    filteredUsers = filter.SortOrder?.ToLower() != "desc"
                        ? filteredUsers.OrderBy(x => x.Id).ToList()
                        : filteredUsers.OrderByDescending(x => x.Id).ToList();
                    break;
            }

            var retorno = filteredUsers;
            if (filter.Id == null && filter.PageNumber != null && filter.PageSize != null)
            {
                retorno = filteredUsers.Skip(filter.PageSize.Value * (filter.PageNumber.Value - 1)).Take(filter.PageSize.Value).ToList();
            }

            var usersRetrieve = retorno.Select(x => new
            {
                x.Id,
                x.Name,
                x.Email,
                x.ImageUrl,
                x.Status
            }).ToList();
            var paginationData = new PaginationData<dynamic>(usersRetrieve, total, filter);
            return FactoryResponse<PaginationData<dynamic>>.Success(paginationData);
        }
    }
}
