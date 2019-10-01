/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.source
{
    /// <summary>
    /// [get-name] slot that will return the name of the node found by evaluating an expression.
    /// </summary>
    [Slot(Name = "get-name")]
    public class GetName : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var src = input.Evaluate();
            if (src.Count() > 1)
                throw new ApplicationException("Too many nodes returned from [name] expression");
            input.Value = src.FirstOrDefault()?.Name ?? null;
        }
    }
}
