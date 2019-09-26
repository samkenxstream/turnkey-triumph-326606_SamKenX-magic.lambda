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
    [Slot(Name = "set-value")]
    [Slot(Name = "set-x")]
    public class SetValue : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-value] can have maximum one child node");

            var destinations = input.Evaluate().ToList();
            if (destinations.Count == 0)
                return;

            signaler.Signal("eval", input);

            var source = input.Name == "set-value" ? input.Children.FirstOrDefault()?.GetEx<object>() : input.Children.FirstOrDefault()?.Get<object>();
            foreach (var idx in destinations)
            {
                idx.Value = source;
            }
        }
    }
}
