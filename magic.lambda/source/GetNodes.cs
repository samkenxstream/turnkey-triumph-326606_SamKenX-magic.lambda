/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.source
{
    /// <summary>
    /// [get-nodes] slot that will return all nodes from evaluating an expression.
    /// </summary>
    [Slot(Name = "get-nodes")]
    public class GetNodes : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Value == null)
                return;

            var src = input.Evaluate();
            foreach (var idx in src)
            {
                input.Add(idx.Clone());
            }
        }
    }
}
