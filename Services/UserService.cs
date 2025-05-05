using OpenEcologyApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using OpenEcologyApp.Shared;
using Microsoft.AspNetCore.Components;

namespace OpenEcologyApp.Services
{
    public class UserService
    {
        private readonly EcologyDbContext _context;
        private readonly NavigationManager _navigationManager;

        public UserService(EcologyDbContext context, NavigationManager navigationManager)
        {
            _context = context;
            _navigationManager = navigationManager;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);
        }

        public async Task<bool> RegisterUser(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return false;
            }

            user.Password = HashPassword(user.Password);
            user.IsAdmin = false; // По умолчанию пользователь не является администратором
            user.Status = UserStatus.Active; // По умолчанию пользователь активен

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    Status = u.Status
                })
                .ToListAsync();
        }

        public async Task<List<User>> GetFilteredUsers(string searchTerm, bool? isAdmin, UserStatus? status)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(u => 
                    u.UserName.ToLower().Contains(searchTerm) || 
                    u.Email.ToLower().Contains(searchTerm));
            }

            if (isAdmin.HasValue)
            {
                query = query.Where(u => u.IsAdmin == isAdmin.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(u => u.Status == status.Value);
            }

            return await query
                .Select(u => new User
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    Status = u.Status
                })
                .ToListAsync();
        }

        public async Task<bool> ToggleUserStatus(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Status = user.Status == UserStatus.Active ? UserStatus.Blocked : UserStatus.Active;
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            if (!NavMenu.IsAuthenticated || NavMenu.CurrentUserId == null) return null;
            return await _context.Users.FindAsync(NavMenu.CurrentUserId);
        }

        public async Task<bool> UpdateUserProfile(string userName, string email)
        {
            if (!NavMenu.IsAuthenticated || NavMenu.CurrentUserId == null) return false;

            var user = await _context.Users.FindAsync(NavMenu.CurrentUserId);
            if (user == null) return false;

            // Проверяем, не занят ли email другим пользователем
            if (await _context.Users.AnyAsync(u => u.Email == email && u.UserId != NavMenu.CurrentUserId))
            {
                return false;
            }

            user.UserName = userName;
            user.Email = email;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            if (!NavMenu.IsAuthenticated || NavMenu.CurrentUserId == null) return false;

            var user = await _context.Users.FindAsync(NavMenu.CurrentUserId);
            if (user == null) return false;

            var hashedOldPassword = HashPassword(oldPassword);
            if (user.Password != hashedOldPassword)
            {
                return false;
            }

            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 