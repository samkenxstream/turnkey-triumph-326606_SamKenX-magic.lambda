/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@gaiasoul.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    // TODO: Consider renaming to something else.
    /// <summary>
    /// [unwrap] slot allowing you to forward evaluate expressions in your lambda graph object.
    /// </summary>
    [Slot(Name = "unwrap")]
    public class Unwrap : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            foreach (var idx in input.Evaluate())
            {
                if (idx.Value != null)
                {
                    var exp = idx.Evaluate();
                    if (exp.Count() > 1)
                        throw new ApplicationException("Multiple sources found for [unwrap]");

                    idx.Value = exp.FirstOrDefault()?.Value;
                }
            }
        }
    }
}
