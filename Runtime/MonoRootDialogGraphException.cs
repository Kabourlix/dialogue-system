using System;

namespace Aurore.DialogSystem
{
    [Serializable]
    public class MonoRootDialogGraphException : Exception
    {
        public MonoRootDialogGraphException()
        {
        }

        public MonoRootDialogGraphException(string message) : base(message)
        {
        }

        public MonoRootDialogGraphException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}