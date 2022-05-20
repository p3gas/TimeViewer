using System;

namespace TimeViewer1.Exceptions
{
    public class UnknownAreaException : Exception
    {
        public UnknownAreaException()
        {

        }

        public UnknownAreaException(string message)
            : base(message)
        {

        }
    }
}
