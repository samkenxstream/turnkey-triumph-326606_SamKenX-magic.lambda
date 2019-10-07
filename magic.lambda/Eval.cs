/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda
{
    /// <summary>
    /// [eval] slot, allowing you to dynamically evaluate a piece of lambda.
    /// </summary>
    [Slot(Name = "eval")]
    public class Eval : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Sanity checking invocation. Notice non [eval] keywords might have expressions and children.
            if (input.Name == "eval" && input.Value != null && input.Children.Any())
                throw new ApplicationException("[eval] cannot handle both expression values and children at the same time");

            // Children have precedence, in case invocation is from a non [eval] keyword.
            if (input.Children.Any())
            {
                Execute(signaler, input.Children);
            }
            else if (input.Name == "eval" && input.Value != null)
            {
                var nodes = input.Evaluate();
                Execute(signaler, nodes.SelectMany(x => x.Children));
            }
        }

        #region [ -- Private helper methods -- ]

        /*
         * Executes the given scope.
         */
        void Execute(ISignaler signaler, IEnumerable<Node> nodes)
        {
            // Storing termination node, to check if we should terminate early for some reasons.
            var terminate = signaler.Peek<Node>("slots.result");

            // Evaluating "scope".
            foreach (var idx in nodes)
            {
                if (idx.Name == "" || idx.Name.FirstOrDefault() == '.')
                    continue;

                signaler.Signal(idx.Name, idx);

                // Checking if execution for some reasons was terminated.
                if (terminate != null && (terminate.Value != null || terminate.Children.Any()))
                    return;
            }
        }

        #endregion
    }
}
