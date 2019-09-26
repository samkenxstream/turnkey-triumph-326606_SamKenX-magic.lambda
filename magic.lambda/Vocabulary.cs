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
    [Slot(Name = "vocabulary")]
    public class Vocabulary : ISlot
    {
        readonly ISignalsProvider _signalProvider;

        public Vocabulary(ISignalsProvider signalProvider)
        {
            _signalProvider = signalProvider ?? throw new ArgumentNullException(nameof(signalProvider));
        }

        public void Signal(ISignaler signaler, Node input)
        {
            input.Clear();
            input.AddRange(_signalProvider.Keys
                .Where(x => !x.StartsWith(".", StringComparison.InvariantCulture))
                .Select(x => new Node("", x)));
        }
    }
}
