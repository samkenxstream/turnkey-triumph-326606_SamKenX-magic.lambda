/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.exceptions
{
    [Slot(Name = "throw")]
    public class Throw : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            throw new ApplicationException(input.GetEx<string>() ?? "[no-message]");
        }
    }
}
