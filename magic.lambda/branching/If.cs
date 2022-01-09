/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    /// <summary>
    /// [if] slot, allowing you to branch in your code execution according to some condition.
    /// </summary>
    [Slot(Name = "if")]
    public class If : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
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
