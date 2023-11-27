﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace AuthenticationService
{
    class Program
    {
        static void Main(string[] args)
        {

            //AuthenticationService SERVER INIT

            NetTcpBinding bindingClient = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/AuthenticationService";

            //WINDOWS AUTHENTICATION PROTOCOL INIT FOR CLIENT

            bindingClient.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            bindingClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            ServiceHost host = new ServiceHost(typeof(AuthenticationService));

            host.AddServiceEndpoint(typeof(IAuthenticationService), bindingClient, address);
            host.Open();

            //AuthenticationService CLIENT INIT - Certificate Authentication

            NetTcpBinding bindingCredentialsStore = new NetTcpBinding();
            bindingCredentialsStore.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate; //Certificate-based authentication
            X509Certificate2 serverCertificate = CertificateManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "credentialsstore"); //Server public-key.CER 
            EndpointAddress credentialsStoreAddress = new EndpointAddress(new Uri("net.tcp://localhost:6000/CredentialsStore"),
                                                                          new X509CertificateEndpointIdentity(serverCertificate));

            Console.WriteLine($"Authentication servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            using (CredentialsStoreProxy credentialsStoreProxy = new CredentialsStoreProxy(bindingCredentialsStore, credentialsStoreAddress))
            {
                try
                {
                    credentialsStoreProxy.LockAccount("mmm");
                    credentialsStoreProxy.EnableAccount("mmm");
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Client certificate check failed. Please contact your system administrator.\n");
                    Console.WriteLine(e);
                    credentialsStoreProxy.Abort(); //To avoid CS server faulted state
                }
            }
            
            Console.ReadLine();
        }
    }
}
