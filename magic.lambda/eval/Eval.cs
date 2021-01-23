/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.eval
{
    /// <summary>
    /// [eval] slot, allowing you to dynamically evaluate a piece of lambda.
    /// </summary>
    [Slot(Name = "eval")]
    public class Eval : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Execute(signaler, GetNodes(input));
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaiatble task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await ExecuteAsync(signaler, GetNodes(input));
        }

        #region [ -- Private helper methods -- ]

        /*
         * Executes the given scope.
         */
        void Execute(ISignaler signaler, IEnumerable<Node> nodes)
        {
            // Storing termination node, to check if we should terminate early for some reasons.
            var terminate = signaler.Peek<Node>("slots.result");
            var whitelist = signaler.Peek<List<Node>>("whitelist");

            // Evaluating "scope".
            foreach (var idx in nodes)
            {
                if (whitelist != null && !whitelist.Any(x => x.Name == idx.Name))
                    throw new ArgumentException($"Slot [{idx.Name}] doesn't exist in currrent scope");

                // Invoking signal.
                signaler.Signal(idx.Name, idx);

                // Checking if execution for some reasons was terminated.
                if (terminate != null && (terminate.Value != null || terminate.Children.Any()))
                    return;
            }
        }

        /*
         * Executes the given scope.
         */
        async Task ExecuteAsync(ISignaler signaler, IEnumerable<Node> nodes)
        {
            // Storing termination node, to check if we should terminate early for some reasons.
            var terminate = signaler.Peek<Node>("slots.result");
            var whitelist = signaler.Peek<List<Node>>("whitelist");

            // Evaluating "scope".
            foreach (var idx in nodes)
            {
                if (whitelist != null && !whitelist.Any(x => x.Name == idx.Name))
                    throw new ArgumentException($"Slot [{idx.Name}] doesn't exist in currrent scope");

                // Invoking signal.
                await signaler.SignalAsync(idx.Name, idx);

                // Checking if execution for some reasons was terminated.
                if (terminate != null && (terminate.Value != null || terminate.Children.Any()))
                    return;
            }
        }

        /*
         * Helper to retrieve execution nodes for slot.
         */
        IEnumerable<Node> GetNodes(Node input)
        {
            // Sanity checking invocation. Notice non [eval] keywords might have expressions and children.
            if (input.Value != null &&
                input.Children.Any() &&
                input.Name == "eval")
                throw new ArgumentException("[eval] cannot handle both expression values and children at the same time");

            // Children have precedence, in case invocation is from a non [eval] keyword.
            if (input.Children.Any())
                return input
                    .Children
                    .Where(x => x.Name != string.Empty && !x.Name.StartsWith("."));

            if (input.Value != null && 
                input.Name == "eval")
                return input
                    .Evaluate()
                    .SelectMany(x => 
                        x.Children
                            .Where(x2 => x2.Name != string.Empty && !x2.Name.StartsWith(".")));

            // Nothing to evaluate here.
            return Array.Empty<Node>();
        }

        #endregion
    }
}
