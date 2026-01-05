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
            var user = await _repository<User>().Get(model.Id);
            if (user == null) return FactoryResponse<dynamic>.NotFound("Usuário não encontrado!");

            if(model.AccessProfile != null) 
            {
                user.ProfilesUsers.Clear();
                user.ProfilesUsers.Add(new Domain.AccessProfile.ProfileUser { ProfileId = model.AccessProfile.Value });
            }
            if(model.Name != null) user.Name = model.Name;
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
            //var prop = typeof(User).GetProperties().FirstOrDefault(x => x.Name.ToLower() == filter.SortField.ToLower());

            //if (filter.SortOrder.ToLower() != "desc")
            //{
            //    filteredUsers = filteredUsers.OrderBy(x => prop == null ? "Id" : prop.GetValue(x, null)).ToList();
            //}
            //else
            //{
            //    filteredUsers = filteredUsers.OrderByDescending(x => prop == null ? "Id" : prop.GetValue(x, null)).ToList();
            //}

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


        // User Authentication Methods
        public async Task<ResponseModel<dynamic>> AuthenticateSocialUser(LoginModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<dynamic>> AuthenticateUser(LoginModel model)
        {
            var user = _repository<User>().GetAllWithInclude(x => x.Email != null && x.Email == model.Login, setIncludes: query => query.Include(u => u.ProfilesUsers).ThenInclude(up => up.Profile)).FirstOrDefault();

            if (user == null || string.IsNullOrEmpty(user.Password))
                throw new Exception("Usuário não encontrado ou sem senha definida.");

            if (string.IsNullOrEmpty(model.Password))
                throw new Exception("Senha não informada.");

            if (!HashHelper.PasswordCompare(user.Password, model.Password))
                throw new Exception("Senha incorreta.");

            if (user.Status == EUserStatus.Inativo) return FactoryResponse<dynamic>.BadRequest("Acesso bloqueado por inativação da conta.");

            if (!HashHelper.PasswordCompare(user.Password!, model.Password)) return FactoryResponse<dynamic>.BadRequest("Senha Incorreta!");

            ResponseModel<dynamic> retorno = await GetToken(user);

            return retorno;
        }
        private async Task<ResponseModel<dynamic>> GetToken(User user, int? seconds = null)
        {
            var userProfilesQuery = _relationRepository<ProfileUser>().GetAllWithInclude(x => x.UserId == user.Id, x => x.Include(y => y.Profile).ThenInclude(y => y.ProfilesModules).ThenInclude(y => y.Module));
            var accessProfiles = await userProfilesQuery.ToListAsync();
            if (accessProfiles == null || accessProfiles.Count() == 0) return FactoryResponse<dynamic>.Unauthorized("Nenhum Perfil Atribuido ao usuario");

            var functionalities = accessProfiles.SelectMany(x => x.Profile.ProfilesModules).ToList();
            var permissionsFunctionalities = GetUserFunctionalities(functionalities);
            var functionalitiesAssembledModel = GetModulesModelUserAssembled(functionalities);
            if (!permissionsFunctionalities.Any()) return FactoryResponse<dynamic>.Unauthorized("Nenhum Perfil Atribuido ao usuario");

            var tokenModel = new Models.Token.GenerateTokenModel
            {
                Modules = permissionsFunctionalities,
                ModulesAssembled = functionalitiesAssembledModel,
                UserEmail = user.Email,
                UserProfileImage = user.ImageUrl,
                UserId = user.Id,
                UserName = user.Name,
                Seconds = seconds
            };

            var token = await _tokenService.GerenerateToken(tokenModel);

            return token;
        }
        private List<string> GetUserFunctionalities(List<ProfileModule> profileModules)
        {
            var functionality = new HashSet<string>();
            foreach (var f in profileModules)
            {
                var ModuleName = f.Module.Name.Trim();

                if (f.Register) functionality.Add($"{ModuleName}-C");
                if (f.Edit) functionality.Add($"{ModuleName}-E");
                if (f.Visualize) functionality.Add($"{ModuleName}-V");
                if (f.Inactivate) functionality.Add($"{ModuleName}-I");
                if (f.Exclude) functionality.Add($"{ModuleName}-EX");
            }
            return functionality.ToList();
        }
        private ModulesModel GetModulesModelUserAssembled(List<ProfileModule> profileModules)
        {
            var ModuleModel = new ModulesModel() { ModuleProfileUser = new List<ModulesModel.ModuleProfileUserModel>() };
            foreach (var f in profileModules)
            {
                var ModuleName = f.Module.Name.Trim();
                var ModuleProfileUser = new ModulesModel.ModuleProfileUserModel();
                ModuleProfileUser.Name = ModuleName;
                ModuleProfileUser.IdModule = f.ModuleId;

                if (f.Register) ModuleProfileUser.Register = true;

                if (f.Edit) ModuleProfileUser.Edit = true;
                if (f.Visualize) ModuleProfileUser.Visualize = true;
                if (f.Inactivate) ModuleProfileUser.Inactivate = true;
                if (f.Exclude) ModuleProfileUser.Exclude = true;

                ModuleModel.ModuleProfileUser.Add(ModuleProfileUser);
            }
            ModuleModel.InfoProfile = new ModulesModel.InfoProfileModel
            {
                IdProfile = profileModules.FirstOrDefault()?.ProfileId,
                Name = profileModules.FirstOrDefault()?.Profile?.Name
            };
            return ModuleModel;
        } 
    }
}
