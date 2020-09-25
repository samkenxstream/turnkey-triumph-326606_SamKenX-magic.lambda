/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.threading
{
    /// <summary>
    /// [fork] slot, allowing you to create and start a new thread.
    /// </summary>
    [Slot(Name = "fork")]
    public class Fork : ISlot, ISlotAsync
    {
        readonly ThreadRunner _runner;

        /// <summary>
        /// CTOR for slot.
        /// </summary>
        /// <param name="runner">Dependency injected implementation</param>
        public Fork(ThreadRunner runner)
        {
            _runner = runner;
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            _runner.Run(input.Clone());
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        /// <returns>An awaiatble task.</returns>
        public Task SignalAsync(ISignaler signaler, Node input)
        {
            _runner.Run(input.Clone());
            return Task.CompletedTask;
        }
    }
}
