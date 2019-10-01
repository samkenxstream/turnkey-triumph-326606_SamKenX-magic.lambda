/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    // TODO: Replace with simply [apply] slot.
    /// <summary>
    /// [apply-file] slot allowing you to use a Hyperlambda file as a template for braiding together
    /// with variables of your own choosing.
    /// </summary>
    [Slot(Name = "apply-file")]
    public class ApplyFile : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Loading [template] and transforming into a lambda object.
            var templateFilename = input.GetEx<string>();
            var template = new Node("", templateFilename);
            signaler.Signal("load-file", template);
            signaler.Signal("lambda", template);

            // Retrieving all other arguments, and applying them to the template.
            var args = input.Children.Where(x => x.Name != "template");
            Apply(args, template.Children);

            // Returning transformed template to caller.
            input.Value = null;
            input.Clear();
            input.AddRange(template.Children.ToList());
        }

        #region [ -- Private helper methods -- ]

        void Apply(IEnumerable<Node> args, IEnumerable<Node> templateNodes)
        {
            foreach (var idx in templateNodes)
            {
                if (idx.Value is string strValue &&
                    strValue.StartsWith("{", StringComparison.InvariantCulture) &&
                    strValue.EndsWith("}", StringComparison.InvariantCulture))
                {
                    // Template variable, finding relevant node from args and applying
                    var templateName = strValue.Substring(1, strValue.Length - 2);
                    var argNode = args.FirstOrDefault(x => x.Name == templateName);
                    if (argNode == null)
                        throw new ApplicationException($"[template] file expected argument named [{templateName}] which was not given");

                    idx.Value = argNode.Value;
                    idx.AddRange(argNode.Children.Select(x => x.Clone()));
                }

                // Recursively invoking self
                Apply(args, idx.Children);
            }
        }

        #endregion
    }
}
