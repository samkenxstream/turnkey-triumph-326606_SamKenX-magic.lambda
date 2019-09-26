/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.strings
{
    [Slot(Name = "concat")]
    public class Concat : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (!input.Children.Any())
                throw new ApplicationException("No arguments provided to [concat]");

            signaler.Signal("eval", input);

            input.Value = string.Join("", input.Children.Select(x => x.GetEx<string>()));
        }
    }
}
