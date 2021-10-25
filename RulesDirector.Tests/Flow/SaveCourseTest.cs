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
    public class SaveCourseTest
    {
        public SaveCourseTest(ITestOutputHelper output)
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
        public void SaveCourse()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest 
            { 
                Entity = new CourseModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    CourseID = 1111,
                    Credits = 4,
                    DepartmentID = 2,
                    Title = "Chemistry"
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savecourse");
            stopWatch.Stop();
            this.output.WriteLine("Saving valid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

            //assert
            Assert.True(flowManager.FlowDataCache.Response.Success);
        }

        [Fact]
        public void SaveInvalidCourse()
        {
            //arrange
            IFlowManager flowManager = serviceProvider.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Request = new SaveEntityRequest
            {
                Entity = new CourseModel
                {
                    EntityState = LogicBuilder.Domain.EntityStateType.Modified,
                    CourseID = 0,
                    Credits = 6,
                    DepartmentID = 0,
                    Title = ""
                }
            };

            //act
            System.Diagnostics.Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            flowManager.Start("savecourse");
            stopWatch.Stop();
            this.output.WriteLine("Saving invalid course  = {0}", stopWatch.Elapsed.TotalMilliseconds);

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
