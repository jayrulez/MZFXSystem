using System;

namespace MarshalZehr.FXSystem.Services.FX
{
    public class FXServiceException : Exception
    {
        public FXServiceException(string message) : base(message)
        {
        }
    }
}