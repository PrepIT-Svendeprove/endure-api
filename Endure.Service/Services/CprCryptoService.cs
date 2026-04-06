using Endure.Service.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Endure.Service.Services;

internal class CprCryptoService : ICprCryptoService
{
    private readonly Lazy<byte[]> _key;
    private const string CPR_REGEX = "^[0-9]{10}$";

    public CprCryptoService(IOptions<CprHashingOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var keypath = options.Value.KeyPath;

        if (string.IsNullOrEmpty(keypath))
            throw new InvalidOperationException("CprHashing:KeyPath has not been configured.");

        _key = new Lazy<byte[]>(() =>
        {
            if (!File.Exists(keypath))
                throw new FileNotFoundException("CPR Key file was not found.", keypath);

            try
            {
                var content = File.ReadAllText(keypath).Trim();
                var key = Convert.FromBase64String(content);

                if (key.Length < 32)
                    throw new InvalidOperationException("CPR Key must be atleast 32 bytes.");

                return key;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("CPR key file is not valid Base64.", ex);
            }
        });

        var _ = _key.Value;
    }

    public string Hash(string cpr)
    {
        if (!Regex.IsMatch(cpr, CPR_REGEX))
            throw new InvalidOperationException("The provided CPR number, is not a valid format. Should match Regex, [0-9]{10}");

        var key = _key.Value;

        var data = Encoding.UTF8.GetBytes(cpr);
        var hash = HMACSHA256.HashData(key, data);

        return Convert.ToHexString(hash);
    }
}

internal interface ICprCryptoService
{
    string Hash(string cpr);
}
