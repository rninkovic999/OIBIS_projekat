﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    [DataContract]
    public class InvalidGroupException : System.Exception 
    {
        [DataMember]
        public string exceptionMessage;

        public InvalidGroupException(string exception) { exceptionMessage = exception; }
    }
}
