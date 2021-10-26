using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

[assembly: CLSCompliant(true)]
namespace LogicBuilder.RulesDirector
{
    [Serializable()]
    public abstract class DirectorBase
    {
        #region constants
        #endregion constants

        #region Variables
        #endregion Variables

        #region Properties
        public static bool VariablesUpdated
        {
            get { return false; }
        }
        #endregion Properties

        #region Flow Properties
        //driver
        [CLSCompliant(false)]
        protected virtual string _driver
        {
            get;
            set;
        }

        //selection
        [CLSCompliant(false)]
        protected virtual string _selection
        {
            get;
            set;
        }

        //callingModuleDriverStack
        [CLSCompliant(false)]
        protected virtual Stack _callingModuleDriverStack
        {
            get;
            set;
        }

        //callingModuleStack
        [CLSCompliant(false)]
        protected virtual Stack _callingModuleStack
        {
            get;
            set;
        }

        //moduleBeginName
        [CLSCompliant(false)]
        protected virtual string _moduleBeginName
        {
            get;
            set;
        }

        //moduleEndName
        [CLSCompliant(false)]
        protected virtual string _moduleEndName
        {
            get;
            set;
        }
        #endregion Flow Properties

        #region Rule Activation Properties
        /// <summary>
        /// This property is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Gets or Sets the condition that dictates which rule or set of rules should be activated
        /// </summary>
        public string Driver
        {
            get { return _driver; }
            set
            {
                _driver = value;
                UpdateProgressList();
            }
        }

        /// <summary>
        /// Gets or Sets the condition used for multiple choice functions in conjunction with the Driver Property
        /// to determine the active rule
        /// </summary>
        public string Selection
        {
            get { return _selection; }
            set
            {
                _selection = value;
            }
        }

        /// <summary>
        /// This property is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Gets the name of the current module or Sets the name of the newly invoked module
        /// </summary>
        public abstract string ModuleBeginName
        {
            get;
            set;
        }

        /// <summary>
        /// This property is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Gets the name of the most recently terminated module or
        /// Sets the name of the terminating module
        /// </summary>
        public abstract string ModuleEndName
        {
            get;
            set;
        }
        #endregion Rule Activation Properties

        #region Other Flow Properties
        /// <summary>
        /// The Rules Cache
        /// </summary>
        protected internal abstract IRulesCache RulesCache
        {
            get;
        }

        /// <summary>
        /// The object executed by the rules engine
        /// </summary>
        protected abstract IFlowActivity FlowActivity
        {
            get;
        }

        /// <summary>
        /// Gets a value specifying the module and shape that define the currently executing rules
        /// </summary>
        protected abstract Progress Progress
        {
            get;
        }
        #endregion Other Flow Properties

        #region Status Properties
        /// <summary>
        /// returns a description of the flow's progress
        /// </summary>
        public string FlowStatus
        {
            get
            {
                return this._driver.Length == 0
              ? string.Format(CultureInfo.CurrentCulture, Strings.flowStatusFormatInitial, ModuleName)
              : string.Format(CultureInfo.CurrentCulture, Strings.flowStatusFormat, ModuleName, CurrentPage, CurrentShapeIndex);
            }
        }

        /// <summary>
        /// The name of the current module
        /// </summary>
        private string ModuleName
        {
            get { return _moduleBeginName; }
        }

        /// <summary>
        /// Index of the index of the Visio page where the last rule activated is defined
        /// </summary>
        private string CurrentPage
        {
            get
            {
                int page = 0;
                if (_driver.Length == 0)
                    return page.ToString(CultureInfo.CurrentCulture);

                string[] driverValues = _driver.Split(new char[] { 'P' });
                return driverValues.Length < 2 ? page.ToString(CultureInfo.CurrentCulture) : driverValues[1];
            }
        }

        /// <summary>
        /// Index of the index of the Visio shape where the last rule activated is defined
        /// </summary>
        private string CurrentShapeIndex
        {
            get
            {
                int page = 0;
                if (_driver.Length == 0)
                    return page.ToString(CultureInfo.CurrentCulture);

                string[] driverValues = _driver.Split(new char[] { 'P' });
                return driverValues.Length < 2 ? page.ToString(CultureInfo.CurrentCulture) : driverValues[0];
            }
        }

