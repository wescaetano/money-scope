using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.SendEmail;
using MoneyScope.Application.Models.Token;
using MoneyScope.Application.Services;
using MoneyScope.Core.Models;
using MoneyScope.Domain;
using MoneyScope.Infra.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Tests
{
    public class AuthTests
    {
        private readonly Mock<IRepositoryFactory> _repositoryFactoryMock;
        private readonly Mock<IBaseRepository<User>> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IOptions<SmtpConfig>> _smtpOptionsMock;
        private readonly Mock<IHostingEnvironment> _hostingEnvironmentMock;

        private readonly IAuthService _authService;

        public AuthTests()
        {
            _repositoryFactoryMock = new Mock<IRepositoryFactory>();
            _userRepositoryMock = new Mock<IBaseRepository<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _smtpOptionsMock = new Mock<IOptions<SmtpConfig>>();
            _hostingEnvironmentMock = new Mock<IHostingEnvironment>();

            _smtpOptionsMock
                .Setup(x => x.Value)
                .Returns(new SmtpConfig());

            _repositoryFactoryMock
                .Setup(x => x.GetRepository<User>())
                .Returns(_userRepositoryMock.Object);

            _authService = new AuthService(
                _repositoryFactoryMock.Object,
                _tokenServiceMock.Object,
                _smtpOptionsMock.Object,
                _hostingEnvironmentMock.Object
            );
        }

        [Fact]
        public async Task Deve_autenticar_usuario_com_sucesso()
        {
            // Arrange
            var login = new LoginModel
            {
                Login = "usuario_teste",
                Password = "senha_teste"
            };

            var senhaDigitada = "senha_teste";

            var usuario = new User
            {
                Id = 1,
                Name = "Usuario Teste",
                Email = "usuario_teste",
                Password = BCrypt.Net.BCrypt.HashPassword(senhaDigitada)
            };


            _userRepositoryMock
            .Setup(x => x.GetWithInclude(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
            .ReturnsAsync(usuario);


            _tokenServiceMock
             .Setup(x => x.GenerateToken(It.IsAny<GenerateTokenModel>()))
             .ReturnsAsync(FactoryResponse<dynamic>.Success(new
             {
                 AccessToken = "TOKEN_FAKE"
             }));


            // Add mock for GetToken which is used by AuthService.AuthenticateUser
            _tokenServiceMock
             .Setup(x => x.GetToken(It.IsAny<User>(), It.IsAny<int?>()))
             .ReturnsAsync(FactoryResponse<dynamic>.Success(new
             {
                 AccessToken = "TOKEN_FAKE"
             }));


            // Act
            var result = await _authService.AuthenticateUser(login);


            // Assert
            var data = result.Data;

            ((object)data).Should().NotBeNull();
            var accessToken = ((dynamic)data).AccessToken as string;
            accessToken.Should().Be("TOKEN_FAKE");
        }
    }
}
