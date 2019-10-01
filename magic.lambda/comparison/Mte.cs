/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using magic.node;
using magic.signals.contracts;
using magic.lambda.comparison.utilities;

namespace magic.lambda.comparison
{
    /// <summary>
    /// [mtw] slot returning true if its first child's value is "more than or equals" to its second child's value.
    /// </summary>
    [Slot(Name = "mte")]
    public class Mte : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Common.Compare(signaler, input, (lhs, rhs) =>
            {
                if (lhs == null && rhs == null)
                    return true;
                else if (lhs != null && rhs == null)
                    return true;
                else if (lhs == null && rhs != null)
                    return false;
                else if (lhs.GetType() != rhs.GetType())
                    return false;
                return ((IComparable)lhs).CompareTo(rhs) >= 0;
            });
        }
    }
}
