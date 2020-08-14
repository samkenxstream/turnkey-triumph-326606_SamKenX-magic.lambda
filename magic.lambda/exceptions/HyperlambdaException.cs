/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;

namespace magic.lambda.exceptions
{
    public class HyperlambdaException : Exception
    {
        public HyperlambdaException(string message, bool isPublic)
            : base(message)
        {
            IsPublic = isPublic;
        }

        public bool IsPublic { get; set; }
    }
}
