using LogicBuilder.Workflow.Activities.Rules;
using System.Collections.Generic;

namespace LogicBuilder.RulesDirector
{
    public interface IRulesCache
    {
        RuleEngine GetRuleEngine(string ruleSet);

        IDictionary<string, RuleEngine> RuleEngines { get; }

        IDictionary<string, string> ResourceStrings { get; }
    }
}
