/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    /// <summary>
    /// [set-value] and [set-x] slots allowing you to change the values of nodes in your lambda graph object.
    /// If you use [set-x] any expresions in your source will not be evaluated, allowing you to set the values
    /// of nodes to become expressions.
    /// </summary>
    [Slot(Name = "set-value")]
    [Slot(Name = "set-x")]
    public class SetValue : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-value] can have maximum one child node");

            var destinations = input.Evaluate().ToList();
            if (destinations.Count == 0)
                return;

            signaler.Signal("eval", input);

            var source = input.Name.EndsWith("set-value", StringComparison.InvariantCulture) ? input.Children.FirstOrDefault()?.GetEx<object>() : input.Children.FirstOrDefault()?.Get<object>();
            foreach (var idx in destinations)
            {
                idx.Value = source;
            }
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-value] can have maximum one child node");

            var destinations = input.Evaluate().ToList();
            if (destinations.Count == 0)
                return;

            await signaler.SignalAsync("eval", input);

            var source = input.Name.EndsWith("set-value", StringComparison.InvariantCulture) ? input.Children.FirstOrDefault()?.GetEx<object>() : input.Children.FirstOrDefault()?.Get<object>();
            foreach (var idx in destinations)
            {
                idx.Value = source;
            }
        }
    }
}
