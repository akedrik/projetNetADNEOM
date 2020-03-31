using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreApp.Core.Exceptions
{
   public class RecordNotFoundException: Exception
    {
        public RecordNotFoundException() : base("Aucun enregistrement trouvé.")
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }

        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
