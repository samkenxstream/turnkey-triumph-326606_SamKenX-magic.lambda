/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.comparison
{
    /// <summary>
    /// Base class for all comparison operators.
    /// </summary>
    public abstract class BaseComparison : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            SanityCheck(input);
            signaler.Signal("eval", input);
            input.Value = Compare(
                input.Children.First().GetEx<object>(),
                input.Children.Skip(1).First().GetEx<object>());
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
            await signaler.SignalAsync("eval", input);
            input.Value = Compare(
                input.Children.First().GetEx<object>(),
                input.Children.Skip(1).First().GetEx<object>());
        }

        #region [ -- Protected abstract methods -- ]

        /// <summary>
        /// Implementation of comparison operator which is expected to be implemented
        /// in sub class.
        /// </summary>
        /// <param name="lhs">Left hand side of comparison.</param>
        /// <param name="rhs">Right hand side of comparison.</param>
        /// <returns>True if comparison yields true.</returns>
        protected abstract bool Compare(object lhs, object rhs);

        #endregion

        #region [ -- Private helper methods -- ]

        static void SanityCheck(Node input)
        {
            if (input.Children.Count() != 2)
                throw new ArgumentException($"Comparison operation [{input.Name}] requires exactly two operands");
        }

        #endregion
    }
}
