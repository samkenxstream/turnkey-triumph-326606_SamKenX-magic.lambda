/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;

namespace magic.lambda.exceptions
{
    /// <summary>
    /// Exception type thrown from Hyperlambda using [throw].
    /// </summary>
    public class HyperlambdaException : Exception
    {
        /// <summary>
        /// Constructs a new instance of a Hyperlambda exception.
        /// </summary>
        /// <param name="message">Exception error text.</param>
        /// <param name="isPublic">Whether or not exception should propagate to client in release builds.</param>
        public HyperlambdaException(string message, bool isPublic, int status)
            : base(message)
        {
            IsPublic = isPublic;
            Status = status;
        }

        /// <summary>
        /// Whether ot not exception will propagate to client in release builds.
        /// </summary>
        /// <value>Returns true if exception is visible to the client.</value>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Status code to return to client.
        /// </summary>
        /// <value>HTTP status code to return to client.</value>
        public int Status { get; set; }
    }
}
