/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.source
{
    /// <summary>
    /// [get-value] slot that will return the value of the node found by evaluating an expression.
    /// </summary>
    [Slot(Name = "get-value")]
    public class GetValue : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var src = input.Evaluate();
            if (src.Count() > 1)
                throw new HyperlambdaException("Too many nodes returned from [get-value] expression");
            input.Value = src.FirstOrDefault()?.Value;
        }
    }
}
