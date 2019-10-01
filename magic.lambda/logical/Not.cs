/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.logical
{
    /// <summary>
    /// [not] slot, negating the value of its first children's value.
    /// </summary>
    [Slot(Name = "not")]
    public class Not : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() != 1)
                throw new ApplicationException("Operator [not] requires exactly one child");

            signaler.Signal("eval", input);

            input.Value = !input.Children.First().GetEx<bool>();
        }
    }
}
