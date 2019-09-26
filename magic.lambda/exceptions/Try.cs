/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.exceptions
{
    [Slot(Name = "try")]
    public class Try : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            try
            {
                signaler.Signal("eval", input);
            }
            catch (Exception err)
            {
                var foundCatch = ExecuteCatch(signaler, input, err);
                ExecuteFinally(signaler, input);
                if (foundCatch)
                    return;
                else
                    throw;
            }
            ExecuteFinally(signaler, input);
        }

        #region [ -- Private helper methods -- ]

        /*
         * Executes [.catch] if existing, and returns true if [.catch] was found
         */
        bool ExecuteCatch(ISignaler signaler, Node input, Exception err)
        {
            if (input.Next?.Name == ".catch")
            {
                var next = input.Next;
                var args = new Node(".arguments");
                args.Add(new Node("message", err.Message));
                args.Add(new Node("type", err.GetType().FullName));
                next.Insert(0, args);
                signaler.Signal("eval", next);
                return true;
            }
            return false;
        }

        /*
         * Executes [.finally] if it exists.
         */
        void ExecuteFinally(ISignaler signaler, Node input)
        {
            if (input.Next?.Name == ".finally")
                signaler.Signal("eval", input.Next);
            else if (input.Next?.Next?.Name == ".finally")
                signaler.Signal("eval", input.Next.Next);
        }

        #endregion
    }
}
