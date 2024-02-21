using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DigitalSignatures.Common
{
    public class DigitalSignatureManager
    {
        public static byte[] Create(byte[] data, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;
            
                if (csp == null)
                    throw new Exception("Valid certificate not found!");
            

            SHA1Managed sha1 = new SHA1Managed();
            byte[] dataByteHash = sha1.ComputeHash(data);

            byte[] signature = csp.SignHash(dataByteHash, CryptoConfig.MapNameToOID("SHA1"));
            return signature;
        }


        public static bool Verify(byte[] data, byte[] signature, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            SHA1Managed sha1 = new SHA1Managed();
            byte[] hashdataBytesArray = sha1.ComputeHash(data);

            return csp.VerifyHash(hashdataBytesArray, CryptoConfig.MapNameToOID("SHA1"), signature);
        }
    }
}