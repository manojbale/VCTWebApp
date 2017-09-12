using System;
using System.Security.Cryptography;
using System.Text;

namespace VCTWeb.Core.Domain
{
    //public class SaltedHash
    //{
    //    #region Instance Variables

    //    private HashAlgorithm HashProvider;
    //    private string Salt = string.Empty;
    //    //private int SalthLength;

    //    #endregion

    //    #region Constructors

    //    /// <summary>
    //    /// The constructor takes a HashAlgorithm as a parameter.
    //    /// </summary>
    //    /// <param name="HashAlgorithm">
    //    /// A <see cref="HashAlgorithm"/> HashAlgorihm which is derived from HashAlgorithm. C# provides
    //    /// the following classes: SHA1Managed,SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
    //    /// </param>
    //    public SaltedHash(HashAlgorithm HashAlgorithm)
    //    {
    //        HashProvider = HashAlgorithm;
    //        this.Salt = Constants.SaltKey;
    //    }

    //    /// <summary>
    //    /// Default constructor which initialises the SaltedHash with the SHA1Managed algorithm
    //    /// and a Salt of 4 bytes ( or 4*8 = 32 bits)
    //    /// </summary>
    //    public SaltedHash()
    //        : this(new System.Security.Cryptography.SHA1Managed())
    //    {
    //    }

    //    #endregion

    //    #region Public Methods

    //    public bool VerifyPassword(string data, string hash)
    //    {
    //        string encyptedData = this.EncryptPassword(data);
    //        if (encyptedData == hash)
    //            return true;
    //        else
    //            return false;
    //    }

    //    public string EncryptPassword(string data)
    //    {
    //        string encyptedData = string.Empty;
    //        byte[] hash1, hash2;

    //        hash1 = this.GetHash(data, this.Salt);


    //        hash2 = hash1; //new byte[hash1.Length];

    //        //int counter = 0;
    //        //for (int i = hash1.Length-1; i >= 0; i++)
    //        //{
    //        //    hash2[counter] = hash1[i];
    //        //}

    //        Array.Reverse(hash2);

    //        byte[] hash12 = new byte[hash1.Length + hash2.Length];

    //        // Copy both the data and salt into the new array
    //        Array.Copy(hash1, hash12, hash1.Length);
    //        Array.Copy(hash2, 0, hash12, hash1.Length, hash2.Length);

    //        encyptedData = Convert.ToBase64String(ComputeHash(hash12, Encoding.UTF8.GetBytes(this.Salt.Trim())));

    //        return encyptedData;
    //    }

    //    private byte[] GetHash(string Data, string Salt)
    //    {
    //        return ComputeHash(Encoding.UTF8.GetBytes(Data.Trim()), Encoding.UTF8.GetBytes(Salt.Trim()));
    //    }

    //    /// <summary>
    //    /// Gets the hash value.
    //    /// </summary>
    //    /// <param name="password">The password.</param>
    //    /// <returns>returns hash value</returns>
    //    private string GetHashValue(string password)
    //    {
    //        string hash;

    //        this.GetHashAndSaltString(password, this.Salt, out hash);

    //        return hash;
    //    }

    //    /// <summary>
    //    /// The routine provides a wrapper around the GetHashAndSalt function providing conversion
    //    /// from the required byte arrays to strings. Both the Hash and Salt are returned as Base-64 encoded strings.
    //    /// </summary>
    //    /// <param name="Data">
    //    /// A <see cref="System.String"/> string containing the data to hash
    //    /// </param>
    //    /// <param name="Hash">
    //    /// A <see cref="System.String"/> base64 encoded string containing the generated hash
    //    /// </param>
    //    /// <param name="Salt">
    //    /// A <see cref="System.String"/> base64 encoded string containing the generated salt
    //    /// </param>
    //    private void GetHashAndSaltString(string Data, string Salt, out string Hash)
    //    {
    //        byte[] HashOut;

    //        // Obtain the Hash and Salt for the given string
    //        GetHashAndSalt(Encoding.UTF8.GetBytes(Data.Trim()), Encoding.UTF8.GetBytes(Salt.Trim()), out HashOut);

