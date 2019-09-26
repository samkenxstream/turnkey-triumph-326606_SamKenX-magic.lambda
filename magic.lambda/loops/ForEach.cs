/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.loops
{
    [Slot(Name = "for-each")]
    public class ForEach : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            // Making sure we can reset back to original nodes after every single iteration.
            var old = input.Children.Select(x => x.Clone()).ToList();

            foreach (var idx in input.Evaluate())
            {
                // Inserting "data pointer".
                input.Insert(0, new Node(".dp", idx));

                // Evaluating "body" lambda of [for-each]
                signaler.Signal("eval", input);

                // Resetting back to original nodes.
                input.Clear();

                // Notice, cloning in case we've got another iteration, to avoid changing original nodes' values.
                input.AddRange(old.Select(x => x.Clone()));
            }
        }
    }
}
