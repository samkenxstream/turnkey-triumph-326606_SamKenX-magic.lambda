/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.logical
{
    [Slot(Name = "or")]
    public class Or : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() < 2)
                throw new ApplicationException("Operator [or] requires at least two children nodes");

            // Notice, to support short circuit evaluation, we cannot use same logic as we're using in [and]
            foreach (var idx in input.Children)
            {
                if (idx.Name.FirstOrDefault() != '.')
                    signaler.Signal(idx.Name, idx);

                if (idx.GetEx<bool>())
                {
                    input.Value = true;
                    return;
                }
            }
            input.Value = false;
        }
    }
}
