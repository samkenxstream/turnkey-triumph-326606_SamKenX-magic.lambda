/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.branching
{
    [Slot(Name = "switch")]
    public class Switch : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (!input.Children.Any(x => x.Name == "case"))
                throw new ApplicationException("[switch] must have one at least one [case] child");

            if (input.Children.Any(x => x.Name != "case" && x.Name != "default"))
                throw new ApplicationException("[switch] can only handle [case] and [default] children");

            if (input.Children.Any(x => x.Name == "case" && x.Value == null))
                throw new ApplicationException("[case] with null value found");

            if (input.Children.Any(x => x.Name == "default" && x.Value != null))
                throw new ApplicationException("[default] with non-null value found");

            var result = input.GetEx<object>();

            var executionNode = input.Children
                .FirstOrDefault(x => x.Name == "case" && x.Value.Equals(result)) ??
                input.Children
                    .FirstOrDefault(x => x.Name == "default");

            if (executionNode != null)
            {
                while(executionNode != null && !executionNode.Children.Any() && executionNode.Name != "default")
                {
                    executionNode = executionNode.Next;
                }
                if (executionNode == null || !executionNode.Children.Any())
                    throw new ApplicationException("No lambda object found for [case]");

                signaler.Signal(executionNode.Name, executionNode);
            }
        }
    }
}
