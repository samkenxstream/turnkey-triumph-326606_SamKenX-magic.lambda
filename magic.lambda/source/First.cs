/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.source
{
    /// <summary>
    /// [first] slot that will return the first value found by evaluating its expression, and/or
    /// its children nodes.
    /// </summary>
    [Slot(Name = "first")]
    public class First : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Prioritising value of node.
            var result = TryValue(input);

            // Checking if above resulted in null at which point we try to evaluate children nodes.
            if (result == null)
            {
                var whitelist = signaler.Peek<List<Node>>("whitelist");
                foreach (var idx in input.Children)
                {
                    if (idx.Name.Any() && idx.Name.FirstOrDefault() != '.')
                    {
                        if (whitelist != null && !whitelist.Any(x => x.Name == idx.Name))
                            throw new HyperlambdaException($"Slot [{idx.Name}] doesn't exist in currrent scope");
                        signaler.Signal(idx.Name, idx);
                    }

                    // Short curcuiting if we've got a value.
                    if (idx.Value != null)
                    {
                        result = idx.Value;
                        break;
                    }
                }
            }

            // Assigning result and doing some house cleaning.
            input.Clear();
            input.Value = result;
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>Awaitable task</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Prioritising value of node.
            var result = TryValue(input);

            // Checking if above resulted in null at which point we try to evaluate children nodes.
            if (result == null)
            {
                var whitelist = signaler.Peek<List<Node>>("whitelist");
                foreach (var idx in input.Children)
                {
                    if (idx.Name.Any() && idx.Name.FirstOrDefault() != '.')
                    {
                        if (whitelist != null && !whitelist.Any(x => x.Name == idx.Name))
                            throw new HyperlambdaException($"Slot [{idx.Name}] doesn't exist in currrent scope");
                        await signaler.SignalAsync(idx.Name, idx);
                    }

                    // Short curcuiting if we've got a value.
                    if (idx.Value != null)
                    {
                        result = idx.Value;
                        break;
                    }
                }
            }

            // Assigning result and doing some house cleaning.
            input.Clear();
            input.Value = result;
        }

        #region [ -- Private helper methods -- ]

        /*
         * Helper method to evaluate expressions in specified node, and return first result found
         * as result of evaluating expression, or null of no value found.
         */
        object TryValue(Node input)
        {
            if (input.Value is Expression exp)
            {
                var expResult = exp.Evaluate(input);
                foreach (var idxRes in expResult)
                {
                    if (idxRes.Value != null)
                        return idxRes.Value;
                }
            }
            return input.Value;
        }

        #endregion
    }
}
