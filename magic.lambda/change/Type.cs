/*
 * Aista Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Text;
using System.Linq;
using sys = System;
using System.Globalization;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.node.extensions.hyperlambda;

namespace magic.lambda.change
{
    /// <summary>
    /// [type] slot allowing you to retrieve Hyperlambda type information for some specified value.
    /// </summary>
    [Slot(Name = "type")]
    public class Type : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var value = input.GetEx<object>();
            var result = Converter.ToString(value);
            input.Value = result.Item1;
        }
    }
}
