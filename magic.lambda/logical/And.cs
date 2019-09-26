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
    [Slot(Name = "and")]
    public class And : ISlot
    {
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
