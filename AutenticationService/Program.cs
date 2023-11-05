using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {

            //Server za autentifikaciju

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/AuthenticationService";


            //Windows authentication protokol

            binding.Security.Mode = SecurityMode.Message;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), binding, address);
            host.Open();

            Console.WriteLine("Authentication servis pokrenu!!\n");

            //Klijentska aplikacija

            string credentialsStoreAddress = "net.tcp://localhost:6000/CredentialsStore";
            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress)) { }

            Console.WriteLine($"Authentication servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");


            Console.ReadLine();
        }
    }
}
