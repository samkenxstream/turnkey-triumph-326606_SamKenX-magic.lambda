/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    /// <summary>
    /// [case] slot for [switch] slots.
    /// </summary>
    [Slot(Name = "case")]
    public class Case : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Parent?.Name != "switch")
                throw new ApplicationException("[case] must be a child of [switch]");

            signaler.Signal("eval", input);
        }
    }
}
