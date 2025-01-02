using System;
using System.Text.RegularExpressions;
using SprintBoard.DTOs;

namespace SprintBoard.BLL.Extensions
{
    public static class UserExtensions
    {
        public static bool IsValid(this UserDto user, out string errorMessage)
        {
            if (user == null)
            {
                errorMessage = "User cannot be null.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                errorMessage = "Username is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                errorMessage = "Email is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                errorMessage = "Password is required.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool IsPasswordComplex(this string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return regex.IsMatch(password);
        }
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }

    }
}
