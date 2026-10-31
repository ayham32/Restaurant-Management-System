
using Azure.Core;
using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.Auth;
using RestaurantShared.Entities;
using RestuarantDataAccess.UintOfWork;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantBusiness.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _token;
        
        private string HashRefreshToken(string refreshToken)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(refreshToken);

            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public AuthService(IUnitOfWork unitOfWork , ITokenService token)
        {
            _unitOfWork = unitOfWork;
            _token = token;
        }

        public async Task<TokenResponse?> Login(LoginRequestDto request)
        {
            if (request == null)
                return null;

            

            var user = await _unitOfWork.usersRepository.GetUserAsync(request.Username);

            
            if (user == null || !user.IsActive)
                return null;


            bool isValidPassword =
               BCrypt.Net.BCrypt.Verify(request.Password, user.HashPassword);


            if (!isValidPassword)
                return null;

            var userRoles = await _unitOfWork.usersRepository.GetUserRoles(user.UserId);

            if (userRoles == null)
                return null;

            var userRolesName = userRoles.Select(ur => ur.Role.RoleName).ToList();


            var token = await _token.GenerateToken(user,userRolesName);

            var refreshToken = await _token.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                UserId = user.UserId,
                RefreshTokenHash = HashRefreshToken(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                RefreshTokenRevokedAt = null
            };

            await _unitOfWork.refreshTokenRepository.AddNewRefreshToken(newRefreshToken);;

            

            return await _unitOfWork.SaveChangesAsync()>0? new TokenResponse 
                                                           {
                                                                AccessToken = token,
                                                                RefreshToken =refreshToken
                                                           }: null!;
        }

        public async Task<bool> Logout(LogoutRequest request)
        {
            var user = await _unitOfWork.usersRepository.GetUserAsync(request.Username);

            if (user == null)
                return false;

            var refreshToken = await _unitOfWork.refreshTokenRepository.GetRefreshToken(HashRefreshToken(request.RefreshToken));
            
            if (refreshToken ==null|| user.UserId != refreshToken.UserId || refreshToken.RefreshTokenRevokedAt != null)
                return false; 

            refreshToken.RefreshTokenRevokedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<TokenResponse> Refresh(RefreshRequest refreshRequest)
        {
            var user = await _unitOfWork.usersRepository.GetUserAsync(refreshRequest.Username);

            if (user == null)
                return null;

            var oldRefreshToken = await _unitOfWork.refreshTokenRepository.GetRefreshToken(HashRefreshToken(refreshRequest.RefreshToken));

            if (oldRefreshToken == null || oldRefreshToken.UserId != user.UserId || oldRefreshToken.RefreshTokenRevokedAt != null || oldRefreshToken.RefreshTokenExpiresAt == null || oldRefreshToken.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return null;

            

            var userRoles = await _unitOfWork.usersRepository.GetUserRoles(user.UserId);

            if (userRoles == null)
                return null;

            var userRolesName = userRoles.Select(ur => ur.Role.RoleName).ToList();


            var token = await _token.GenerateToken(user, userRolesName);

            var refreshToken = await _token.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                UserId = user.UserId,
                RefreshTokenHash =HashRefreshToken(refreshToken),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                RefreshTokenRevokedAt = null
            };


            oldRefreshToken.RefreshTokenRevokedAt = DateTime.UtcNow;

            await _unitOfWork.refreshTokenRepository.AddNewRefreshToken(newRefreshToken);

            return await _unitOfWork.SaveChangesAsync() > 0? new TokenResponse {AccessToken = token , RefreshToken = refreshToken } : null!;

        }
    }
}
