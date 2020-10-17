/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda
{
    /// <summary>
    /// [whitelist] slot, allowing you to create sub-vocabulary of legal slots.
    /// </summary>
    [Slot(Name = "whitelist")]
    public class Whitelist : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var whitelist = GetWhitelist(input);
            signaler.Scope("whitelist", whitelist.Vocabulary, () =>
            {
                signaler.Signal("eval", whitelist.Lambda);
            });
        }

        /// <summary>
        /// Async implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            var whitelist = GetWhitelist(input);
            await signaler.ScopeAsync("whitelist", whitelist.Vocabulary, async () =>
            {
                await signaler.SignalAsync("eval", whitelist.Lambda);
            });
        }

        #region [ -- Private helper methods -- ]

        /*
         * Helper method to retrieve [whitelist] arguments.
         */
        (List<Node> Vocabulary, Node Lambda) GetWhitelist(Node input)
        {
            var vocabulary = input.Children
                .FirstOrDefault(x => x.Name == "vocabulary")?
                .Children?
                .Select(x => x.Clone())
                .ToList() ??
                    throw new ArgumentException("No [vocabulary] provided to [whitelist]");

            var lambda = input.Children.FirstOrDefault(x => x.Name == ".lambda") ??
                throw new ArgumentException("No [.lambda] provided to [whitelist]");

            return (vocabulary, lambda);
        }

        #endregion
    }
}
