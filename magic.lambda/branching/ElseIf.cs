/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    /// <summary>
    /// [else-if] slot for branching logic. Must come after either another [else-if] or an [if] slot.
    /// </summary>
    [Slot(Name = "else-if")]
    public class ElseIf : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Sanity checking invocation.
            Common.SanityCheckElse(input);

            // Checking if we should short circuit invocation.
            if (!Common.ShouldEvaluateElse(input))
            {
                // Previous conditional invocation yielded true.
                input.Value = false;
                return;
            }

            // Checking if we should evaluate lambda object.
            if (!Common.ConditionIsTrue(signaler, input))
            {
                // Result of condition yields false.
                input.Value = false;
                return;
            }

            // Result of condition yields true. ORDER COUNTS!
            signaler.Signal("eval", Common.GetLambda(input));
            input.Value = true;
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Sanity checking invocation.
            Common.SanityCheckElse(input);

            // Checking if we should short circuit invocation.
            if (!Common.ShouldEvaluateElse(input))
            {
                input.Value = false;
                return;
            }

            // Checking if we should evaluate lambda object.
            if (!await Common.ConditionIsTrueAsync(signaler, input))
            {
                // Result of condition yields false.
                input.Value = false;
                return;
            }

            // Result of condition yields true. ORDER COUNTS!
            await signaler.SignalAsync("eval", Common.GetLambda(input));
            input.Value = true;
        }
    }
}
