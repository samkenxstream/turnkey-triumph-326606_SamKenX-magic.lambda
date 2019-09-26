/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.source
{
    [Slot(Name = "get-nodes")]
    public class GetNodes : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Value == null)
                return;

            var src = input.Evaluate();
            foreach (var idx in src)
            {
                input.Add(idx.Clone());
            }
        }
    }
}
