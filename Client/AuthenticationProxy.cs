﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Client
{
    public class AuthenticationProxy : ChannelFactory<IAuthenticationService>, IAuthenticationService, IDisposable
    {
        IAuthenticationService factory;

        public AuthenticationProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            //Credentials.Windows.AllowNtlm = false; not usable as we dont have domain controllers.
            factory = this.CreateChannel();
        }

        public AuthenticationProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            //Credentials.Windows.AllowNtlm = false; not usable as we dont have domain controllers.
            factory = this.CreateChannel();
        }

        public void Login(string username, string password)
        {
            factory.Login(username, password);
        }

        public void Logout()
        {
            factory.Logout();
        }
    }
}
