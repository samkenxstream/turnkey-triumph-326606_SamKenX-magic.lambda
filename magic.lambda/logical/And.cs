/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.logical
{
    /// <summary>
    /// [and] slot allowing you to group multiple comparisons (for instance), where all of these must evaluate
    /// to true, for the [and] slot as a whole to evaluate to true.
    /// </summary>
    [Slot(Name = "and")]
    public class And : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            SanityCheck(input);
            input.Value = !Common.Signal(signaler, input, false);
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            SanityCheck(input);
            input.Value = !await Common.SignalAsync(signaler, input, false);
        }

        #region [ -- Private helper methods -- ]

        void SanityCheck(Node input)
        {
            if (input.Children.Count() < 2)
                throw new ArgumentException("[and] must have at least two children nodes");
        }

        #endregion
    }
}
