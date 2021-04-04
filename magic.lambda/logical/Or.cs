/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
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
    /// [or] slot allowing you to group multiple comparisons (for instance), where at least one of these must evaluate
    /// to true, for the [or] slot as a whole to evaluate to true.
    /// </summary>
    [Slot(Name = "or")]
    public class Or : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            SanityCheck(input);
            input.Value = Common.Signal(signaler, input, true);
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
            input.Value = await Common.SignalAsync(signaler, input, true);
        }

        #region [ -- Private helper methods -- ]

        void SanityCheck(Node input)
        {
            if (input.Children.Count() < 2)
                throw new ArgumentException("[or] must have at least 2 argument nodes");
        }

        #endregion
    }
}
