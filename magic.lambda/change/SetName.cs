/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    // TODO: Consider renaming to only "name" ...?
    /// <summary>
    /// [set-name] slot allowing you to change the names of nodes in your lambda graph object.
    /// </summary>
    [Slot(Name = "set-name")]
    public class SetName : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-name] can have maximum one child node");

            signaler.Signal("eval", input);

            var source = input.Children.FirstOrDefault()?.GetEx<string>() ?? "";
            foreach (var idx in input.Evaluate())
            {
                idx.Name = source;
            }
        }
    }
}
