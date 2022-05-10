using System;
using System.Collections.Generic;
using System.Text;

namespace Backgroundworker.Exceptions
{
    public class UnknownQueueException : Exception
    {
        public UnknownQueueException(string exceptionMessage) : base(message: exceptionMessage) { }
    }
}
