using System.Runtime.Serialization;

namespace AuthenticationService.Application.Exceptions
{
    [Serializable]
    public class ServiceErrorException : Exception
    {
        public string Servicename { get; }

        public ServiceErrorException()
            : base($"Service got error !")
        {
        }

        public ServiceErrorException(string servicename)
            : base($"Service Name : '{servicename}' got error !")
        {
            Servicename = servicename;
        }

        public ServiceErrorException(string servicename, string message)
            : base($"Service Name : {servicename} - Error : {message}")
        {
            Servicename = servicename;
        }

        public ServiceErrorException(string servicename, string message, Exception inner)
            : base(message, inner)
        {
            Servicename = servicename;
        }

        protected ServiceErrorException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
            Servicename = info.GetString("Servicename")!;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Servicename", Servicename);
        }
    }
}
