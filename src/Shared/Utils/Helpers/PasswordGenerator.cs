namespace Shared.Utils.Helpers;

public class PasswordGenerator
{
    public static string GenerateStrongPassword(int length = 12)
    {
        if (length < 8)
        {
            throw new ArgumentException("Password length must be at least 8 characters.");
        }

        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+[]{}|;:',.<>?/";

        Random random = new Random();

        // Ensure at least one character from each category
        char[] password = new char[length];
        password[0] = lowerCase[random.Next(lowerCase.Length)];
        password[1] = upperCase[random.Next(upperCase.Length)];
        password[2] = digits[random.Next(digits.Length)];
        password[3] = specialChars[random.Next(specialChars.Length)];

        // Fill the rest of the password with random characters from all categories
        string allChars = lowerCase + upperCase + digits + specialChars;
        for (int i = 4; i < length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        // Shuffle the password to ensure randomness
        return new string(password.OrderBy(x => random.Next()).ToArray());
    }
}