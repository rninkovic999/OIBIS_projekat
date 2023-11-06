using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CredentialsStore
{
    public class CredentialsStore : IAccountManagement
    {
        public void CreateAccount(string username, string password)
        {
            try
            {
                Console.WriteLine("Uspjelo dodavanje novog korisnika -" + username);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
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
