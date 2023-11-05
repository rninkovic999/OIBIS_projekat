using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Common
{
    [DataContract]
    public class InvalidGroupException
    {

        [DataMember]
        public string exceptionMessage;

        public InvalidGroupException(string exception) { exceptionMessage = exception; }

    }
}
