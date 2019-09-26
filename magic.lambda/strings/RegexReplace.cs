/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using System.Text.RegularExpressions;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.strings
{
    [Slot(Name = "regex-replace")]
    public class RegexReplace : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            var original = input.GetEx<string>();
            var what = input.Children.First(x => x.Name == "what").GetEx<string>();
            var with = input.Children.First(x => x.Name == "with").GetEx<string>();
            var ex = new Regex(what);
            input.Value = ex.Replace(original, with);
        }
    }
}
