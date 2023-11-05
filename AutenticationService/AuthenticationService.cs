using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

namespace AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string username, string password)
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.generalUsers))
            {
                Console.WriteLine($"{Thread.CurrentPrincipal.Identity.Name} is now logged in.");
            }
            else
            {
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions!!!\n"));
            }
        }

        public void Logout()
        {
            if (Thread.CurrentPrincipal.IsInRole(Groups.generalUsers))
            {
                Console.WriteLine($"{Thread.CurrentPrincipal.Identity.Name} is now logged out.");
            }
            else
            {
                throw new FaultException<InvalidGroupException>(new InvalidGroupException("Invalid Group permissions!!!\n"));
            }
        }
    }
}
