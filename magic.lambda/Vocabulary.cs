/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.slots
{
    /// <summary>
    /// [vocabulary] slot allowing you to dynamically retrieve all the names of all slots that exists in the system.
    /// </summary>
    [Slot(Name = "vocabulary")]
    public class Vocabulary : ISlot
    {
        readonly ISignalsProvider _signalProvider;

        // TODO: Rename ISignalsProvider to ISlotProvider.
        /// <summary>
        /// Constructor creating an object requiring a signals provider to be able to fetch all slots that exists.
        /// </summary>
        /// <param name="signalProvider">Slot provider, providing all slots that exists in the system.</param>
        public Vocabulary(ISignalsProvider signalProvider)
        {
            _signalProvider = signalProvider ?? throw new ArgumentNullException(nameof(signalProvider));
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            input.Clear();
            input.AddRange(_signalProvider.Keys
                .Where(x => !x.StartsWith(".", StringComparison.InvariantCulture))
                .Select(x => new Node("", x)));
        }
    }
}
