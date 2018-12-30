using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;

namespace LogicBuilder.RulesDirector
{
    public interface IRulesCache
    {
        RuleEngine GetRuleEngine(string ruleSet);

        Dictionary<string, RuleEngine> RuleEngines { get; }

        Dictionary<string, string> ResourceStrings { get; }
    }
}
