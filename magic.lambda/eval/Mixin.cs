/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magic.node;
using magic.node.contracts;
using magic.node.extensions;
using magic.signals.contracts;
using magic.node.extensions.hyperlambda;

namespace magic.lambda.eval
{
    /// <summary>
    /// [mixin] slot for mixing a static content file with its associated Hyperlambda file.
    /// </summary>
    [Slot(Name = "mixin")]
    public class Mixin : ISlot, ISlotAsync
    {
        readonly IFileService _fileService;
        readonly IRootResolver _rootResolver;
        
        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="fileService">File service to use to resolve files</param>
        /// <param name="rootResolver">Root resolver used to find files with relative files paths</param>
        public Mixin(IFileService fileService, IRootResolver rootResolver)
        {
            _fileService = fileService;
            _rootResolver = rootResolver;
        }

        /// <summary>
        /// Implementation of signal
        /// </summary>
        /// <param name="signaler">Signaler used to signal</param>
        /// <param name="input">Parameters passed from signaler</param>
        public void Signal(ISignaler signaler, Node input)
        {
            SignalAsync(signaler, input).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Finding static file and Hyperlambda file, and performing some basic sanity checking.
            var staticFile = input.GetEx<string>() ?? throw new HyperlambdaException("No filename specified for [mixin]");
            var hlFile = staticFile.Substring(0, staticFile.LastIndexOf('.')) + ".hl";
            var lambda = new Node();
            if (await _fileService.ExistsAsync(_rootResolver.AbsolutePath(hlFile)))
                lambda = HyperlambdaParser.Parse(await _fileService.LoadAsync(_rootResolver.AbsolutePath(hlFile)));
            var staticContent = await _fileService.LoadAsync(_rootResolver.AbsolutePath(staticFile));

            // Executing mixin logic.
            input.Value = await MergeContent(signaler, staticContent, lambda, input);
        }

        #region [ -- Private helper methods -- ]

        /*
         * Parses file and combines results from lambda invocations with static content.
         */
        async Task<string> MergeContent(ISignaler signaler, string content, Node lambda, Node args)
        {
            var builder = new StringBuilder();
            using (var reader = new StringReader(content))
            {
                while (reader.Peek() != -1)
                {
                    var cur = (char)reader.Read();
                    switch (cur)
                    {
                        // Escaped character, probably an escaped '{'.
                        case '\\':
                            builder.Append((char)reader.Read());
                            break;

                        // Probably the beginning of a scope.
                        case '{':
                            var next = (char)reader.Read();
                            if (next != '{')
                            {
                                builder.Append(cur).Append(next);
                                continue;
                            }
                            var lambdaName = ReadToEndOfScope(reader);
                            builder.Append(await ExecuteLambda(signaler, lambda, lambdaName, args));
                            break;

                        default:
                            builder.Append(cur);
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        /*
         * Helper method to read to end of lambda reference
         */
        string ReadToEndOfScope(StringReader reader)
        {
            var builder = new StringBuilder();
            while (reader.Peek() != -1)
            {
                var cur = (char)reader.Read();
                switch (cur)
                {
                    // Escaped character, probably an escaped '}'.
                    case '\\':
                        builder.Append((char)reader.Read());
                        break;

                    // Probably the end of scope.
                    case '}':
                        var next = (char)reader.Read();
                        if (next != '}')
                        {
                            builder.Append(cur).Append(next);
                            continue;
                        }
                        return builder.ToString();

                    default:
                        builder.Append(cur);
                        break;
                }
            }
            throw new HyperlambdaException("Server side include was never closed!");
        }

        /*
         * Helper method to execute lambda object and return result of invocation to caller.
         */
        async Task<string> ExecuteLambda(ISignaler signaler, Node lambda, string lambdaName, Node args)
        {
            var exe = lambda.Children.FirstOrDefault(x => x.Name == lambdaName) ??
                throw new HyperlambdaException($"Couldn't find node named [{lambdaName}] in Hyperlambda codebehind file");
            var result = new Node();
            await signaler.ScopeAsync("slots.result", result, async () =>
            {
                var clone = exe.Clone();
                if (args != null && args.Clone().Children.Any())
                    clone.Insert(0, new Node(".arguments", null, args.Children.ToList()));
                await signaler.SignalAsync("eval", clone);
            });
            return result.GetEx<string>();
        }

        #endregion
    }
}
