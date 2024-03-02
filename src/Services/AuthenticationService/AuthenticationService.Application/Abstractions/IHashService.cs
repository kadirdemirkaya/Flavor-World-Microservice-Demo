namespace AuthenticationService.Application.Abstractions
{
    public interface IHashService
    {
        string StringHashingEncrypt(string password);
        string StringHashingDecrypt(string encryptPassword);
        string GetSHA256Hash(string input);
        string EncryptString(string plainText, string key);
        string DecryptString(string cipherText, string key);
    }
}
