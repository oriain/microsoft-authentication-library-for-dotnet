﻿//----------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// The exception type thrown when service returns and error response or other networking errors occur.
    /// </summary>
    public class MsalServiceException : MsalException
    {
        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MsalServiceException(string errorCode, string message)
            : base(
                errorCode, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">Status code of the resposne received from the service.</param>
        public MsalServiceException(string errorCode, string message, int statusCode)
            : this(errorCode, message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified. It may especially contain the actual error message returned by the service.
        /// </param>
        public MsalServiceException(string errorCode, string message,
            Exception innerException)
            : base(
                errorCode, message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">Status code of the resposne received from the service.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified. It may especially contain the actual error message returned by the service.
        /// </param>
        public MsalServiceException(string errorCode, string message, int statusCode,
            Exception innerException)
            : base(
                errorCode, message, innerException)
        {
            StatusCode = statusCode;
        }


        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">The status code of the request.</param>
        /// <param name="claims">The claims challenge returned back from the service.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified. It may especially contain the actual error message returned by the service.
        /// </param>
        public MsalServiceException(string errorCode, string message, int statusCode, string claims,
            Exception innerException)
            : this(
                errorCode, message, statusCode, innerException)
        {
            Claims = claims;
        }

        /// <summary>
        /// Gets the status code returned from http layer. This status code is either the HttpStatusCode in the inner
        /// HttpRequestException response or
        /// NavigateError Event Status Code in browser based flow (See
        /// http://msdn.microsoft.com/en-us/library/bb268233(v=vs.85).aspx).
        /// You can use this code for purposes such as implementing retry logic or error investigation.
        /// </summary>
        public int StatusCode { get; } = 0;

        public string Claims { get; }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(CultureInfo.InvariantCulture, "\n\tStatusCode: {0}\n\tClaims: {1}", StatusCode, Claims);
        }
    }
}