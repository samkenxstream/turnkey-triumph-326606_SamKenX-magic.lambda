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
    /// [and] slot allowing you to group multiple comparisons (for instance), where all of these must evaluate
    /// to true, for the [and] slot as a whole to evaluate to true.
    /// </summary>
    [Slot(Name = "and")]
    public class And : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() < 2)
                throw new ApplicationException("Operator [and] requires at least two children nodes");

            signaler.Signal("eval", input);

            foreach (var idx in input.Children)
            {
                if (!idx.GetEx<bool>())
                {
                    input.Value = false;
                    return;
                }
            }
            input.Value = true;
        }
    }
}
