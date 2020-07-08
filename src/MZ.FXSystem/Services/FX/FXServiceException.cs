using System;

namespace MZ.FXSystem.Services.FX
{
    public class FXServiceException : Exception
    {
        public FXServiceException(string message) : base(message)
        {
        }
    }
}