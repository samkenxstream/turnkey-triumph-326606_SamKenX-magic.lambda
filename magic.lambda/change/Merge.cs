/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    /// <summary>
    /// [merge] slot combining [invoke] and [add] to dynamically inject nodes into result of executing expression.
    /// </summary>
    [Slot(Name = "merge")]
    public class Merge : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            SignalAsync(signaler, input).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Iterating through each destination.
            foreach (var idxDest in input.Evaluate().ToList())
            {
                // Making sure we can reset back to original nodes after every single iteration.
                var old = input.Children.Select(x => x.Clone()).ToList();

                // Making sure we're able to handle returned values and nodes from slot implementation.
                var result = new Node();
                await signaler.ScopeAsync("slots.result", result, async () =>
                {
                    input.Insert(0, new Node(".dp", idxDest));

                    // Evaluating lambda of slot.
                    await signaler.SignalAsync("eval", input);

                    // Resetting back to original nodes.
                    input.Clear();

                    // Notice, cloning in case we've got another iteration, to avoid changing original nodes' values.
                    input.AddRange(old.Select(x => x.Clone()));
                });

                // Applying result.
                idxDest.AddRange(result.Children);
            }
            input.Clear();
        }
    }
}