        /// <summary>
        /// Gets the necessary current values to put on the backup stack
        /// </summary>
        /// <returns></returns>
        public object FlowBackupData
        {
            get
            {
                return new FlowBackupData(_driver, _selection, CopyStack(_callingModuleDriverStack), CopyStack(_callingModuleStack), _moduleBeginName, _moduleEndName);
            }
        }
        #endregion Status Properties

        #region Methods
        /// <summary>
        /// adds an item to the progress list
        /// </summary>
        internal protected void UpdateProgressList()
        {
            if (Progress.ProgressItems.Count > 0 && Progress.ProgressItems[Progress.ProgressItems.Count - 1].Equals(new ProgressInfo(FlowStatus)))
                return;

            Progress.AddProgressItem(FlowStatus);
        }

        /// <summary>
        /// adds initial module item to the progress list
        /// </summary>
        internal protected void UpdateProgressList(string module)
        {
            string item = string.Format(CultureInfo.CurrentCulture, Strings.flowStatusFormatInitial, module);
            if (Progress.ProgressItems.Count > 0 && Progress.ProgressItems[Progress.ProgressItems.Count - 1].Equals(new ProgressInfo(item)))
                return;

            Progress.AddProgressItem(item);
        }

        /// <summary>
        /// Sets the current module and executes the rules engine
        /// </summary>
        public abstract void StartInitialFlow(string module);

        /// <summary>
        /// Executes the current ruleset
        /// </summary>
        public abstract void ExecuteRulesEngine();

        /// <summary>
        /// Resets the current flow values given a FlowBackupData object
        /// </summary>
        /// <param name="flowBackupData"></param>
        public void ResetFlowValuesOnBackup(object fBackupData)
        {
            if (fBackupData == null)
                throw new DirectorException(string.Format(CultureInfo.CurrentCulture, Strings.invalidArgumentFormat, "{3509055C-DAA1-40c6-83AE-B53625605067}"));

            FlowBackupData flowBackupData = (FlowBackupData)fBackupData;
            this._driver = flowBackupData.Driver;
            this._selection = flowBackupData.Selection;
            this._callingModuleDriverStack = flowBackupData.CallingModuleDriverStack;
            this._callingModuleStack = flowBackupData.CallingModuleStack;
            this._moduleBeginName = flowBackupData.ModuleBeginName;
            this._moduleEndName = flowBackupData.ModuleEndName;
        }

        /// <summary>
        /// Creates a copy of the stack
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        private static Stack CopyStack(Stack stack)
        {
            object[] stackArray = stack.ToArray();
            Stack newStack = new Stack();
            for (int i = stackArray.Length - 1; i > -1; i--)
                newStack.Push(stackArray[i]);

            return newStack;
        }

        /// <summary>
        /// Sets the selection from the Selection Form and executes the rules engine
        /// </summary>
        /// <param name="selection"></param>
        public void SetSelection(string selection)
        {
            Selection = selection;
        }

        /// <summary>
        /// This property is used by the auto-generated rules and should not be used otherwise. It is called when the rule generated by a Flow Begin shape executes.
        /// </summary>
        /// <param name="moduleName"></param>
        public void SetModuleName(string moduleName)
        {
            this._moduleBeginName = moduleName;
        }

        protected virtual void ClearFlowVariables()
        {
            _callingModuleDriverStack = new System.Collections.Stack();
            _callingModuleStack = new System.Collections.Stack();
            _driver = string.Empty;
            _moduleBeginName = string.Empty;
            _moduleEndName = string.Empty;
            _selection = string.Empty;
        }
        #endregion Methods

        #region Required Override Functions
        /// <summary>
        /// Invoked when the rule activation properties are modified
        /// </summary>
        public abstract void SetCurrentBusinessBackupData();
        #endregion Required Override Functions
    }
}
