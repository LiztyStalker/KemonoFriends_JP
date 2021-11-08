using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FieldIndexOutException : SystemException
{
    public FieldIndexOutException() : base() { }
    public FieldIndexOutException(string msg) : base(msg) { }
    public FieldIndexOutException(string msg, Exception innerException) : base(msg, innerException) { }

}

