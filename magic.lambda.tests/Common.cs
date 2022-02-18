/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using magic.node;
using magic.node.contracts;
using magic.lambda.contracts;
using magic.signals.services;
using magic.signals.contracts;
using magic.node.extensions.hyperlambda;

namespace magic.lambda.tests
{
    public static class Common
    {
        [Slot(Name = "foo")]
        public class FooSlot : ISlot
        {
            public void Signal(ISignaler signaler, Node input)
            {
                input.Value = "OK";
            }
        }

        static public Node Evaluate(string hl, bool maxIterations = true)
        {
            var services = Initialize(maxIterations);
            var lambda = HyperlambdaParser.Parse(hl);
            var signaler = services.GetService(typeof(ISignaler)) as ISignaler;
            var evalResult = new Node();
            signaler.Scope("slots.result", evalResult, () =>
            {
                signaler.Signal("eval", lambda);
            });
            return lambda;
        }

        static async public Task<Node> EvaluateAsync(string hl)
        {
            var services = Initialize();
            var lambda = HyperlambdaParser.Parse(hl);
            var signaler = services.GetService(typeof(ISignaler)) as ISignaler;
            var evalResult = new Node();
            await signaler.ScopeAsync("slots.result", evalResult, async () =>
            {
                await signaler.SignalAsync("eval", lambda);
            });
            return lambda;
        }

        #region [ -- Private helper methods -- ]

        static IServiceProvider Initialize(bool maxIterations = true)
        {
            var services = new ServiceCollection();
            services.AddTransient<ISignaler, Signaler>();
            var types = new SignalsProvider(InstantiateAllTypes<ISlot>(services));
            services.AddTransient<ISignalsProvider>((svc) => types);
            var settingsMock = new Mock<IOptions<Settings>>();
            settingsMock.SetupGet(x => x.Value).Returns(new Settings
            {
                MaxWhileIterations = maxIterations ? 60 : 5000,
            });
            services.AddTransient<IOptions<Settings>>((svc) => settingsMock.Object);
            var provider = services.BuildServiceProvider();
            return provider;
        }

        static IEnumerable<Type> InstantiateAllTypes<T>(ServiceCollection services) where T : class
        {
            var type = typeof(T);
            var result = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic && !x.FullName.StartsWith("Microsoft", StringComparison.InvariantCulture))
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            foreach (var idx in result)
            {
                services.AddTransient(idx);
            }
            return result;
        }

        #endregion
    }
}
