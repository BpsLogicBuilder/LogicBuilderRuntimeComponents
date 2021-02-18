using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace LogicBuilder.RulesDirector.AspNetCore
{
    [Serializable()]
    abstract public class AspNetCoreDirectorBase : DirectorBase
    {
        public AspNetCoreDirectorBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.session = this.httpContextAccessor.HttpContext.Session;
        }
        #region Session Variable Names
        private const string SELECTION = "{2B83648C-B5A2-49e0-9467-0AB18737EAE0}";
        private const string DRIVER = "{77D313E2-2CA1-439e-9E31-D34E554E172D}";
        private const string DIALOGCLOSED = "{5E9BE4B6-6284-477f-81F3-D3D568B9721A}";
        private const string ISLICENSED = "{E86A136E-86B4-4ACF-B812-F7E9BD588C1B}";
        private const string CALLINGMODULEDRIVERSTACK = "{FD327E76-1AEF-4d37-971C-FEDB3265A1FA}";
        private const string CALLINGMODULESTACK = "{9718660D-269D-4d33-A71E-61BF58C5A0B4}";
        private const string MODULEBEGINNAME = "{02F5B96F-309B-4dee-B3DC-3B25E8A03471}";
        private const string MODULEENDNAME = "{F512B69F-EBE9-46a2-81B8-C64218C02D9A}";
        #endregion Session Variable Names

        #region Variables
        private ISession session;
        private IHttpContextAccessor httpContextAccessor;
        #endregion Variables

        #region Properties
        #endregion Properties

        #region Private Properties
        protected sealed override string _driver
        {
            get
            {
                if (session.Get<string>(DRIVER) == null)
                    session.Set<string>(DRIVER, string.Empty);
                return session.Get<string>(DRIVER);
            }
            set { session.Set<string>(DRIVER, value); }
        }

        protected sealed override string _selection
        {
            get
            {
                if (session.Get<string>(SELECTION) == null)
                    session.Set<string>(SELECTION, string.Empty);
                return session.Get<string>(SELECTION);
            }
            set { session.Set<string>(SELECTION, value); }
        }

        protected sealed override Stack _callingModuleDriverStack
        {
            get
            {
                if (session.Get<Stack>(CALLINGMODULEDRIVERSTACK) == null)
                    session.Set<Stack>(CALLINGMODULEDRIVERSTACK, new Stack());
                return session.Get<Stack>(CALLINGMODULEDRIVERSTACK);
            }
            set { session.Set<Stack>(CALLINGMODULEDRIVERSTACK, value); }
        }

        protected sealed override Stack _callingModuleStack
        {
            get
            {
                if (session.Get<Stack>(CALLINGMODULESTACK) == null)
                    session.Set<Stack>(CALLINGMODULESTACK, new Stack());
                return session.Get<Stack>(CALLINGMODULESTACK);
            }
            set { session.Set<Stack>(CALLINGMODULESTACK, value); }
        }

        protected sealed override string _moduleBeginName
        {
            get
            {
                if (session.Get<string>(MODULEBEGINNAME) == null)
                    session.Set<string>(MODULEBEGINNAME, string.Empty);
                return session.Get<string>(MODULEBEGINNAME);
            }
            set { session.Set<string>(MODULEBEGINNAME, value); }
        }

        protected sealed override string _moduleEndName
        {
            get
            {
                if (session.Get<string>(MODULEENDNAME) == null)
                    session.Set<string>(MODULEENDNAME, string.Empty);
                return session.Get<string>(MODULEENDNAME);
            }
            set { session.Set<string>(MODULEENDNAME, value); }
        }

        protected sealed override bool _dialogClosed
        {
            get { return session.Get<bool>(CALLINGMODULEDRIVERSTACK); }
            set { session.Set<bool>(CALLINGMODULEDRIVERSTACK, value); }
        }
        #endregion Private Properties

        #region Properties
        /// This property is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Gets the name of the current module or Sets the name of the newly invoked module
        /// </summary>
        public sealed override string ModuleBeginName
        {
            get { return _moduleBeginName; }
            set
            {
                _callingModuleStack.Push(_moduleBeginName);
                _moduleBeginName = value;
                if (_moduleBeginName.Length != 0)
                {
                    _callingModuleDriverStack.Push(this._driver);

                    this._driver = string.Empty;
                    this._moduleEndName = string.Empty;

                    UpdateProgressList();

                    RuleEngine ruleEngine = RulesCache.GetRuleEngine(_moduleBeginName);
                    if (ruleEngine == null)
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.moduleNotFoundFormat, _moduleBeginName));

                    ruleEngine.Execute(this.FlowActivity);
                }
            }
        }

        /// <summary>
        /// This property is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Gets the name of the most recently terminated module or
        /// Sets the name of the terminating module
        /// </summary>
        public sealed override string ModuleEndName
        {
            get { return _moduleEndName; }
            set
            {
                _moduleEndName = value;
                _moduleBeginName = _callingModuleStack.Pop().ToString();
                if (_moduleBeginName.Length != 0)
                {
                    this._driver = _callingModuleDriverStack.Pop().ToString();

                    UpdateProgressList();

                    RuleEngine ruleEngine = RulesCache.GetRuleEngine(_moduleBeginName);
                    if (ruleEngine == null)
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.moduleNotFoundFormat, _moduleBeginName));

                    ruleEngine.Execute(this.FlowActivity);
                }
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Executes the current ruleset
        /// </summary>
        public sealed override void ExecuteRulesEngine()
        {
            _executeRulesEngine();
        }

        private void _executeRulesEngine()
        {
            UpdateProgressList();

            RuleEngine ruleEngine = RulesCache.GetRuleEngine(_moduleBeginName);
            if (ruleEngine == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.moduleNotFoundFormat, _moduleBeginName));


            ruleEngine.Execute(this.FlowActivity);
        }

        /// <summary>
        /// Sets the current module and executes the rules engine
        /// </summary>
        public sealed override void StartInitialFlow(string module)
        {
            _startInitialFlow(module);
        }

        private void _startInitialFlow(string module)
        {
            if (string.IsNullOrEmpty(module))
                throw new ArgumentException(Strings.ruleSetCannotBeNull);

            ClearFlowVariables();
            ClearFlowSessionVariables();

            UpdateProgressList(module);

            RuleEngine ruleEngine = RulesCache.GetRuleEngine(module);
            if (ruleEngine == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.moduleNotFoundFormat, module));

            ruleEngine.Execute(this.FlowActivity);
        }

        /// <summary>
        /// Removes flow variables from the session
        /// </summary>
        public void ClearFlowSessionVariables()
        {
            _clearFlowSessionVariables();
        }

        private void _clearFlowSessionVariables()
        {
            session.Set<string>(DRIVER, null);
            session.Set<string>(SELECTION, null);
            session.Set<Stack>(CALLINGMODULEDRIVERSTACK, null);
            session.Set<Stack>(CALLINGMODULESTACK, null);
            session.Set<string>(MODULEBEGINNAME, null);
            session.Set<string>(MODULEENDNAME, null);
            session.Set<bool>(DIALOGCLOSED, false);
        }
        #endregion Methods
    }
}
