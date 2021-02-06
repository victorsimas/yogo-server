using System.Collections.Generic;
using static System.Net.HttpStatusCode;

namespace YogoServer.ErrorHandling
{
    public class FailedDependencyException : ApiException
    {
        public FailedDependencyException() : base(FailedDependency)
        {
        }

        public FailedDependencyException(string message) : base(FailedDependency, message)
        {
        }

        public FailedDependencyException(string message, IEnumerable<string> messagesToLog) : base(FailedDependency, message, messagesToLog)
        {
        }
    }
}