using System.Runtime.Serialization;

namespace ImageService.Application.Exceptions
{
    [Serializable]
    public class ServiceErrorException : Exception
    {
        public ServiceErrorException()
            : base($"An unexpected error occurred in the service !")
        {
        }

        public ServiceErrorException(string serviceName, string message)
            : base($"{serviceName}:{message}")
        {

        }

        public ServiceErrorException(string serviceName, string message, Exception inner)
            : base($"{serviceName}:{message}", inner)
        {

        }

        protected ServiceErrorException(
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
