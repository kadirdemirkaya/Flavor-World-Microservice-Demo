﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Exceptions
{

    [Serializable]
    public class RepositoryErrorException : Exception
    {
        public string Repositoryname { get; }

        public RepositoryErrorException()
            : base($"Repository got error !")
        {
        }

        public RepositoryErrorException(string repositoryname)
            : base($"Repository Name : '{repositoryname}' got error !")
        {
            Repositoryname = repositoryname;
        }

        public RepositoryErrorException(string repositoryname, string message)
            : base($"Repository Name : {repositoryname} - Error : {message}")
        {
            Repositoryname = repositoryname;
        }

        public RepositoryErrorException(string repositoryname, string message, Exception inner)
            : base(message, inner)
        {
            Repositoryname = repositoryname;
        }

        protected RepositoryErrorException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
            Repositoryname = info.GetString("Repositoryname")!;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Repositoryname", Repositoryname);
        }
    }
}
