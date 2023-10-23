﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

            using (AuthenticationProxy authenticationProxy = new AuthenticationProxy(binding, authenticationServiceAddress)) { }
            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(binding, credentialsStoreAddress)) {
                credentialsStoreProxy.CreateAccount("risto", "risto");
            
            
            }

            Console.WriteLine("Client pokrenut!");
            
            Console.ReadLine();
        }
    }
}
