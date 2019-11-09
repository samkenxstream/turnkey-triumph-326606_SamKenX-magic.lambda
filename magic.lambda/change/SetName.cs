/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    // TODO: Consider renaming to only "name" ...?
    /// <summary>
    /// [set-name] slot allowing you to change the names of nodes in your lambda graph object.
    /// </summary>
    [Slot(Name = "set-name")]
    [Slot(Name = "wait.set-name")]
    public class SetName : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-name] can have maximum one child node");

            signaler.Signal("eval", input);
            SetNameToSource(input);
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-name] can have maximum one child node");

            await signaler.SignalAsync("eval", input);
            SetNameToSource(input);
        }

        #region [ -- Private helper methods -- ]

        void SetNameToSource(Node input)
        {
            var source = input.Children.FirstOrDefault()?.GetEx<string>() ?? "";
            foreach (var idx in input.Evaluate())
            {
                idx.Name = source;
            }
        }

        #endregion
    }
}
