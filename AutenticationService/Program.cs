using Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), binding, address);
            host.Open();

            Console.WriteLine("Authentication servis pokrenu!!\n");

            //Klijentska aplikacija

            string credentialsStoreAddress = "net.tcp://localhost:6000/CredentialsStore";
            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress)) { }

            Console.WriteLine("Authentication servis [validacija] pokrenut!!\n");


            Console.ReadLine();
        }
    }
}
