using System.Runtime.Serialization;

namespace AuthenticationService.Application.Exceptions
{
    [Serializable]
    public class PiplineValidationError : Exception
    {

        public PiplineValidationError()
            : base($"Validation pipline error !")
        {
        }

        public PiplineValidationError(string message)
            : base(message)
        {

        }

        public PiplineValidationError(string message, Exception inner)
            : base(message, inner)
        {

        }

        protected PiplineValidationError(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
