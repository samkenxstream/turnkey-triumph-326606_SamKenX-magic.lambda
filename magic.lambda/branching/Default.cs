/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    [Slot(Name = "default")]
    public class Default : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Parent?.Name != "switch")
                throw new ApplicationException("[default] must be a child of [switch]");

            signaler.Signal("eval", input);
        }
    }
}
