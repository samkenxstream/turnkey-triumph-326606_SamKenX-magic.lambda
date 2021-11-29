﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
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
            // Checking to see if we should evaluate at all.
            if (ShouldEvaluate(input, out Node lambda))
            {
                // Evaluating condition.
                signaler.Signal("eval", input);

                // Checking if evaluation of condition evaluated to true.
                if (input.Children.First().GetEx<bool>())
                    signaler.Signal("eval", lambda);
            }
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Checking to see if we should evaluate at all.
            if (ShouldEvaluate(input, out Node lambda))
            {
                // Evaluating condition.
                await signaler.SignalAsync("eval", input);

                // Checking if evaluation of condition evaluated to true.
                if (input.Children.First().GetEx<bool>())
                    await signaler.SignalAsync("eval", lambda);
            }
        }

        #region [ -- Private helper methods -- ]

        /*
         * Helper method for the above, to check if we should evaluate [else-if] at all.
         */
        bool ShouldEvaluate(Node input, out Node lambda)
        {
            if (input.Children.Count() != 2)
                throw new HyperlambdaException("Keyword [else-if] requires exactly two children nodes");

            lambda = input.Children.Skip(1).First();
            if (lambda.Name != ".lambda")
                throw new HyperlambdaException("Keyword [else-if] requires its second child node to be [.lambda]");

            var previous = input.Previous;
            if (previous == null ||
                (previous.Name != "if" && previous.Name != "else-if"))
                throw new HyperlambdaException("[else-if] must have an [if] or [else-if] before it");

            return PreviousIsFalse(previous);
        }

        #endregion

        #region [ -- Internal helper methods -- ]

        static internal bool PreviousIsFalse(Node input)
        {
            while (input != null && (input.Name == "if" || input.Name == "else-if"))
            {
                var current = input.Children.First();
                if (current.Value != null && current.GetEx<bool>())
                    return false;
                if (input.Name == "if")
                    break;
                input = input.Previous;
            }
            return true;
        }

        #endregion
    }
}
