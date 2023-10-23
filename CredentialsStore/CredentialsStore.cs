using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CredentialsStore
{
    public class CredentialsStore : IAccountManagement
    {
        public void CreateAccount(string username, string password)
        {
            Console.WriteLine("Napravljen novi akaunt");
        }

        public void DeleteAccount(string username)
        {
            throw new NotImplementedException();
        }

        public void DisableAccount(string username)
        {
            throw new NotImplementedException();
        }

        public void EnableAccount(string username)
        {
            throw new NotImplementedException();
        }

        public void LockAccount(string username)
        {
            throw new NotImplementedException();
        }
    }
}
