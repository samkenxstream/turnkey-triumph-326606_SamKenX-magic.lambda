/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    [Slot(Name = "unwrap")]
    public class Unwrap : ISlot
    {
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
