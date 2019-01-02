using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SparShaMap.DataService;
using SparShaMap.Models;

namespace SparShaMap.AuthService
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly DataBaseService _db = new DataBaseService();
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var result = _db.SelectQueryNoAsync("SELECT [UID],[USER_ID],[USER_PASS],[USER_FULLNAME]" +
                        "FROM[M_USER]" +
                        "WHERE[USER_ID] = '"+ username + "'").FirstOrDefault();
            //TODO to Models
            var id= result["UID"].ToString();
            var user = result["USER_ID"].ToString();
            var passwordDataBase= result["USER_PASS"].ToString();
            var fullName = result["USER_FULLNAME"].ToString();
            var flag_login = Sodium.PasswordHash.ScryptHashStringVerify(passwordDataBase, password);
            if (flag_login)
            {
                return new User
                {
                    Id =id,
                    Username = user,
                    FullName= fullName
                };

            }
            else
            {
                return null;
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            try
            {
                // await _context.Users.AddAsync(user);
                // await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                //if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
