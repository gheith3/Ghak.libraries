using System.Reflection;
using System.Text.RegularExpressions;
using Ghak.libraries.AppBase.Exceptions;

namespace Ghak.libraries.AppBase.Utils;

public static partial class Helpers
{
    /**
     * <summary>
     * This method is used to generate a random string key
     * </summary>
     * <param name="prefix">string that put before the generated key</param>
     * <param name="id">string (default: null), the method will generate a new guid if null or empty</param>
     * <returns>
     *   string key with the following format: prefix-guid or prefix-id if id is not null
     * </returns>
     */
    public static string GetStringKey(string prefix = "", string id = "")
    {
        if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith("-"))
        {
            prefix = $"{prefix}-";
        }

        return prefix +
               (string.IsNullOrEmpty(id)
                   ? Guid.NewGuid().ToString()
                   : id);
    }

    /**
     <summary>
        * This method is used to generate a random number between 10000 and 99999
     </summary>
         <param name="prefix">prefix string</param>
         <param name="isTest">bool (default: false) if true, the method will return 00000 as otp code</param>
     * <returns>string -> otp code</returns>
     */
    public static string GetOtpCode(string prefix = "", bool isTest = false)
    {
        return isTest
            ? "0000"
            : prefix + new Random().Next(1000, 9999);
    }
    
    /// <summary>
    /// Validates and sanitizes the given username by removing leading/trailing spaces, spaces within the username, and the "@" symbol.
    /// </summary>
    /// <param name="username">The username to be validated and sanitized.</param>
    /// <returns>A sanitized username if it passes validation; otherwise, returns null.</returns>
    public static string? CheckUserName(string username)
    {
        username = username.Trim()
            .Replace(" ", "")
            .Replace("@", "");

        var regex = @"^[a-zA-Z0-9]*$";
        var match = Regex.Match(username, regex, RegexOptions.IgnoreCase);
        
        return match.Success
            ? $"@{username}"
            : null;
    }
    
    public  static void CheckRequestId(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new AppException("Id Is Required",
                404,
                nameof(id));
        }
    }
}