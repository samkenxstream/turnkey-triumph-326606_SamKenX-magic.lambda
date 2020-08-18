/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using sys = System.Threading;

namespace magic.lambda.threading
{
    /// <summary>
    /// [semaphore] slot, allowing you to create a semaphore,
    /// only allowing one caller entry into some lambda object at the same time.
    /// </summary>
    [Slot(Name = "sleep")]
    [Slot(Name = "wait.sleep")]
    public class Sleep : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            sys.Thread.Sleep(input.GetEx<int>());
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaiatble task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await sys.Tasks.Task.Delay(input.GetEx<int>());
        }

        #region [ -- Private helper methods -- ]


        #endregion
    }
}
