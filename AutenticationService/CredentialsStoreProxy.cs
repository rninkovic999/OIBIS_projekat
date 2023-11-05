using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    class CredentialsStoreProxy : ChannelFactory<IAuthenticationServiceManager>, IAuthenticationServiceManager, IDisposable
    {
        IAuthenticationServiceManager factory;

        public CredentialsStoreProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void DisableAccount(string username)
        {
            factory.DisableAccount(username);
        }

        public void EnableAccount(string username)
        {
            factory.EnableAccount(username);
        }

        public void LockAccount(string username)
        {
            factory.LockAccount(username);
        }
    }
}
