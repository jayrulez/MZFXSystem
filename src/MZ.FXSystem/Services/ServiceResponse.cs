namespace MZ.FXSystem.Services
{
    /// <summary>
    /// Wraps an error code an associated error message in a standard format.
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// The error code.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        public ServiceError()
        {
            ErrorCode = string.Empty;
            ErrorMessage = "An unexpected error occured.";
        }

        public ServiceError(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode ?? string.Empty;
            ErrorMessage = errorMessage;
        }

        public ServiceError(string errorMessage)
        {
            ErrorCode = string.Empty;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Represents a standard response from an operation.
    /// Either the expected data is returned as a successful response or an error is returned.
    /// </summary>
    /// <typeparam name="TData">The type of data to return.</typeparam>
    public class ServiceResponse<TData> where TData : class, new()
    {
        public TData Data { get; set; }

        public ServiceError Error { get; set; }

        /// <summary>
        /// Whether or not the response is successful.
        /// If the response is successful then check the <param name="Data">Data</param> property for the data.
        /// If the response is unsuccessful then check the <param name="Error">Error</param> property for the error information.
        /// </summary>
        public bool IsSuccessful { get { return Error == null; } }

        public ServiceResponse()
        {
            Data = default;
            Error = null;
        }

        public ServiceResponse(TData data)
        {
            Data = data;
            Error = null;
        }

        public ServiceResponse(ServiceError error)
        {
            Data = default;
            Error = error;
        }

        public ServiceResponse<TData> Succeed(TData data)
        {
            Data = data;
            Error = null;

            return this;
        }

        public ServiceResponse<TData> Fail(ServiceError error)
        {
            Data = default;
            Error = error;

            return this;
        }

        public ServiceResponse<TData> Fail(string errorCode, string errorMessage)
        {
            Data = default;
            Error = new ServiceError(errorCode, errorMessage);

            return this;
        }

        public ServiceResponse<TData> Fail(string errorMessage)
        {
            Data = default;
            Error = new ServiceError(errorMessage);

            return this;
        }
    }
}
