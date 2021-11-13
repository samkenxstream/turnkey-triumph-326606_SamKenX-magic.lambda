/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.eval
{
    /// <summary>
    /// [context] slot allowing you to create a dynamic stack object context,
    /// that you can retrieve in children scopes of your lambda using [get-context].
    /// </summary>
    [Slot(Name = "context")]
    public class Context : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var arguments = GetArguments(input);
            signaler.Scope(arguments.Name, arguments.Value, () =>
            {
                signaler.Signal("eval", arguments.Lambda);
            });
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaiatble task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            var arguments = GetArguments(input);
            await signaler.ScopeAsync(arguments.Name, arguments.Value, async () =>
            {
                await signaler.SignalAsync("eval", arguments.Lambda);
            });
        }

        #region [ -- Private helper methods -- ]

        /*
         * Retrieves areguments from input, and returns to caller.
         */
        (string Name, object Value, Node Lambda) GetArguments(Node input)
        {
            var name = input.GetEx<string>() ??
                throw new ArgumentException("[context] requires a [value] argument, being whatever object you want to push unto your stack");
            name = "dynamic." + name;

            var value = input.Children.FirstOrDefault(x => x.Name == "value")?.GetEx<object>() ??
                throw new ArgumentException("[context] requires a [value] argument, being whatever object you want to push unto your stack");
            if (value is Node valueNode)
                value = valueNode.Clone();

            var lambda = input.Children.FirstOrDefault(x => x.Name == ".lambda") ??
                throw new ArgumentException("[context] requires a [.lambda] object to evaluate");

            return (name, value, lambda);
        }

        #endregion
    }
}
