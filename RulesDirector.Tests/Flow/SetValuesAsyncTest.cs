using Contoso.Domain.Entities;
using Contoso.Test.Business.Requests;
using Contoso.Test.Flow;
using Contoso.Test.Flow.Cache;
using Contoso.Test.Flow.Rules;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace RulesDirector.Tests.Flow
{
    public class SetValuesAsyncTest
    {
        public SetValuesAsyncTest(ITestOutputHelper output)
        {
            this.output = output;
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        //[Fact]
        //public void SetValues()
        //{
        //    //arrange
        //    IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();

        //    //act
        //    System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
        //    flowManager.Start("setvaluesasync");
        //    stopWatch.Stop();
        //    this.output.WriteLine("Setting values async  = {0}", stopWatch.Elapsed.TotalMilliseconds);

        //    //assert
        //    Assert.Equal("A", flowManager.FlowDataCache.Items["A"].ToString());
        //    Assert.Equal("B", flowManager.FlowDataCache.Items["B"].ToString());
        //    Assert.Equal("C", flowManager.FlowDataCache.Items["C"].ToString());
        //    Assert.True(flowManager.FlowDataCache.Response.Success);
        //}

        #region Helpers
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddLogging
                (
                    loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.Services.AddSingleton<ILoggerProvider>
                        (
                            serviceProvider => new XUnitLoggerProvider(this.output)
                        );
                        loggingBuilder.AddFilter<XUnitLoggerProvider>("*", LogLevel.None);
                        loggingBuilder.AddFilter<XUnitLoggerProvider>("Contoso.Test.Flow", LogLevel.Trace);
                    }
                )
                .AddTransient<IFlowManager, FlowManager>()
                .AddTransient<FlowActivityFactory, FlowActivityFactory>()
                .AddTransient<DirectorFactory, DirectorFactory>()
                .AddTransient<ICustomActions, CustomActions>()
                .AddSingleton<FlowDataCache, FlowDataCache>()
                .AddSingleton<Progress, Progress>()
                .AddSingleton<IRulesCache>(sp =>
                {
                    return RulesService.LoadRules().GetAwaiter().GetResult();
                })
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
