﻿/*
 * Aista Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 * See the enclosed LICENSE file for details.
 */

using System;
using magic.signals.contracts;

namespace magic.lambda.comparison
{
    /// <summary>
    /// [mtw] slot returning true if its first child's value is "more than or equal" to its second child's value.
    /// </summary>
    [Slot(Name = "mte")]
    public class Mte : BaseComparison
    {
        #region [ -- Protected overridden methods -- ]

        /// <inheritdoc />
        protected override bool Compare(object lhs, object rhs)
        {
            if (lhs == null && rhs == null)
                return true;
            else if (lhs != null && rhs == null)
                return true;
            else if (lhs == null)
                return false;
            else if (lhs.GetType() != rhs.GetType())
                return false;
            return ((IComparable)lhs).CompareTo(rhs) >= 0;
        }

        #endregion
    }
}
