/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.strings
{
    [Slot(Name = "to-upper")]
    public class ToUpper : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            input.Value = input.GetEx<string>().ToUpperInvariant();
        }
    }
}
