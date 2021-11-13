﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    /// <summary>
    /// [insert-after] slot allowing you to insert a range of nodes before some other node
    /// in your lambda graph object.
    /// </summary>
    [Slot(Name = "insert-before")]
    public class InsertBefore : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            signaler.Signal("eval", input);
            Insert(input);
            input.Clear();
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await signaler.SignalAsync("eval", input);
            Insert(input);
            input.Clear();
        }

        #region [ -- Private helper methods -- ]

        void Insert(Node input)
        {
            // Looping through each destination.
            foreach (var idxDest in input.Evaluate().ToList()) // To avoid changing collection during enumeration
            {
                // Looping through each source node and adding its children to currently iterated destination.
                foreach (var idxSource in input.Children.SelectMany(x => x.Children))
                {
                    idxDest.InsertBefore(idxSource.Clone()); // Cloning in case of multiple destinations.
                }
            }
        }

        #endregion
    }
}
