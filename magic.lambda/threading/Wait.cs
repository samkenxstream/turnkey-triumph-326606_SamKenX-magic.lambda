/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.threading
{
    /// <summary>
    /// [join] slot, waiting for all (direct) children [fork] invocations to finish their work,
    /// before allowing execution to continue.
    /// </summary>
    [Slot(Name = "join")]
    public class Join : ISlot, ISlotAsync
    {
        readonly ThreadRunner _runner;

        /// <summary>
        /// CTOR for slot.
        /// </summary>
        /// <param name="runner">Dependency injected implementation</param>
        public Join(ThreadRunner runner)
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
            var tasks = new List<Task>();
            signaler.Scope(".magic.lambe.join", tasks, () =>
            {
                signaler.Signal("eval", input);
                Task.WaitAll(tasks.ToArray());
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
            var tasks = new List<Task>();
            await signaler.ScopeAsync(".magic.lambe.threading.join", tasks, async () =>
            {
                await signaler.SignalAsync("eval", input);
                Task.WaitAll(tasks.ToArray());
            });
        }
    }
}
