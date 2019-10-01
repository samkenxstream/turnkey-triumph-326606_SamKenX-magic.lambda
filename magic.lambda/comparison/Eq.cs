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
    // TODO: Consider adding [=] "synonym" for this slot, and its related slots.
    /// <summary>
    /// [eq] slot allowing you to compare two values for equality.
    /// </summary>
    [Slot(Name = "eq")]
    public class Eq : ISlot
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
                    return false;
                else if (lhs == null && rhs != null)
                    return false;
                else if (lhs.GetType() != rhs.GetType())
                    return false;
                return ((IComparable)lhs).CompareTo(rhs) == 0;
            });
        }
    }
}
