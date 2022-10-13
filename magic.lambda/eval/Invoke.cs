/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.eval
{
    /// <summary>
    /// [invoke] slot, allowing you to dynamically evaluate a piece of lambda as if it was
    /// a function, passing in arguments, and returning nodes to caller.
    /// </summary>
    [Slot(Name = "invoke")]
    public class Invoke : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            ExecuteAsync(signaler, input).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaiatble task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await ExecuteAsync(signaler, input);
        }

        #region [ -- Private helper methods -- ]

        /*
         * Executes the given scope.
         */
        async Task ExecuteAsync(ISignaler signaler, Node input)
        {
            // Making sure we're able to handle returned values and nodes from slot implementation.
            var result = new Node();
            await signaler.ScopeAsync("slots.result", result, async () =>
            {
                var lambda = GetLambda(input, input.Get<Expression>().Evaluate(input).Single().Clone());

                // Evaluating lambda of slot.
                await signaler.SignalAsync("eval", lambda);

                // Applying result.
                ApplyResult(input, result);
            });
        }

        /*
         * Helper method containing commonalities to retrieve and parametrise lambda object.
         */
        Node GetLambda(Node input, Node lambda)
        {
            /*
             * Preparing arguments, if there are any, making sure we remove
             * any declarative [.arguments] first.
             */
            lambda.Children
                .FirstOrDefault(x => x.Name == ".arguments")?
                .UnTie();
            if (input.Children.Any())
                lambda.Insert(0, new Node(".arguments", null, input.Children.ToList()));

            // Returning lambda object to caller.
            return lambda;
        }

        /*
         * Commonalities to apply result after invocation of [eval].
         */
        void ApplyResult(Node input, Node result)
        {
            // Clearing Children collection, since it might contain input parameters.
            input.Clear();

            // Making sure we return both value and any children nodes to caller.
            input.Value = result.Value;
            input.AddRange(result.Children.ToList());
        }

        #endregion
    }
}
