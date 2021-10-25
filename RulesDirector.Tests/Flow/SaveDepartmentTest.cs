using Contoso.Domain.Entities;
using Contoso.Test.Business.Requests;
using Contoso.Test.Flow;
using Contoso.Test.Flow.Cache;
using Contoso.Test.Flow.Rules;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace RulesDirector.Tests.Flow
{
    public class SaveDepartmentTest
    {
        public SaveDepartmentTest(ITestOutputHelper output)
        {
            List<System.Reflection.Assembly> assemblies = new List<System.Reflection.Assembly>
            {
                typeof(Contoso.Test.Business.Requests.BaseRequest).Assembly,
                typeof(Contoso.Domain.BaseModelClass).Assembly,
                typeof(LogicBuilder.RulesDirector.DirectorBase).Assembly,
                typeof(string).Assembly
            };

            rulesCache = RulesService.LoadRulesSync(new RulesLoader());
            this.output = output;
            Initialize();
        }

        #region Fields
        private readonly IRulesCache rulesCache;
        private IServiceProvider serviceProvider;
        private readonly ITestOutputHelper output;
        #endregion Fields

        [Fact]
        public void SaveDepartment()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new DepartmentModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    InstructorID = 1,
                    Budget = 10000,
                    StartDate = new DateTime(2020,2,2),
                    Name = "Physics"
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savedepartment");
            stopWatch.Stop();
            this.output.WriteLine("Saving valid department  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public void SaveInvalidDepartment()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new DepartmentModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    InstructorID = null,
                    Budget = -1,
                    StartDate = new DateTime(),
                    Name = ""
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savedepartment");
            stopWatch.Stop();
            this.output.WriteLine("Saving invalid department  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.False(flowManager.FlowDataCache.Response.Success);
            Assert.Equal(4, flowManager.FlowDataCache.Response.ErrorMessages.Count);
        }

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
                    return rulesCache;
                })
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
