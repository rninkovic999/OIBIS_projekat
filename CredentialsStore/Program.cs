﻿using Common;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;


namespace CredentialsStore
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding bindingClient = new NetTcpBinding();
            NetTcpBinding bindingAuthentificationService = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:5000/CredentialsStore";
            string addressAuthentificationService = "net.tcp://localhost:6000/CredentialsStore";

            //WINDOWS AUTHENTICATION PROTOCOL INIT FOR CLIENT

            bindingClient.Security.Mode = SecurityMode.Message;
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost hostCredentialsStore = new ServiceHost(typeof(CredentialsStore));
            ServiceHost hostAuthenticationServiceManagement = new ServiceHost(typeof(AuthenticationServiceManagement));

            //CERTIFICATE SERVER CONFIGURATION INIT

            string serverName = CertificateFormatter.ParseName(WindowsIdentity.GetCurrent().Name); 
            bindingAuthentificationService.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            hostAuthenticationServiceManagement.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostAuthenticationServiceManagement.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck; 
            hostAuthenticationServiceManagement.Credentials.ServiceCertificate.Certificate = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "credentialsstore"); //Server public/private-key.PFX

            hostCredentialsStore.AddServiceEndpoint(typeof(IAccountManagement), bindingClient, addressClient);
            hostAuthenticationServiceManagement.AddServiceEndpoint(typeof(IAuthenticationServiceManagement), bindingAuthentificationService, addressAuthentificationService);

            hostCredentialsStore.Open();

            try
            {
                hostAuthenticationServiceManagement.Open();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Server certificate check failed. Please contact your system administrator.\n");
                hostCredentialsStore.Abort(); //To avoid CS server faulted state
            }

            Console.WriteLine($"Credentials store servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");

            Console.ReadLine();

            hostCredentialsStore.Close();
            hostAuthenticationServiceManagement.Close();

        }
    }
}
