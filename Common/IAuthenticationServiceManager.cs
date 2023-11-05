using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
    public interface IAuthenticationServiceManager
    {

        [OperationContract]
        void LockAccount(string username);

        [OperationContract]
        void EnableAccount(string username);

        [OperationContract]
        void DisableAccount(string username);


    }
}
