using System.Runtime.Serialization;

namespace ImageService.Application.Exceptions
{
    [Serializable]
    public class ImageExistErrorException : Exception
    {
        public ImageExistErrorException()
            : base($"User Image Already Exists !")
        {
        }

        public ImageExistErrorException(string message)
            : base(message)
        {

        }

        public ImageExistErrorException(string message, Exception inner)
            : base(message, inner)
        {

        }

        protected ImageExistErrorException(
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
