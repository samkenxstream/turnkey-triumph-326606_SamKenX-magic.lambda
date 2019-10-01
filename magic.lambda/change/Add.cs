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
    /// <summary>
    /// [add] slot allowing you to append nodes into some destination node.
    /// </summary>
    [Slot(Name = "add")]
    public class Add : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            signaler.Signal("eval", input);

            // Looping through each destination.
            foreach (var idxDest in input.Evaluate())
            {
                // Looping through each source node and adding its children to currently iterated destination.
                foreach (var idxSource in input.Children.SelectMany(x => x.Children))
                {
                    idxDest.Add(idxSource.Clone()); // Cloning in case of multiple destinations.
                }
            }
        }
    }
}
