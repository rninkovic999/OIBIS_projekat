using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string authenticationServiceAddress = "net.tcp://localhost:4000/AuthenticationService";
            string credentialsStoreAddress = "net.tcp://localhost:5000/CredentialsStore";

            //WINDOWS AUTHENTICATION PROTOCOL INIT

            //Used for Anti-Phishing protection
           
            binding.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection


            using (AuthenticationProxy authenticationProxy = new AuthenticationProxy(binding, authenticationServiceAddress))
            {

            }

            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress))
            {
                try
                {
                    credentialsStoreProxy.CreateAccount("marko", "marko");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine($"Currently used by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + "\n");


            Console.ReadLine();
        }
    }
}