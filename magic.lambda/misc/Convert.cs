/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Text;
using System.Linq;
using sys = System;
using System.Globalization;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.node.extensions.hyperlambda;

namespace magic.lambda.misc
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
            var value = input.GetEx<object>();
            var type = input.Children.FirstOrDefault()?
                .GetEx<string>() ??
                throw new HyperlambdaException("No [type] declaration found in invocation to [convert]");
            input.Clear(); // House cleaning.
            switch (type)
            {
                case "int":
                    input.Value = System.Convert.ToInt32(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "uint":
                    input.Value = System.Convert.ToUInt32(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "short":
                    input.Value = System.Convert.ToInt16(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "ushort":
                    input.Value = System.Convert.ToUInt16(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "long":
                    input.Value = System.Convert.ToInt64(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "ulong":
                    input.Value = System.Convert.ToUInt64(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "decimal":
                    input.Value = System.Convert.ToDecimal(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "double":
                    input.Value = System.Convert.ToDouble(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "single":
                case "float":
                    input.Value = System.Convert.ToSingle(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "bool":
                    input.Value = value?.Equals("true") ?? false;
                    break;

                case "date":
                    input.Value = DateTime.ParseExact(
                        value?.ToString() ?? DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                        "yyyy-MM-ddTHH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal).ToUniversalTime();
                    break;

                case "guid":
                    input.Value = new Guid(value?.ToString() ?? Guid.Empty.ToString());
                    break;

                case "char":
                    input.Value = System.Convert.ToChar(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "byte":
                    input.Value = System.Convert.ToByte(value ?? 0, CultureInfo.InvariantCulture);
                    break;

                case "x":
                    input.Value = new Expression(value?.ToString() ?? "");
                    break;

                case "bytes":
                    input.Value = Encoding.UTF8.GetBytes(value?.ToString() ?? "");
                    break;

                case "string":
                    if (value is byte[] bytes)
                        input.Value = Encoding.UTF8.GetString(bytes);
                    else
                        input.Value = value?.ToString() ?? "";
                    break;

                case "base64":
                    if (value is byte[] bytes2)
                        input.Value = sys.Convert.ToBase64String(bytes2);
                    else
                        throw new HyperlambdaException($"I don't know how to base64 encode {value}");
                    break;

                case "from-base64":
                    if (value is string strValue)
                        input.Value = sys.Convert.FromBase64String(strValue);
                    else
                        throw new HyperlambdaException($"I don't know how to base64 decode {value}");
                    break;

                case "node":
                    input.Value = HyperlambdaParser.Parse(value?.ToString() ?? "");
                    break;

                default:
                    throw new HyperlambdaException($"Unknown type '{type}' when invoking [convert]");
            }
        }
    }
}
