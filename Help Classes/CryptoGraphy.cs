using Chat_Application.Models;
using System.Security.Cryptography;
using System.Text;

namespace Chat_Application.Help_Classes
{
    public class CryptoGraphy : IDisposable
    {
        readonly AesManaged _algorithm;
        readonly byte[] _salt;
        private readonly string CryptKey = "992142484233823";
        public CryptoGraphy()
        {
            {
                _salt = Convert.FromBase64String("4556484548529632");
                _algorithm = new AesManaged
                {
                    Padding = PaddingMode.Zeros
                };
            }
        }
        public string Encrypter(string json, string salt)
        {
            try
            {
                if (salt == null)
                    salt = CryptKey;
                return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(json), salt));
            }
            catch
            {
                throw new Exception("Error Code 5.3 - Error at Helper clases Encryption Error");
            }
        }

        public async Task<string> Decrypter(string json, string salt)
        {
            try
            {
                if (salt == null)
                    salt = CryptKey;

                var result = Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(json), salt));
                return await Task.FromResult(result);
            }
            catch
            {
                throw new Exception("Error Code 5.4 - Error at Helper clases Decryption Error");
            }
        }


        //Encryption Task, used to encrypt data, using existing salt code
        public byte[] Encrypt(byte[] bytesToEncrypt, string pass)
        {
            var passwordHash = GeneratePasswordHash(pass);
            var key = GenerateKey(passwordHash);
            var IV = GenerateIV(passwordHash);
            ICryptoTransform Encryption = _algorithm.CreateEncryptor(key, IV);
            return TransformBytes(Encryption, bytesToEncrypt);
        }

        //Decryption Task, used to decrypt data, using existing salt code
        public byte[] Decrypt(byte[] bytesToDecrypt, string pass)
        {
            var passwordHash = GeneratePasswordHash(pass);
            var key = GenerateKey(passwordHash);
            var IV = GenerateIV(passwordHash);
            var decrypt = _algorithm.CreateDecryptor(key, IV);
            return TransformBytes(decrypt, bytesToDecrypt);
        }

        //used to create password hashed key
        private Rfc2898DeriveBytes GeneratePasswordHash(string pass) =>
            new Rfc2898DeriveBytes(pass, _salt);

        //converts password key to bytes
        private byte[] GenerateKey(Rfc2898DeriveBytes passwordHash) =>
            passwordHash.GetBytes(_algorithm.KeySize / 8);

        //Generates initialization vector (IV)
        private byte[] GenerateIV(Rfc2898DeriveBytes passwordHash) =>
            passwordHash.GetBytes(_algorithm.BlockSize / 8);

        // Writes the decrypted / encrypted version to an array, then returns result
        private byte[] TransformBytes(ICryptoTransform transformer, byte[] bytesToTransform)
        {
            byte[] contentResult;
            using (var bufferstream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(bufferstream, transformer, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytesToTransform, 0, bytesToTransform.Length);
                    cryptoStream.FlushFinalBlock();
                    contentResult = bufferstream.ToArray();
                    cryptoStream.Close();
                }
                bufferstream.Close();
            }
            return contentResult;
        }
        protected virtual void Dispose(bool isDisposin)
        {
            if (isDisposin)
                _algorithm.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class Salting
    {
        public static byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(RandomNumberGenerator.GetInt32(20, 25));
        }

        public static SaltHash HashSalt(string pass, byte[] saltBytes)
        {
            try
            {
                if (saltBytes == null || saltBytes.Length == 0)
                    saltBytes = GenerateSalt();

                //Converts passwords to bytes
                byte[] passBytes = Encoding.UTF8.GetBytes(pass);

                //Makes new empty byte with lenght of the salt and pass combined
                byte[] passSaltBytes = new byte[passBytes.Length + saltBytes.Length];

                //Adds password bytes to the result byte array
                for (int i = 0; i < passBytes.Length; i++)
                    passSaltBytes[i] = passBytes[i];

                //Adds the salt bytes to the resulting byte array
                for (int i = 0; i < saltBytes.Length; i++)
                    passSaltBytes[i + passBytes.Length] = saltBytes[i];

                //computes password bytes with salt bytes with sha256
                HashAlgorithm hash = new SHA256Managed();
                byte[] Hashing = hash.ComputeHash(passSaltBytes);

                byte[] ResultHashSalt = new byte[Hashing.Length + saltBytes.Length];

                //fills the result byte with hashing(pass) bytes 
                for (int i = 0; i < Hashing.Length; i++)
                    ResultHashSalt[i] = Hashing[i];

                //fills the result byte with salting bytes after pass bytes
                for (int i = 0; i < saltBytes.Length; i++)
                    ResultHashSalt[i + Hashing.Length] = saltBytes[i];

                //returns readable Base64String of the result byte
                return new SaltHash(Convert.ToBase64String(ResultHashSalt), Convert.ToBase64String(saltBytes));
            }
            catch
            {
                throw new Exception("Error Code 5.2 - Error at Helper clases Salitng(hash) Error");
            }
        }
    }
}
