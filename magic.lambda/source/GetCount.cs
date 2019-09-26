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
    [Slot(Name = "get-count")]
    public class GetCount : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Value == null)
                throw new ApplicationException("No expression source provided for [count]");

            var src = input.Evaluate();
            input.Value = src.Count();
        }
    }
}
