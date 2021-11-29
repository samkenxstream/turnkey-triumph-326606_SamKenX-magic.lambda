/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Globalization;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    /// <summary>
    /// [format] slot allowing you to format some value according to some pattern.
    /// </summary>
    [Slot(Name = "format")]
    public class Format : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var value = input.GetEx<object>();
            var pattern = input.Children.First().GetEx<string>();
            input.Clear();
            input.Value = string.Format(CultureInfo.InvariantCulture, pattern, value);
        }
    }
}