    //        // Transform the byte[] to Base-64 encoded strings
    //        Hash = Convert.ToBase64String(HashOut);
    //    }

    //    /// <summary>
    //    /// This routine provides a wrapper around VerifyHash converting the strings containing the
    //    /// data, hash and salt into byte arrays before calling VerifyHash.
    //    /// </summary>
    //    /// <param name="Data">A UTF-8 encoded string containing the data to verify</param>
    //    /// <param name="Hash">A base-64 encoded string containing the previously stored hash</param>
    //    /// <param name="Salt">A base-64 encoded string containing the previously stored salt</param>
    //    /// <returns></returns>
    //    private bool VerifyHashString(string Data, string Hash)
    //    {
    //        byte[] HashToVerify = Convert.FromBase64String(Hash.Trim());
    //        byte[] SaltToVerify = Encoding.UTF8.GetBytes(this.Salt.Trim()); //!string.IsNullOrEmpty(Salt) ? Convert.FromBase64String(Salt.Trim()) : new byte[] { };
    //        byte[] DataToVerify = Encoding.UTF8.GetBytes(Data.Trim());
    //        return VerifyHash(DataToVerify, HashToVerify, SaltToVerify);
    //    }

    //    #endregion

    //    #region Private Methods

    //    /// <summary>
    //    /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
    //    /// </summary>
    //    /// <param name="Data">A byte array of the Data to Hash</param>
    //    /// <param name="Salt">A byte array of the Salt to add to the Hash</param>
    //    /// <returns>A byte array with the calculated hash</returns>
    //    private byte[] ComputeHash(byte[] Data, byte[] Salt)
    //    {
    //        // Allocate memory to store both the Data and Salt together
    //        byte[] DataAndSalt = new byte[Data.Length + Salt.Length];

    //        // Copy both the data and salt into the new array
    //        Array.Copy(Data, DataAndSalt, Data.Length);
    //        Array.Copy(Salt, 0, DataAndSalt, Data.Length, Salt.Length);

    //        // Calculate the hash
    //        // Compute hash value of our plain text with appended salt.
    //        return HashProvider.ComputeHash(DataAndSalt);
    //    }

    //    /// <summary>
    //    /// Given a data block this routine returns both a Hash and a Salt
    //    /// </summary>
    //    /// <param name="Data">
    //    /// A <see cref="System.Byte"/>byte array containing the data from which to derive the salt
    //    /// </param>
    //    /// <param name="Hash">
    //    /// A <see cref="System.Byte"/>byte array which will contain the hash calculated
    //    /// </param>
    //    /// <param name="Salt">
    //    /// A <see cref="System.Byte"/>byte array which will contain the salt generated
    //    /// </param>
    //    private void GetHashAndSalt(byte[] Data, byte[] Salt, out byte[] Hash)
    //    {
    //        //// Allocate memory for the salt
    //        //Salt = new byte[SalthLength];

    //        //// Strong runtime pseudo-random number generator, on Windows uses CryptAPI
    //        //// on Unix /dev/urandom
    //        //RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

    //        //// Create a random salt
    //        //random.GetNonZeroBytes(Salt);

    //        // Compute hash value of our data with the salt.
    //        Hash = ComputeHash(Data, Salt);
    //    }


    //    /// <summary>
    //    /// This routine verifies whether the data generates the same hash as we had stored previously
    //    /// </summary>
    //    /// <param name="Data">The data to verify </param>
    //    /// <param name="Hash">The hash we had stored previously</param>
    //    /// <param name="Salt">The salt we had stored previously</param>
    //    /// <returns>True on a succesfull match</returns>
    //    private bool VerifyHash(byte[] Data, byte[] Hash, byte[] Salt)
    //    {
    //        byte[] NewHash = ComputeHash(Data, Salt);

    //        //  No easy array comparison in C# -- we do the legwork
    //        if (NewHash.Length != Hash.Length) return false;

    //        for (int Lp = 0; Lp < Hash.Length; Lp++)
    //            if (!Hash[Lp].Equals(NewHash[Lp]))
    //                return false;

    //        return true;
    //    }

    //    #endregion

    //}
}