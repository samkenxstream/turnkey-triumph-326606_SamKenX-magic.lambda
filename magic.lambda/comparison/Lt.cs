/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using magic.signals.contracts;

namespace magic.lambda.comparison
{
    /// <summary>
    /// [lt] slot returning true if its first child's value is "less than" its second child's value.
    /// </summary>
    [Slot(Name = "lt")]
    [Slot(Name = "wait.lt")]
    public class Lt : BaseComparison
    {
        #region [ -- Protected overridden methods -- ]

        /// <inheritdoc />
        protected override bool Compare(object lhs, object rhs)
        {
            if (lhs == null && rhs == null)
                return false;
            else if (lhs != null && rhs == null)
                return false;
            else if (lhs == null)
                return true;
            else if (lhs.GetType() != rhs.GetType())
                return false;
            return ((IComparable)lhs).CompareTo(rhs) == -1;
        }

        #endregion
    }
}
