/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Globalization;
using magic.node;
using magic.node.extensions;
using magic.node.expressions;
using magic.signals.contracts;
using magic.node.extensions.hyperlambda;

namespace magic.lambda.change
{
    /// <summary>
    /// [convert] slot allowing you to convert values of nodes from one type to some other type.
    /// </summary>
    [Slot(Name = "convert")]
    public class Convert : ISlot
    {
        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            if (input.Children.Count() != 1 || !input.Children.Any(x => x.Name == "type"))
                throw new ApplicationException("[convert] can only handle one argument, which is [type]");

            var type = input.Children.First().Get<string>();

            var value = input.GetEx<object>();
            if (value == null)
            {
                input.Value = null;
                return;
            }

            switch (type)
            {
                case "int":
                    input.Value = System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    break;

                case "uint":
                    input.Value = System.Convert.ToUInt32(value, CultureInfo.InvariantCulture);
                    break;

                case "long":
                    input.Value = System.Convert.ToInt64(value, CultureInfo.InvariantCulture);
                    break;

                case "ulong":
                    input.Value = System.Convert.ToUInt64(value, CultureInfo.InvariantCulture);
                    break;

                case "decimal":
                    input.Value = System.Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    break;

                case "double":
                    input.Value = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    break;

                case "single":
                    input.Value = System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
                    break;

                case "bool":
                    input.Value = value.Equals("true");
                    break;

                case "date":
                    input.Value = DateTime.ParseExact(value.ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                    break;

                case "guid":
                    input.Value = new Guid(value.ToString());
                    break;

                case "char":
                    input.Value = System.Convert.ToChar(value, CultureInfo.InvariantCulture);
                    break;

                case "byte":
                    input.Value = System.Convert.ToByte(value, CultureInfo.InvariantCulture);
                    break;

                case "x":
                    input.Value = new Expression(value.ToString());
                    break;

                case "string":
                    input.Value = value.ToString();
                    break;

                case "node":
                    input.Value = new Parser(value.ToString()).Lambda();
                    break;

                default:
                    throw new ApplicationException($"Unknown type '{type}' when invoking [convert]");
            }
        }
    }
}
