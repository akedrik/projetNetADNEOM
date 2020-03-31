using System;

namespace NetCoreApp.Core.Exceptions
{
    public class RecordAlreadyExistException : Exception
    {
        public RecordAlreadyExistException() : base("Cet enregistrement existe déjà.")
        {
        }

        public RecordAlreadyExistException(string message) : base(message)
        {
        }

        public RecordAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
