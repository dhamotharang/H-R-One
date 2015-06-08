using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace HROne.CommonLib
{
    public class Crypto
    {
        /// <remarks>
        /// Supported .Net intrinsic SymmetricAlgorithm classes.
        /// </remarks>
        public enum SymmProvEnum : int
        {
            DES, RC2, Rijndael, TripleDES
        }

        private SymmetricAlgorithm mobjCryptoService;
        private string storedKey = string.Empty;
        ICryptoTransform encrypto;
        ICryptoTransform decrypto;
        /// <remarks>
        /// Constructor for using an intrinsic .Net SymmetricAlgorithm class.
        /// </remarks>
        public Crypto(SymmProvEnum NetSelected)
        {
            switch (NetSelected)
            {
                case SymmProvEnum.DES:
                    mobjCryptoService = new DESCryptoServiceProvider();
                    break;
                case SymmProvEnum.RC2:
                    mobjCryptoService = new RC2CryptoServiceProvider();
                    break;
                case SymmProvEnum.Rijndael:
                    mobjCryptoService = new RijndaelManaged();
                    break;
                case SymmProvEnum.TripleDES:
                    mobjCryptoService = new TripleDESCryptoServiceProvider();
                    break;

            }
        }

        /// <remarks>
        /// Constructor for using a customized SymmetricAlgorithm class.
        /// </remarks>
        public Crypto(SymmetricAlgorithm ServiceProvider)
        {
            mobjCryptoService = ServiceProvider;
        }

        /// <remarks>
        /// Depending on the legal key size limitations of a specific CryptoService provider
        /// and length of the private key provided, padding the secret key with space character
        /// to meet the legal size of the algorithm.
        /// </remarks>
        private byte[] GetLegalKey(string Key)
        {
            string sTemp;
            if (mobjCryptoService.LegalKeySizes.Length > 0)
            {
                int lessSize = 0, moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;
                // key sizes are in bits
                while (Key.Length * 8 > moreSize && mobjCryptoService.LegalKeySizes[0].SkipSize > 0)
                {
                    lessSize = moreSize;
                    moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
                }
                sTemp = Key.PadRight(moreSize / 8, ' ');
                sTemp = sTemp.Substring(0, moreSize / 8);
            }
            else
                sTemp = Key;

            // convert the secret key to byte array
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        private void initialEncryptorDecryptor(string Key)
        {
            storedKey = Key;
            byte[] bytKey = GetLegalKey(Key);


            // set the private key
            mobjCryptoService.Key = bytKey;
            string tmpVIString = ASCIIEncoding.ASCII.GetString(bytKey);
            while (tmpVIString.Length < mobjCryptoService.IV.Length)
                tmpVIString += tmpVIString;
            mobjCryptoService.IV = ASCIIEncoding.ASCII.GetBytes(tmpVIString.Substring(0, mobjCryptoService.IV.Length));
            //            mobjCryptoService.IV = bytKey;
            // create an Encryptor from the Provider Service instance
            encrypto = mobjCryptoService.CreateEncryptor();
            // create a Decryptor from the Provider Service instance
            decrypto = mobjCryptoService.CreateDecryptor();
        }
        //  use key string as vector too
        public string Encrypting(string Source, string Key)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            // create a MemoryStream so that the process can be done without I/O files
            MemoryStream ms = new MemoryStream();


            //if (storedKey != Key)
            //{
            //  initialize CryptoTransform everytime to prevent any unknown error caused by reusing CryptoTransform
                initialEncryptorDecryptor(Key);
            //}

            // create Crypto Stream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // get the output and trim the '\0' bytes
            byte[] bytOut = ms.GetBuffer();
            long memoryStreamLength = ms.Length;

            cs.Close();
            ms.Close();
            if (memoryStreamLength > bytOut.Length)
            {
                throw new Exception("Incorrect length during encryption");
            }


            //int i = 0;
            //for (i = bytOut.Length - 1; i >= 0; i--)
            //    if (bytOut[i] != 0)
            //        break;
            //if (i % 8 != 7)
            //    i += (8 - i % 8 -1);
            // convert into Base64 so that the result can be used in xml
            return Convert.ToBase64String(bytOut, 0, Convert.ToInt32(memoryStreamLength));
        }

        //  use key string as vector too
        public string Decrypting(string Source, string Key)
        {
            //if (string.IsNullOrEmpty(Source))
            //    throw new Exception("Invalid Encrypted String");
            //if (Source.Contains(" "))
            //    throw new Exception("Invalid Encrypted String");
            //if (Source.Length % 4 != 0)
            //    throw new Exception("Invalid Encrypted String");

            //if (storedKey != Key)
            //{
                //  initialize CryptoTransform everytime to prevent any unknown error caused by reusing CryptoTransform
                initialEncryptorDecryptor(Key);
            //}

            // convert from Base64 to binary
            byte[] bytIn = Convert.FromBase64String(Source);
            //if (bytIn.Length % decrypto.InputBlockSize !=0)
            //    throw new Exception("Invalid Encrypted String");

            // create a MemoryStream with the input
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);

            // create Crypto Stream that transforms a stream using the decryption
            CryptoStream cs = new CryptoStream(ms, decrypto, CryptoStreamMode.Read);

            // read out the result from the Crypto Stream
            StreamReader sr = new StreamReader(cs,UTF8Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }

    public class RSACrypto
    {
        private RSACryptoServiceProvider rsa;

        public RSACrypto()
        {
            CspParameters param = new CspParameters();
            param.Flags = CspProviderFlags.UseMachineKeyStore;
            rsa = new RSACryptoServiceProvider(1024);
        }

        public RSACrypto(string ContainerName)
        {
            CspParameters param = new CspParameters();
            param.Flags = CspProviderFlags.UseMachineKeyStore;
            param.KeyContainerName = ContainerName;
            rsa = new RSACryptoServiceProvider(1024, param);
        }

        public void FromXMLString(string FromXMLString)
        {
            rsa.FromXmlString(FromXMLString);
        }

        public string ToXmlString()
        {
            return rsa.ToXmlString(false);
        }

        public string Encrypting(string Source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
           

            // get the output and trim the '\0' bytes
            byte[] bytOut =  rsa.Encrypt(bytIn,true);

            //To interoperate with CAPI, you must manually reverse the order of encrypted bytes before the encrypted data interoperates with another API. You can easily reverse the order of a managed byte array by calling the Array.Reverse method.
            Array.Reverse(bytOut);

            return Convert.ToBase64String(bytOut, Base64FormattingOptions.None);
        }

        //  use key string as vector too
        public string Decrypting(string Source)
        {
            byte[] bytIn = Convert.FromBase64String(Source);
            //To interoperate with CAPI, you must manually reverse the order of encrypted bytes before the encrypted data interoperates with another API. You can easily reverse the order of a managed byte array by calling the Array.Reverse method.
            Array.Reverse(bytIn);
            byte[] bytOut = rsa.Decrypt(bytIn, true);
            return UTF8Encoding.UTF8.GetString(bytOut);

        }

    }
    public class Hash
    {
        public static string PasswordHash(string text)
        {
            SHA1Managed sha1 = new SHA1Managed();
            sha1.ComputeHash(Encoding.Unicode.GetBytes(text));
            byte[] bytes = sha1.Hash;
            return Convert.ToBase64String(bytes);
        }
    }
}
