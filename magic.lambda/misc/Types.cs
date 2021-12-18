/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.misc
{
    /// <summary>
    /// [types] slot returning all supported Hyperlambda types.
    /// </summary>
    [Slot(Name = "types")]
    public class Types : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            input.AddRange(Converter.ListTypes().Select(x => new Node("", x)));
        }
    }
}
