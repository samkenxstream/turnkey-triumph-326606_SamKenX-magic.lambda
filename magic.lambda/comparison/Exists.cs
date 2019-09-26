/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.comparison
{
    [Slot(Name = "exists")]
    public class Exists : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            input.Value = input.Evaluate().Any();
        }
    }
}
