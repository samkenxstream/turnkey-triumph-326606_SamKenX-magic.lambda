/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;

namespace magic.lambda.threading
{
    /// <summary>
    /// Helper class to run threads.
    /// </summary>
    public class ThreadRunner
    {
        readonly ISignaler _signaler;

        /// <summary>
        /// Constructor creating new instance of type.
        /// </summary>
        /// <param name="signaler">Dependency injected signals implementation.</param>
        public ThreadRunner(ISignaler signaler)
        {
            _signaler = signaler;
        }

        /// <summary>
        /// Executes the specified lambda node on a new thread, fire and forget style.
        /// </summary>
        /// <param name="input">Node being lambda object to execute.</param>
        public void Run(Node input)
        {
            // Notice, fire && forget invocation of async method.
            _signaler.SignalAsync("wait.eval", input);
        }
    }
}
