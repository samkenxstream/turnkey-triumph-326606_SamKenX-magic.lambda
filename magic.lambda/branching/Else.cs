/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    /// <summary>
    /// [else] slot for matching with an [if] and/or [else-if] slot. Must come after either or the previously mentioned slots.
    /// </summary>
    [Slot(Name = "else")]
    public class Else : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var previous = input.Previous;
            if (previous == null || (previous.Name != "if" && previous.Name != "else-if"))
                throw new ApplicationException("[else] must have an [if] or [else-if] before it");

            var evaluate = true;
            while (previous != null && (previous.Name == "if" || previous.Name == "else-if"))
            {
                if (previous.Children.First().GetEx<bool>())
                {
                    evaluate = false;
                    break;
                }
                previous = previous.Previous;
            }
            if (evaluate)
                signaler.Signal("eval", input);
        }
    }
}
