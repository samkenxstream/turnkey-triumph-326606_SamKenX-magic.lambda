/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.strings
{
    [Slot(Name = "replace")]
    public class Replace : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            var original = input.GetEx<string>();
            var what = input.Children.First(x => x.Name == "what").GetEx<string>();
            var with = input.Children.First(x => x.Name == "with").GetEx<string>();
            input.Value = original.Replace(what, with);
        }
    }
}
