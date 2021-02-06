using System;
using System.Collections.Generic;
using System.Net;

namespace YogoServer.ErrorHandling
{
    public class ApiException : Exception
    {
        public IEnumerable<string> MessagesToLog { get; set; }

        public int Status { get; private set; }

        public ApiException(int status, string message = null, IEnumerable<string> messagesToLog = null) : base(message)
        {
            Status = status;
            MessagesToLog = messagesToLog;
        }

        public ApiException(HttpStatusCode status, string message = null, IEnumerable<string> messagesToLog = null) : this((int)status, message, messagesToLog)
        {
        }
    }
}
