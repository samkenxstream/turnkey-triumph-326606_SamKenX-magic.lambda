/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.misc
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
            var result = new Node();
            signaler.Scope("slots.result", result, () =>
            {
                var whitelist = GetWhitelist(input);
                signaler.Scope("whitelist", whitelist.Vocabulary, () =>
                {
                    signaler.Signal("eval", whitelist.Lambda.Clone());
                });
                input.Clear();
                input.Value = result.Value;
                input.AddRange(result.Children.ToList());
            });
        }

        /// <summary>
        /// Async implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            var result = new Node();
            await signaler.ScopeAsync("slots.result", result, async () =>
            {
                var whitelist = GetWhitelist(input);
                await signaler.ScopeAsync("whitelist", whitelist.Vocabulary, async () =>
                {
                    await signaler.SignalAsync("eval", whitelist.Lambda.Clone());
                });
                input.Clear();
                input.Value = result.Value;
                input.AddRange(result.Children.ToList());
            });
        }

        #region [ -- Private helper methods -- ]

        /*
         * Helper method to retrieve [whitelist] arguments.
         */
        (List<Node> Vocabulary, Node Lambda) GetWhitelist(Node input)
        {
            var vocabulary = input
                .Children
                .FirstOrDefault(x => x.Name == "vocabulary")?
                .Children
                .ToList() ??
                    throw new HyperlambdaException("No [vocabulary] provided to [whitelist]");

            var lambda = input.Children.FirstOrDefault(x => x.Name == ".lambda") ??
                throw new HyperlambdaException("No [.lambda] provided to [whitelist]");

            return (vocabulary, lambda);
        }

        #endregion
    }
}
