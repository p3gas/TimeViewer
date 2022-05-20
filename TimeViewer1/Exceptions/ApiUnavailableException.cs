using System;

namespace TimeViewer1.Exceptions
{
    public class ApiUnavailableException : Exception
    {
        public ApiUnavailableException()
        {

        }

        public ApiUnavailableException(string message)
            : base(message)
        {

        }

        public ApiUnavailableException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
