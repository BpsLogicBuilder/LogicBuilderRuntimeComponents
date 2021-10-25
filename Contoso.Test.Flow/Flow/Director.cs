using LogicBuilder.RulesDirector;
using System;
using System.Collections.Generic;

namespace Contoso.Test.Flow
{
    public class Director : AppDirectorBase
    {
        public Director(IFlowManager flowManager, IRulesCache rulesCache)
        {
            this.flowManager = flowManager;
            this.rulesCache = rulesCache;
        }

        #region Fields
        private readonly IFlowManager flowManager;
        private readonly IRulesCache rulesCache;
        #endregion Fields

        #region Properties
        protected override IRulesCache RulesCache => this.rulesCache;
        protected override IFlowActivity FlowActivity => this.flowManager.FlowActivity;
        protected override Progress Progress => this.flowManager.Progress;
        protected override Dictionary<int, int> QuestionListAnswers => throw new NotImplementedException();
        protected override Dictionary<int, object> InputQuestionsAnswers => throw new NotImplementedException();
        protected override Variables Variables => throw new NotImplementedException();
        #endregion Properties

        public override void SetCurrentBusinessBackupData() => this.flowManager.SetCurrentBusinessBackupData();
    }
}
