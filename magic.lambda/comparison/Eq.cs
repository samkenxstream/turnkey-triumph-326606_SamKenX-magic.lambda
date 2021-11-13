/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using magic.signals.contracts;

namespace magic.lambda.comparison
{
    /// <summary>
    /// [eq] slot allowing you to compare two values for equality.
    /// </summary>
    [Slot(Name = "eq")]
    public class Eq : BaseComparison
    {
        #region [ -- Protected overridden methods -- ]

        /// <inheritdoc />
        protected override bool Compare(object lhs, object rhs)
        {
            if (lhs == null && rhs == null)
                return true;
            else if (lhs != null && rhs == null)
                return false;
            else if (lhs == null)
                return false;
            else if (lhs.GetType() != rhs.GetType())
                return false;
            return ((IComparable)lhs).CompareTo(rhs) == 0;
        }

        #endregion
    }
}
