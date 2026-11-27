using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Authentication;
using KooliProjekt.Application.Authentication.Jwt;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Authentication
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, OperationResult<string>>
    {
        private const string InvalidCredentialsError = "Invalid e-mail or password.";

        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenBuilder _tokenBuilder;

        public AuthenticateCommandHandler(ApplicationDbContext dbContext, JwtTokenBuilder tokenBuilder)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenBuilder = tokenBuilder ?? throw new ArgumentNullException(nameof(tokenBuilder));
        }

        public async Task<OperationResult<string>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new OperationResult<string>();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                response.AddError(InvalidCredentialsError);
                return response;
            }

            var user = await _dbContext
                .Customers
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                response.AddError(InvalidCredentialsError);
                return response;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            // Admin kasutaja saab tokenisse rolli claimi
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            response.Value = _tokenBuilder.GenerateToken(claims.ToArray());

            return response;
        }

        // Vanad seed-andmed võivad sisaldada mitte-räsitud paroole, need ei ole kehtiv Base64 -> lihtsalt vale parool
        private static bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash))
            {
                return false;
            }

            try
            {
                return PasswordHelper.Verify(password, passwordHash);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
