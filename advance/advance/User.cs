using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Task> Tasks { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        Tasks = new List<Task>();
    }
}

public static class UserAuth
{
    private static List<User> Users = TaskManager.LoadUsersFromFile("Users.txt");

    public static bool Authenticate(string inputUsername, string inputPassword)
    {
        return Users.Any(user => user.Username == inputUsername && PasswordHasher.VerifyPassword(user.Password, inputPassword));
    }

    public static bool SignUp(string newUsername, string newPassword)
    {
        if (Users.Any(user => user.Username == newUsername))
        {
            return false;
        }

        string hashedPassword = PasswordHasher.HashPassword(newPassword);
        User newUser = new User(newUsername, hashedPassword);
        Users.Add(newUser);
        TaskManager.SaveUsersToFile("Users.txt", Users);
        return true;
    }
}



public static class PasswordHasher
{
    // This method hashes a password using PBKDF2 with SHA256 and returns the hashed password as a hex string.
    public static string HashPassword(string password)
    {
        // Generate a 128-bit salt using a cryptographic random number generator.
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive a 256-bit subkey (hash) using PBKDF2 with SHA256, the generated salt, and 100,000 iterations.
        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
        {
            hash = pbkdf2.GetBytes(32);
        }

        // Combine the salt and the hash into a single byte array.
        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        // Convert the byte array to a hex string and return it.
        string savedPasswordHash = HexEncoding.ToHexString(hashBytes);
        return savedPasswordHash;
    }

    // This method verifies if the input password matches the hashed password.
    public static bool VerifyPassword(string savedPasswordHash, string inputPassword)
    {
        if (string.IsNullOrEmpty(savedPasswordHash))
        {
            throw new ArgumentException("Invalid saved password hash");
        }

        byte[] hashBytes;
        try
        {
            // Convert the saved password hash from hex string to byte array.
            hashBytes = HexEncoding.FromHexString(savedPasswordHash);
        }
        catch (Exception)
        {
            throw new ArgumentException("Invalid HexBug string for saved password hash");
        }

        if (hashBytes.Length != 48)
        {
            throw new ArgumentException("Invalid length of the hexbug");
        }

        // Extract the salt from the first 16 bytes of the hashBytes.
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Derive the hash from the input password using the extracted salt and the same parameters.
        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, 100000, HashAlgorithmName.SHA256))
        {
            hash = pbkdf2.GetBytes(32);
        }

        // Compare the derived hash with the saved hash.
        for (int i = 0; i < 32; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false; // Return false if any byte doesn't match.
            }
        }
        return true;
    }
}

public static class HexEncoding
{
    // This method converts a byte array to a hex string.
    public static string ToHexString(byte[] bytes)
    {
        char[] c = new char[bytes.Length * 2];
        byte b;
        for (int i = 0; i < bytes.Length; i++)
        {
            b = ((byte)(bytes[i] >> 4));
            c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
            b = ((byte)(bytes[i] & 0xF));
            c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
        }
        return new string(c);
    }

    // This method converts a hex string to a byte array.
    public static byte[] FromHexString(string hex)
    {
        if (hex.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid hexbug string length");
        }

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

}
