using Common;
using Common.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {

            string secretKey = SecretKey.GenerateKey();
            SecretKey.StoreKey(secretKey, AES.KeyLocation);

            //AuthenticationService SERVER INIT

            NetTcpBinding bindingClient = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/AuthenticationService";


            //WINDOWS AUTHENTICATION PROTOCOL INIT FOR CLIENT

            bindingClient.Security.Mode = SecurityMode.Message; 
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; 
            bindingClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; 

            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), bindingClient, address);
            host.Open();

           
            Console.WriteLine($"Authentication servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

          
            Console.ReadKey();

            //RESET CURRENTLY LOGGED IN USERS ON AS CLOSE

            CurrentUsers users = new CurrentUsers();
            users.resetCurrentUsers();

            host.Close();
        }
    }
}
