/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.eval
{
    /// <summary>
    /// [vocabulary] slot allowing you to dynamically retrieve the names
    /// of all slots that exists in the system.
    /// </summary>
    [Slot(Name = "vocabulary")]
    public class Vocabulary : ISlot
    {
        readonly ISignalsProvider _signalProvider;

        /// <summary>
        /// Constructor creating an object requiring a signals provider to be able to fetch all slots that exists.
        /// </summary>
        /// <param name="signalProvider">Slot provider, providing all slots that exists in the system.</param>
        public Vocabulary(ISignalsProvider signalProvider)
        {
            _signalProvider = signalProvider;
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            input.Clear();
            var filter = input.GetEx<string>();
            input.Value = null;
            var whitelist = signaler.Peek<List<Node>>("whitelist");
            if (filter == null)
            {
                input.AddRange(_signalProvider.Keys
                    .Where(x => 
                        !x.StartsWith(".", StringComparison.InvariantCulture) &&
                        (whitelist == null || whitelist.Any(x2 => x2.Name == x)))
                    .Select(x => new Node("", x)));
            }
            else
            {
                input.AddRange(_signalProvider.Keys
                    .Where(x => 
                        !x.StartsWith(".", StringComparison.InvariantCulture) &&
                        x.StartsWith(filter, StringComparison.InvariantCulture) &&
                        (whitelist == null || whitelist.Any(x2 => x2.Name == x)))
                    .Select(x => new Node("", x)));
            }
        }
    }
}
