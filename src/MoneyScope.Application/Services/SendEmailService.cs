using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Core.Enums.SendEmail;
using MoneyScope.Core.Enums.User;
using MoneyScope.Core.Models;
using MoneyScope.Core.Utils;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class SendEmailService : BaseService, ISendEmailService
    {
        private readonly ITokenService _tokenService;
        private readonly SmtpConfig _configSmtp;
        private IHostingEnvironment _environment;
        public SendEmailService(IRepositoryFactory repositoryFactory, ITokenService tokenService, IOptions<SmtpConfig> configSmtp, IHostingEnvironment environment) : base(repositoryFactory)
        {
            _tokenService = tokenService;
            _configSmtp = configSmtp.Value;
            _environment = environment;
        }
        private string GetBodyResetPassword(string token, ERedefinitionEmailType type = ERedefinitionEmailType.RequestToResetPassword, string nome = "")
        {
            var path = Path.Combine(_environment.WebRootPath, $"MailTemplates/{"RedefinirSenha"}.html");
            var body = File.ReadAllText(path);

            string titulo = "";
            string texto1 = "";
            string texto2 = "";

            switch (type)
            {
                case ERedefinitionEmailType.RequestToResetPassword:
                    titulo = "Solicitação de Redefinição de Senha";
                    texto1 = $"Olá, {nome}!<br><br> Recebemos uma solicitação para redefinir sua senha de acesso ao MoneyScope.<br>Para continuar, clique no botão abaixo:";
                    texto2 = "Se você não fez essa solicitação, apenas ignore este e-mail. Sua conta permanecerá segura.";
                    break;
                case ERedefinitionEmailType.Unsubscribe:
                    titulo = "Cancelamento de inscricão";
                    texto1 = $"Olá, {nome}!<br><br> Sua inscrição foi cancelada com sucesso.";
                    texto2 = "Se você não fez essa solicitação, apenas ignore este e-mail. Sua conta permanecerá segura.";
                    break;
                default:
                    break;
            }

            var variables = new Dictionary<string, string>
            {
                { "#URLLOGO", _configSmtp.UrlLogo },
                { "#TituloEmail", titulo },
                { "#TEXTO1", texto1 },
                { "#URLREDEFINIR", $"{_configSmtp.UrlRedefinicao}/{token}" },
                { "#TEXTO2", texto2 }
            };

            foreach (var item in variables)
                body = body.Replace(item.Key, item.Value);

            return body;
        }
        public async Task<ResponseModel<dynamic>> ResetPassword(string token, string newPassword)
        {
            var isvalid = _tokenService.IsValidToken(token);
            if (!isvalid) return FactoryResponse<dynamic>.Forbiden("Token invalido");

            var login = _tokenService.Getclaim("unique_name", token);
            if (login is null) return FactoryResponse<dynamic>.Forbiden("Token invalido");

            var user = await _repository<User>().Get(x => (x.Email.Replace(" ", "") == login.Replace(" ", "")));
            if (user is null) return FactoryResponse<dynamic>.NotFound("Usuário não encontrado!");

            user.Status = EUserStatus.Ativo;
            user.Password = HashHelper.HashGeneration(newPassword);
            //       if (!usuario.IsValid) return FactoryResponse<dynamic>.InvalidModel(null, usuario.ValidationErrors);

            try
            {
                await _repository<User>().Update(user);
            }
            catch (Exception e)
            {
                return FactoryResponse<dynamic>.BadRequest($"erro interno: " + e.Message);
            }

            //var retornoToken = await GetToken(usuario, null, 86400);

            return FactoryResponse<dynamic>.Success("Senha definida com sucesso!");
        }
        public async Task<bool> SendEmail(SendEmailModel model, ERedefinitionEmailType type, string name)
        {
            try
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(_configSmtp.EmailFrom, _configSmtp.Password),
                    EnableSsl = true, // Gmail exige STARTTLS
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var body = GetBodyResetPassword(model.Content, type, name);

                using var message = new MailMessage
                {
                    From = new MailAddress(_configSmtp.EmailFrom, _configSmtp.NameFrom, Encoding.UTF8),
                    Subject = model.Subject,
                    Body = body,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                message.To.Add(model.To);

                await client.SendMailAsync(message);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP error: {ex.Message}");
                Console.WriteLine($"Inner: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                Console.WriteLine($"Inner: {ex.InnerException?.Message}");
                return false;
            }
        }
        public async Task<ResponseModel<dynamic>> SendEmailResetPassword(string email, ERedefinitionEmailType type = ERedefinitionEmailType.RequestToResetPassword)
        {
            var usuario = await _repository<User>().Get(x => (x.Email != null && (x.Email.ToLower().Replace(" ", "") == email.ToLower().Replace(" ", ""))));
            if (usuario is null) return FactoryResponse<dynamic>.NotFound("Usuario não encontrado!");

            var tokenEmail = await _tokenService.GenerateTokenByEmail(email);

            var sendEmail = await SendEmail(new SendEmailModel("Envio de senha", usuario.Email, tokenEmail.Data!), type, usuario.Name);
            if (!sendEmail) return FactoryResponse<dynamic>.BadRequest("Erro ao Enviar Email!");
            return FactoryResponse<dynamic>.Success("Email enviado com sucesso!");
        }
    }
}
