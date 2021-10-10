/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.exceptions
{
    /// <summary>
    /// [throw] slot that throws an exception.
    /// </summary>
    [Slot(Name = "throw")]
    public class Throw : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var isPublic = input.Children
                .FirstOrDefault(x => x.Name == "public")?
                .GetEx<bool>() ?? false;

            var status = input.Children
                .FirstOrDefault(x => x.Name == "status")?
                .GetEx<int>() ?? 500;

            var field = input.Children
                .FirstOrDefault(x => x.Name == "field")?
                .GetEx<string>();

            throw new HyperlambdaException(input.GetEx<string>() ?? "[no-message]", isPublic, status, field);
        }
    }
}
