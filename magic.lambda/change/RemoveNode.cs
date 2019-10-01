/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    // TODO: Consider renaming slot.
    /// <summary>
    /// [remove-node] slot allowing you to remove nodes from your lambda graph object.
    /// </summary>
    [Slot(Name = "remove-node")]
    public class RemoveNode : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            foreach (var idx in input.Evaluate().ToList())
            {
                idx.Parent.Remove(idx);
            }
        }
    }
}
