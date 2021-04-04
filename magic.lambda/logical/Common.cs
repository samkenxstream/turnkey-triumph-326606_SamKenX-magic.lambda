/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.logical
{
    /*
     * Common helper class for conditional slots, being [and] and [or].
     */
    internal static class Common
    {
        /*
         * Signals each children node of specified input node, until condition
         * is somehow returned from invocation. If condition is reached,
         * the method returns true - Otherwise it'll return false.
         */
        static internal bool Signal(ISignaler signaler, Node input, bool condition)
        {
            var whitelist = signaler.Peek<List<Node>>("whitelist");
            foreach (var idx in input.Children)
            {
                if (idx.Name != string.Empty && idx.Name.FirstOrDefault() != '.')
                {
                    if (whitelist != null && !whitelist.Any(x => x.Name == idx.Name))
                        throw new ArgumentException($"Slot [{idx.Name}] doesn't exist in currrent scope");
                    signaler.Signal(idx.Name, idx);
                }
                if (idx.GetEx<bool>() == condition)
                    return true;
            }
            return false;
        }

        /*
         * Same as above, only async version.
         */
        static internal async Task<bool> SignalAsync(ISignaler signaler, Node input, bool condition)
        {
            var whitelist = signaler.Peek<List<Node>>("whitelist");
            foreach (var idx in input.Children)
            {
                if (idx.Name.Any() && idx.Name.FirstOrDefault() != '.')
                {
                    if (whitelist != null && !whitelist.Any(x => x.Name == idx.Name))
                        throw new ArgumentException($"Slot [{idx.Name}] doesn't exist in currrent scope");
                    await signaler.SignalAsync(idx.Name, idx);
                }
                if (idx.GetEx<bool>() == condition)
                    return true;
            }
            return false;
        }
    }
}
