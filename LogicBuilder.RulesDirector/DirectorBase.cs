/*Copyright © 2009 by Bonamy Taylor
 * */
using LogicBuilder.Workflow.Activities.Rules;
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

        //dialogClosed
        [CLSCompliant(false)]
        protected virtual bool _dialogClosed
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
                //SetCurrentBusinessBackupData();
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
                //SetCurrentBusinessBackupData();
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

        /// <summary>
        /// This property is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Gets or sets a value indicating whether the form invoked
        /// by a Dialog Function is closed
        /// </summary>
        public bool DialogClosed
        {
            get { return _dialogClosed; }
            set
            {
                //SetCurrentBusinessBackupData();
                _dialogClosed = value;
            }
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

        /// <summary>
        /// When overridden, holds a list of responses for each question in the current question list.
        /// TKey is the questionID and TValue is the responseID for the answer.
        /// </summary>
        protected abstract Dictionary<int, int> QuestionListAnswers
        {
            get;
        }

        /// <summary>
        /// When overridden, holds a list of responses for each question in the current input question list.
        /// TKey is the questionID and TValue is the response entered by the user at runtime.
        /// </summary>
        protected internal abstract Dictionary<int, object> InputQuestionsAnswers
        {
            get;
        }

        /// <summary>
        /// Gets or sets the automatic variables set i.e. the properties that do
        /// not have to be implemented by the developer.
        /// </summary>
        protected internal abstract Variables Variables
        {
            get;
            //set;
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
                return new FlowBackupData(_driver, _selection, _dialogClosed, CopyStack(_callingModuleDriverStack), CopyStack(_callingModuleStack), _moduleBeginName, _moduleEndName);
            }
        }
        #endregion Status Properties

        #region Methods
        /// <summary>
        /// This method is used by the auto-generated rules and should
        /// not be used otherwise. Given a questionId and answerId, determines
        /// whether the corresponding answer was selected by the runtime user.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answerId"></param>
        /// <returns></returns>
        public bool AnswerSelected(int questionId, int answerId)
        {
            if (!QuestionListAnswers.TryGetValue(questionId, out int answerIdStored))
                return false;

            return answerIdStored == answerId;
        }

        /// <summary>
        /// This method is used by the auto-generated rules and should
        /// not be used otherwise. Given a questionId, determines
        /// whether the corresponding answer is present.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answerId"></param>
        /// <returns></returns>
        public bool InputAnswerExists(int questionId)
        {
            return InputQuestionsAnswers.ContainsKey(questionId);
        }

        /// <summary>
        /// This method is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Empties the Dictionary<int, int> holding the questionId (TKey) and the
        /// corresponding answerId (TValue).
        /// </summary>
        public void ClearQuestionListAnswers()
        {
            QuestionListAnswers.Clear();
        }

        /// <summary>
        /// This method is used by the auto-generated rules and should
        /// not be used otherwise.
        /// Empties the Dictionary<int, object> holding the questionId (TKey) and the
        /// corresponding response (TValue).
        /// </summary>
        public void ClearInputQuestionsAnswers()
        {
            InputQuestionsAnswers.Clear();
        }

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

        /*/// <summary>
        /// Given the variable name, returns a list of string
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<string> GetListOfStringVariable(string key)
        {
            return VariablesHelper<IList<string>>.GetVariable(key, this, VariableTypes.ListOfStringType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of strings to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfStringVariable(string key, IList<string> newValue)
        {
            VariablesHelper<IList<string>>.SetVariable(key, newValue, this, VariableTypes.ListOfStringType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Byte?> GetListOfNullableByteVariable(string key)
        {
            return VariablesHelper<IList<Byte?>>.GetVariable(key, this, VariableTypes.ListOfNullableByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableByteVariable(string key, IList<Byte?> newValue)
        {
            VariablesHelper<IList<Byte?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableByteType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<SByte?> GetListOfNullableSByteVariable(string key)
        {
            return VariablesHelper<IList<SByte?>>.GetVariable(key, this, VariableTypes.ListOfNullableSByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfNullableSByteVariable(string key, IList<SByte?> newValue)
        {
            VariablesHelper<IList<SByte?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableSByteType);
        }

        /// <summary>
        /// Given the variable name, returns a list of character
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Char?> GetListOfNullableCharVariable(string key)
        {
            return VariablesHelper<IList<Char?>>.GetVariable(key, this, VariableTypes.ListOfNullableCharType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of characters to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableCharVariable(string key, IList<Char?> newValue)
        {
            VariablesHelper<IList<Char?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableCharType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<UInt16?> GetListOfNullableUShortVariable(string key)
        {
            return VariablesHelper<IList<UInt16?>>.GetVariable(key, this, VariableTypes.ListOfNullableUShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfNullableUShortVariable(string key, IList<UInt16?> newValue)
        {
            VariablesHelper<IList<UInt16?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableUShortType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Int16?> GetListOfNullableShortVariable(string key)
        {
            return VariablesHelper<IList<Int16?>>.GetVariable(key, this, VariableTypes.ListOfNullableShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableShortVariable(string key, IList<Int16?> newValue)
        {
            VariablesHelper<IList<Int16?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableShortType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<UInt32?> GetListOfNullableUIntegerVariable(string key)
        {
            return VariablesHelper<IList<UInt32?>>.GetVariable(key, this, VariableTypes.ListOfNullableUIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfNullableUIntegerVariable(string key, IList<UInt32?> newValue)
        {
            VariablesHelper<IList<UInt32?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableUIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Int32?> GetListOfNullableIntegerVariable(string key)
        {
            return VariablesHelper<IList<Int32?>>.GetVariable(key, this, VariableTypes.ListOfNullableIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableIntegerVariable(string key, IList<Int32?> newValue)
        {
            VariablesHelper<IList<Int32?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<UInt64?> GetListOfNullableULongVariable(string key)
        {
            return VariablesHelper<IList<UInt64?>>.GetVariable(key, this, VariableTypes.ListOfNullableULongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfNullableULongVariable(string key, IList<UInt64?> newValue)
        {
            VariablesHelper<IList<UInt64?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableULongType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Int64?> GetListOfNullableLongVariable(string key)
        {
            return VariablesHelper<IList<Int64?>>.GetVariable(key, this, VariableTypes.ListOfNullableLongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableLongVariable(string key, IList<Int64?> newValue)
        {
            VariablesHelper<IList<Int64?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableLongType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Single?> GetListOfNullableFloatVariable(string key)
        {
            return VariablesHelper<IList<Single?>>.GetVariable(key, this, VariableTypes.ListOfNullableFloatType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableFloatVariable(string key, IList<Single?> newValue)
        {
            VariablesHelper<IList<Single?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableFloatType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Double?> GetListOfNullableDoubleVariable(string key)
        {
            return VariablesHelper<IList<Double?>>.GetVariable(key, this, VariableTypes.ListOfNullableDoubleType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableDoubleVariable(string key, IList<Double?> newValue)
        {
            VariablesHelper<IList<Double?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableDoubleType);
        }

        /// <summary>
        /// Given the variable name, returns a list of decimal
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<decimal?> GetListOfNullableDecimalVariable(string key)
        {
            return VariablesHelper<IList<decimal?>>.GetVariable(key, this, VariableTypes.ListOfNullableDecimalType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of decimals to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableDecimalVariable(string key, IList<decimal?> newValue)
        {
            VariablesHelper<IList<decimal?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableDecimalType);
        }

        /// <summary>
        /// Given the variable name, returns a list of DateTime
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<DateTime?> GetListOfNullableDateTimeVariable(string key)
        {
            return VariablesHelper<IList<DateTime?>>.GetVariable(key, this, VariableTypes.ListOfNullableDateTimeType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of DateTime values to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableDateTimeVariable(string key, IList<DateTime?> newValue)
        {
            VariablesHelper<IList<DateTime?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableDateTimeType);
        }

        /// <summary>
        /// Given the variable name, returns a list of TimeSpan
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<TimeSpan?> GetListOfNullableTimeSpanVariable(string key)
        {
            return VariablesHelper<IList<TimeSpan?>>.GetVariable(key, this, VariableTypes.ListOfNullableTimeSpanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of TimeSpan values to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableTimeSpanVariable(string key, IList<TimeSpan?> newValue)
        {
            VariablesHelper<IList<TimeSpan?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableTimeSpanType);
        }

        /// <summary>
        /// Given the variable name, returns a list of Guid
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Guid?> GetListOfNullableGuidVariable(string key)
        {
            return VariablesHelper<IList<Guid?>>.GetVariable(key, this, VariableTypes.ListOfNullableGuidType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of Guid values to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableGuidVariable(string key, IList<Guid?> newValue)
        {
            VariablesHelper<IList<Guid?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableGuidType);
        }

        /// <summary>
        /// Given the variable name, returns a list of boolean
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<bool?> GetListOfNullableBoolVariable(string key)
        {
            return VariablesHelper<IList<bool?>>.GetVariable(key, this, VariableTypes.ListOfNullableBooleanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of booleans to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfNullableBoolVariable(string key, IList<bool?> newValue)
        {
            VariablesHelper<IList<bool?>>.SetVariable(key, newValue, this, VariableTypes.ListOfNullableBooleanType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Byte> GetListOfByteVariable(string key)
        {
            return VariablesHelper<IList<Byte>>.GetVariable(key, this, VariableTypes.ListOfByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfByteVariable(string key, IList<Byte> newValue)
        {
            VariablesHelper<IList<Byte>>.SetVariable(key, newValue, this, VariableTypes.ListOfByteType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<SByte> GetListOfSByteVariable(string key)
        {
            return VariablesHelper<IList<SByte>>.GetVariable(key, this, VariableTypes.ListOfSByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfSByteVariable(string key, IList<SByte> newValue)
        {
            VariablesHelper<IList<SByte>>.SetVariable(key, newValue, this, VariableTypes.ListOfSByteType);
        }

        /// <summary>
        /// Given the variable name, returns a list of character
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Char> GetListOfCharVariable(string key)
        {
            return VariablesHelper<IList<Char>>.GetVariable(key, this, VariableTypes.ListOfCharType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of characters to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfCharVariable(string key, IList<Char> newValue)
        {
            VariablesHelper<IList<Char>>.SetVariable(key, newValue, this, VariableTypes.ListOfCharType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<UInt16> GetListOfUShortVariable(string key)
        {
            return VariablesHelper<IList<UInt16>>.GetVariable(key, this, VariableTypes.ListOfUShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfUShortVariable(string key, IList<UInt16> newValue)
        {
            VariablesHelper<IList<UInt16>>.SetVariable(key, newValue, this, VariableTypes.ListOfUShortType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Int16> GetListOfShortVariable(string key)
        {
            return VariablesHelper<IList<Int16>>.GetVariable(key, this, VariableTypes.ListOfShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfShortVariable(string key, IList<Int16> newValue)
        {
            VariablesHelper<IList<Int16>>.SetVariable(key, newValue, this, VariableTypes.ListOfShortType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<UInt32> GetListOfUIntegerVariable(string key)
        {
            return VariablesHelper<IList<UInt32>>.GetVariable(key, this, VariableTypes.ListOfUIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfUIntegerVariable(string key, IList<UInt32> newValue)
        {
            VariablesHelper<IList<UInt32>>.SetVariable(key, newValue, this, VariableTypes.ListOfUIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Int32> GetListOfIntegerVariable(string key)
        {
            return VariablesHelper<IList<Int32>>.GetVariable(key, this, VariableTypes.ListOfIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfIntegerVariable(string key, IList<Int32> newValue)
        {
            VariablesHelper<IList<Int32>>.SetVariable(key, newValue, this, VariableTypes.ListOfIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public IList<UInt64> GetListOfULongVariable(string key)
        {
            return VariablesHelper<IList<UInt64>>.GetVariable(key, this, VariableTypes.ListOfULongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetListOfULongVariable(string key, IList<UInt64> newValue)
        {
            VariablesHelper<IList<UInt64>>.SetVariable(key, newValue, this, VariableTypes.ListOfULongType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Int64> GetListOfLongVariable(string key)
        {
            return VariablesHelper<IList<Int64>>.GetVariable(key, this, VariableTypes.ListOfLongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfLongVariable(string key, IList<Int64> newValue)
        {
            VariablesHelper<IList<Int64>>.SetVariable(key, newValue, this, VariableTypes.ListOfLongType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Single> GetListOfFloatVariable(string key)
        {
            return VariablesHelper<IList<Single>>.GetVariable(key, this, VariableTypes.ListOfFloatType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfFloatVariable(string key, IList<Single> newValue)
        {
            VariablesHelper<IList<Single>>.SetVariable(key, newValue, this, VariableTypes.ListOfFloatType);
        }

        /// <summary>
        /// Given the variable name, returns a list of number
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Double> GetListOfDoubleVariable(string key)
        {
            return VariablesHelper<IList<Double>>.GetVariable(key, this, VariableTypes.ListOfDoubleType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of numbers to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfDoubleVariable(string key, IList<Double> newValue)
        {
            VariablesHelper<IList<Double>>.SetVariable(key, newValue, this, VariableTypes.ListOfDoubleType);
        }

        /// <summary>
        /// Given the variable name, returns a list of decimal
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<decimal> GetListOfDecimalVariable(string key)
        {
            return VariablesHelper<IList<decimal>>.GetVariable(key, this, VariableTypes.ListOfDecimalType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of decimals to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfDecimalVariable(string key, IList<decimal> newValue)
        {
            VariablesHelper<IList<decimal>>.SetVariable(key, newValue, this, VariableTypes.ListOfDecimalType);
        }

        /// <summary>
        /// Given the variable name, returns a list of DateTime
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<DateTime> GetListOfDateTimeVariable(string key)
        {
            return VariablesHelper<IList<DateTime>>.GetVariable(key, this, VariableTypes.ListOfDateTimeType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of DateTime values to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfDateTimeVariable(string key, IList<DateTime> newValue)
        {
            VariablesHelper<IList<DateTime>>.SetVariable(key, newValue, this, VariableTypes.ListOfDateTimeType);
        }

        /// <summary>
        /// Given the variable name, returns a list of TimeSpan
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<TimeSpan> GetListOfTimeSpanVariable(string key)
        {
            return VariablesHelper<IList<TimeSpan>>.GetVariable(key, this, VariableTypes.ListOfTimeSpanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of TimeSpan values to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfTimeSpanVariable(string key, IList<TimeSpan> newValue)
        {
            VariablesHelper<IList<TimeSpan>>.SetVariable(key, newValue, this, VariableTypes.ListOfTimeSpanType);
        }

        /// <summary>
        /// Given the variable name, returns a list of Guid
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<Guid> GetListOfGuidVariable(string key)
        {
            return VariablesHelper<IList<Guid>>.GetVariable(key, this, VariableTypes.ListOfGuidType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of Guid values to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfGuidVariable(string key, IList<Guid> newValue)
        {
            VariablesHelper<IList<Guid>>.SetVariable(key, newValue, this, VariableTypes.ListOfGuidType);
        }

        /// <summary>
        /// Given the variable name, returns a list of boolean
        /// variables from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public IList<bool> GetListOfBoolVariable(string key)
        {
            return VariablesHelper<IList<bool>>.GetVariable(key, this, VariableTypes.ListOfBooleanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a list of booleans to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetListOfBoolVariable(string key, IList<bool> newValue)
        {
            VariablesHelper<IList<bool>>.SetVariable(key, newValue, this, VariableTypes.ListOfBooleanType);
        }

        /// <summary>
        /// Given the variable name, returns a string
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public string GetStringVariable(string key)
        {
            return VariablesHelper<string>.GetVariable(key, this, VariableTypes.StringType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a string to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetStringVariable(string key, string newValue)
        {
            VariablesHelper<string>.SetVariable(key, newValue, this, VariableTypes.StringType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Byte? GetNullableByteVariable(string key)
        {
            return VariablesHelper<Byte?>.GetVariable(key, this, VariableTypes.NullableByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableByteVariable(string key, Byte? newValue)
        {
            VariablesHelper<Byte?>.SetVariable(key, newValue, this, VariableTypes.NullableByteType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public SByte? GetNullableSByteVariable(string key)
        {
            return VariablesHelper<SByte?>.GetVariable(key, this, VariableTypes.NullableSByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetNullableSByteVariable(string key, SByte? newValue)
        {
            VariablesHelper<SByte?>.SetVariable(key, newValue, this, VariableTypes.NullableSByteType);
        }

        /// <summary>
        /// Given the variable name, returns a character
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Char? GetNullableCharVariable(string key)
        {
            return VariablesHelper<Char?>.GetVariable(key, this, VariableTypes.NullableCharType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a character to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableCharVariable(string key, Char? newValue)
        {
            VariablesHelper<Char?>.SetVariable(key, newValue, this, VariableTypes.NullableCharType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public UInt16? GetNullableUShortVariable(string key)
        {
            return VariablesHelper<UInt16?>.GetVariable(key, this, VariableTypes.NullableUShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetNullableUShortVariable(string key, UInt16? newValue)
        {
            VariablesHelper<UInt16?>.SetVariable(key, newValue, this, VariableTypes.NullableUShortType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Int16? GetNullableShortVariable(string key)
        {
            return VariablesHelper<Int16?>.GetVariable(key, this, VariableTypes.NullableShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableShortVariable(string key, Int16? newValue)
        {
            VariablesHelper<Int16?>.SetVariable(key, newValue, this, VariableTypes.NullableShortType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public UInt32? GetNullableUIntegerVariable(string key)
        {
            return VariablesHelper<UInt32?>.GetVariable(key, this, VariableTypes.NullableUIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetNullableUIntegerVariable(string key, UInt32? newValue)
        {
            VariablesHelper<UInt32?>.SetVariable(key, newValue, this, VariableTypes.NullableUIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Int32? GetNullableIntegerVariable(string key)
        {
            return VariablesHelper<Int32?>.GetVariable(key, this, VariableTypes.NullableIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableIntegerVariable(string key, Int32? newValue)
        {
            VariablesHelper<Int32?>.SetVariable(key, newValue, this, VariableTypes.NullableIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public UInt64? GetNullableULongVariable(string key)
        {
            return VariablesHelper<UInt64?>.GetVariable(key, this, VariableTypes.NullableULongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetNullableULongVariable(string key, UInt64? newValue)
        {
            VariablesHelper<UInt64?>.SetVariable(key, newValue, this, VariableTypes.NullableULongType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Int64? GetNullableLongVariable(string key)
        {
            return VariablesHelper<Int64?>.GetVariable(key, this, VariableTypes.NullableLongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableLongVariable(string key, Int64? newValue)
        {
            VariablesHelper<Int64?>.SetVariable(key, newValue, this, VariableTypes.NullableLongType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Single? GetNullableFloatVariable(string key)
        {
            return VariablesHelper<Single?>.GetVariable(key, this, VariableTypes.NullableFloatType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableFloatVariable(string key, Single? newValue)
        {
            VariablesHelper<Single?>.SetVariable(key, newValue, this, VariableTypes.NullableFloatType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Double? GetNullableDoubleVariable(string key)
        {
            return VariablesHelper<Double?>.GetVariable(key, this, VariableTypes.NullableDoubleType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableDoubleVariable(string key, Double? newValue)
        {
            VariablesHelper<Double?>.SetVariable(key, newValue, this, VariableTypes.NullableDoubleType);
        }

        /// <summary>
        /// Given the variable name, returns a decimal
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public decimal? GetNullableDecimalVariable(string key)
        {
            return VariablesHelper<decimal?>.GetVariable(key, this, VariableTypes.NullableDecimalType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a decimal to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableDecimalVariable(string key, decimal? newValue)
        {
            VariablesHelper<decimal?>.SetVariable(key, newValue, this, VariableTypes.NullableDecimalType);
        }

        /// <summary>
        /// Given the variable name, returns a DateTime
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public DateTime? GetNullableDateTimeVariable(string key)
        {
            return VariablesHelper<DateTime?>.GetVariable(key, this, VariableTypes.NullableDateTimeType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a DateTime value to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableDateTimeVariable(string key, DateTime? newValue)
        {
            VariablesHelper<DateTime?>.SetVariable(key, newValue, this, VariableTypes.NullableDateTimeType);
        }

        /// <summary>
        /// Given the variable name, returns a TimeSpan
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public TimeSpan? GetNullableTimeSpanVariable(string key)
        {
            return VariablesHelper<TimeSpan?>.GetVariable(key, this, VariableTypes.NullableTimeSpanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a TimeSpan value to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableTimeSpanVariable(string key, TimeSpan? newValue)
        {
            VariablesHelper<TimeSpan?>.SetVariable(key, newValue, this, VariableTypes.NullableTimeSpanType);
        }

        /// <summary>
        /// Given the variable name, returns a Guid
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Guid? GetNullableGuidVariable(string key)
        {
            return VariablesHelper<Guid?>.GetVariable(key, this, VariableTypes.NullableGuidType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a Guid value to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableGuidVariable(string key, Guid? newValue)
        {
            VariablesHelper<Guid?>.SetVariable(key, newValue, this, VariableTypes.NullableGuidType);
        }

        /// <summary>
        /// Given the variable name, returns a boolean
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public bool? GetNullableBoolVariable(string key)
        {
            return VariablesHelper<bool?>.GetVariable(key, this, VariableTypes.NullableBooleanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a boolean to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetNullableBoolVariable(string key, bool? newValue)
        {
            VariablesHelper<bool?>.SetVariable(key, newValue, this, VariableTypes.NullableBooleanType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Byte GetByteVariable(string key)
        {
            return VariablesHelper<Byte>.GetVariable(key, this, VariableTypes.ByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetByteVariable(string key, Byte newValue)
        {
            VariablesHelper<Byte>.SetVariable(key, newValue, this, VariableTypes.ByteType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public SByte GetSByteVariable(string key)
        {
            return VariablesHelper<SByte>.GetVariable(key, this, VariableTypes.SByteType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetSByteVariable(string key, SByte newValue)
        {
            VariablesHelper<SByte>.SetVariable(key, newValue, this, VariableTypes.SByteType);
        }

        /// <summary>
        /// Given the variable name, returns a character
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Char GetCharVariable(string key)
        {
            return VariablesHelper<Char>.GetVariable(key, this, VariableTypes.CharType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a character to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetCharVariable(string key, Char newValue)
        {
            VariablesHelper<Char>.SetVariable(key, newValue, this, VariableTypes.CharType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public UInt16 GetUShortVariable(string key)
        {
            return VariablesHelper<UInt16>.GetVariable(key, this, VariableTypes.UShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetUShortVariable(string key, UInt16 newValue)
        {
            VariablesHelper<UInt16>.SetVariable(key, newValue, this, VariableTypes.UShortType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Int16 GetShortVariable(string key)
        {
            return VariablesHelper<Int16>.GetVariable(key, this, VariableTypes.ShortType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetShortVariable(string key, Int16 newValue)
        {
            VariablesHelper<Int16>.SetVariable(key, newValue, this, VariableTypes.ShortType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public UInt32 GetUIntegerVariable(string key)
        {
            return VariablesHelper<UInt32>.GetVariable(key, this, VariableTypes.UIntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetUIntegerVariable(string key, UInt32 newValue)
        {
            VariablesHelper<UInt32>.SetVariable(key, newValue, this, VariableTypes.UIntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Int32 GetIntegerVariable(string key)
        {
            return VariablesHelper<Int32>.GetVariable(key, this, VariableTypes.IntegerType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetIntegerVariable(string key, Int32 newValue)
        {
            VariablesHelper<Int32>.SetVariable(key, newValue, this, VariableTypes.IntegerType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        [CLSCompliant(false)]
        public UInt64 GetULongVariable(string key)
        {
            return VariablesHelper<UInt64>.GetVariable(key, this, VariableTypes.ULongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        [CLSCompliant(false)]
        public void SetULongVariable(string key, UInt64 newValue)
        {
            VariablesHelper<UInt64>.SetVariable(key, newValue, this, VariableTypes.ULongType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Int64 GetLongVariable(string key)
        {
            return VariablesHelper<Int64>.GetVariable(key, this, VariableTypes.LongType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetLongVariable(string key, Int64 newValue)
        {
            VariablesHelper<Int64>.SetVariable(key, newValue, this, VariableTypes.LongType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Single GetFloatVariable(string key)
        {
            return VariablesHelper<Single>.GetVariable(key, this, VariableTypes.FloatType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetFloatVariable(string key, Single newValue)
        {
            VariablesHelper<Single>.SetVariable(key, newValue, this, VariableTypes.FloatType);
        }

        /// <summary>
        /// Given the variable name, returns a number
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Double GetDoubleVariable(string key)
        {
            return VariablesHelper<Double>.GetVariable(key, this, VariableTypes.DoubleType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a number to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetDoubleVariable(string key, Double newValue)
        {
            VariablesHelper<Double>.SetVariable(key, newValue, this, VariableTypes.DoubleType);
        }

        /// <summary>
        /// Given the variable name, returns a decimal
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public decimal GetDecimalVariable(string key)
        {
            return VariablesHelper<decimal>.GetVariable(key, this, VariableTypes.DecimalType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a decimal to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetDecimalVariable(string key, decimal newValue)
        {
            VariablesHelper<decimal>.SetVariable(key, newValue, this, VariableTypes.DecimalType);
        }

        /// <summary>
        /// Given the variable name, returns a DateTime
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public DateTime GetDateTimeVariable(string key)
        {
            return VariablesHelper<DateTime>.GetVariable(key, this, VariableTypes.DateTimeType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a DateTime value to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetDateTimeVariable(string key, DateTime newValue)
        {
            VariablesHelper<DateTime>.SetVariable(key, newValue, this, VariableTypes.DateTimeType);
        }

        /// <summary>
        /// Given the variable name, returns a TimeSpan
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public TimeSpan GetTimeSpanVariable(string key)
        {
            return VariablesHelper<TimeSpan>.GetVariable(key, this, VariableTypes.TimeSpanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a TimeSpan value to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetTimeSpanVariable(string key, TimeSpan newValue)
        {
            VariablesHelper<TimeSpan>.SetVariable(key, newValue, this, VariableTypes.TimeSpanType);
        }

        /// <summary>
        /// Given the variable name, returns a Guid
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public Guid GetGuidVariable(string key)
        {
            return VariablesHelper<Guid>.GetVariable(key, this, VariableTypes.GuidType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a Guid value to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetGuidVariable(string key, Guid newValue)
        {
            VariablesHelper<Guid>.SetVariable(key, newValue, this, VariableTypes.GuidType);
        }

        /// <summary>
        /// Given the variable name, returns a boolean
        /// variable from automatic variables set.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [RuleRead("VariablesUpdated")]
        public bool GetBoolVariable(string key)
        {
            return VariablesHelper<bool>.GetVariable(key, this, VariableTypes.BooleanType);
        }

        /// <summary>
        /// Given the variable name and value, updates or
        /// adds a boolean to the automatic variables set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        [RuleWrite("VariablesUpdated")]
        public void SetBoolVariable(string key, bool newValue)
        {
            VariablesHelper<bool>.SetVariable(key, newValue, this, VariableTypes.BooleanType);
        }*/

        /// <summary>
        /// Given a dictionary of name and VariableInfo pairs, updates or
        /// adds the variables to the automatic variables set
        /// </summary>
        /// <param name="values"></param>
        protected void SetVariableValues(Dictionary<string, VariableInfo> values)
        {
            Variables.SetValues(values);
        }

        /*/// <summary>
        /// given the index, gets a boolean resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Boolean GetBooleanResource(string shortValue)
        {
            return ResourcesHelper<Boolean>.GetResource(shortValue, this, VariableTypes.BooleanType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Byte GetByteResource(string shortValue)
        {
            return ResourcesHelper<Byte>.GetResource(shortValue, this, VariableTypes.ByteType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public SByte GetSByteResource(string shortValue)
        {
            return ResourcesHelper<SByte>.GetResource(shortValue, this, VariableTypes.SByteType);
        }

        /// <summary>
        /// given the index, gets a character resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Char GetCharResource(string shortValue)
        {
            return ResourcesHelper<Char>.GetResource(shortValue, this, VariableTypes.CharType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt16 GetUShortResource(string shortValue)
        {
            return ResourcesHelper<UInt16>.GetResource(shortValue, this, VariableTypes.UShortType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Int16 GetShortResource(string shortValue)
        {
            return ResourcesHelper<Int16>.GetResource(shortValue, this, VariableTypes.ShortType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt32 GetUIntegerResource(string shortValue)
        {
            return ResourcesHelper<UInt32>.GetResource(shortValue, this, VariableTypes.UIntegerType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Int32 GetIntegerResource(string shortValue)
        {
            return ResourcesHelper<Int32>.GetResource(shortValue, this, VariableTypes.IntegerType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt64 GetULongResource(string shortValue)
        {
            return ResourcesHelper<UInt64>.GetResource(shortValue, this, VariableTypes.ULongType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Int64 GetLongResource(string shortValue)
        {
            return ResourcesHelper<Int64>.GetResource(shortValue, this, VariableTypes.LongType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Single GetFloatResource(string shortValue)
        {
            return ResourcesHelper<Single>.GetResource(shortValue, this, VariableTypes.FloatType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Double GetDoubleResource(string shortValue)
        {
            return ResourcesHelper<Double>.GetResource(shortValue, this, VariableTypes.DoubleType);
        }

        /// <summary>
        /// given the index, gets a number resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Decimal GetDecimalResource(string shortValue)
        {
            return ResourcesHelper<Decimal>.GetResource(shortValue, this, VariableTypes.DecimalType);
        }

        /// <summary>
        /// given the index, gets a DateTime resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public DateTime GetDateTimeResource(string shortValue)
        {
            return ResourcesHelper<DateTime>.GetResource(shortValue, this, VariableTypes.DateTimeType);
        }

        /// <summary>
        /// given the index, gets a TimeSpan resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public TimeSpan GetTimeSpanResource(string shortValue)
        {
            return ResourcesHelper<TimeSpan>.GetResource(shortValue, this, VariableTypes.TimeSpanType);
        }

        /// <summary>
        /// given the index, gets a Guid resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public Guid GetGuidResource(string shortValue)
        {
            return ResourcesHelper<Guid>.GetResource(shortValue, this, VariableTypes.GuidType);
        }

        /// <summary>
        /// given the index, gets a string resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <returns></returns>
        public string GetStringResource(string shortValue)
        {
            return RulesCache.ResourceStrings.TryGetValue(shortValue, out string longValue) ? longValue : shortValue;
        }

        /// <summary>
        /// given the index and type, gets a resource from the auto-generated resource set.
        /// </summary>
        /// <param name="shortValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetResource(string shortValue, string type)
        {
            switch (type)
            {
                case VariableTypes.BooleanType:
                case VariableTypes.NullableBooleanType:
                    return GetBooleanResource(shortValue);
                case VariableTypes.ByteType:
                case VariableTypes.NullableByteType:
                    return GetByteResource(shortValue);
                case VariableTypes.CharType:
                case VariableTypes.NullableCharType:
                    return GetCharResource(shortValue);
                case VariableTypes.DateTimeType:
                case VariableTypes.NullableDateTimeType:
                    return GetDateTimeResource(shortValue);
                case VariableTypes.TimeSpanType:
                case VariableTypes.NullableTimeSpanType:
                    return GetTimeSpanResource(shortValue);
                case VariableTypes.GuidType:
                case VariableTypes.NullableGuidType:
                    return GetGuidResource(shortValue);
                case VariableTypes.DecimalType:
                case VariableTypes.NullableDecimalType:
                    return GetDecimalResource(shortValue);
                case VariableTypes.DoubleType:
                case VariableTypes.NullableDoubleType:
                    return GetDoubleResource(shortValue);
                case VariableTypes.FloatType:
                case VariableTypes.NullableFloatType:
                    return GetFloatResource(shortValue);
                case VariableTypes.IntegerType:
                case VariableTypes.NullableIntegerType:
                    return GetIntegerResource(shortValue);
                case VariableTypes.LongType:
                case VariableTypes.NullableLongType:
                    return GetLongResource(shortValue);
                case VariableTypes.SByteType:
                case VariableTypes.NullableSByteType:
                    return GetSByteResource(shortValue);
                case VariableTypes.ShortType:
                case VariableTypes.NullableShortType:
                    return GetShortResource(shortValue);
                case VariableTypes.UIntegerType:
                case VariableTypes.NullableUIntegerType:
                    return GetUIntegerResource(shortValue);
                case VariableTypes.ULongType:
                case VariableTypes.NullableULongType:
                    return GetULongResource(shortValue);
                case VariableTypes.UShortType:
                case VariableTypes.NullableUShortType:
                    return GetUShortResource(shortValue);
                case VariableTypes.StringType:
                    return GetStringResource(shortValue);
                default:
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.getResourceFailedInvalidTypeFormat, shortValue, type));
            }
        }*/

        /*/// <summary>
        /// given the index, gets a boolean response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Boolean GetBooleanInput(int index)
        {
            return InputsHelper<Boolean>.GetInput(this, index, VariableTypes.BooleanType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Byte GetByteInput(int index)
        {
            return InputsHelper<Byte>.GetInput(this, index, VariableTypes.ByteType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public SByte GetSByteInput(int index)
        {
            return InputsHelper<SByte>.GetInput(this, index, VariableTypes.SByteType);
        }

        /// <summary>
        /// given the index, gets a character response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Char GetCharInput(int index)
        {
            return InputsHelper<Char>.GetInput(this, index, VariableTypes.CharType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt16 GetUShortInput(int index)
        {
            return InputsHelper<UInt16>.GetInput(this, index, VariableTypes.UShortType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Int16 GetShortInput(int index)
        {
            return InputsHelper<Int16>.GetInput(this, index, VariableTypes.ShortType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt32 GetUIntegerInput(int index)
        {
            return InputsHelper<UInt32>.GetInput(this, index, VariableTypes.UIntegerType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Int32 GetIntegerInput(int index)
        {
            return InputsHelper<Int32>.GetInput(this, index, VariableTypes.IntegerType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt64 GetULongInput(int index)
        {
            return InputsHelper<UInt64>.GetInput(this, index, VariableTypes.ULongType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Int64 GetLongInput(int index)
        {
            return InputsHelper<Int64>.GetInput(this, index, VariableTypes.LongType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Single GetFloatInput(int index)
        {
            return InputsHelper<Single>.GetInput(this, index, VariableTypes.FloatType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Double GetDoubleInput(int index)
        {
            return InputsHelper<Double>.GetInput(this, index, VariableTypes.DoubleType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Decimal GetDecimalInput(int index)
        {
            return InputsHelper<Decimal>.GetInput(this, index, VariableTypes.DecimalType);
        }

        /// <summary>
        /// given the index, gets a DateTime response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime GetDateTimeInput(int index)
        {
            return InputsHelper<DateTime>.GetInput(this, index, VariableTypes.DateTimeType);
        }

        /// <summary>
        /// given the index, gets a TimeSpan response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TimeSpan GetTimeSpanInput(int index)
        {
            return InputsHelper<TimeSpan>.GetInput(this, index, VariableTypes.TimeSpanType);
        }

        /// <summary>
        /// given the index, gets a Guid response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Guid GetGuidInput(int index)
        {
            return InputsHelper<Guid>.GetInput(this, index, VariableTypes.GuidType);
        }

        /// <summary>
        /// given the index, gets a boolean response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Boolean? GetNullableBooleanInput(int index)
        {
            return InputsHelper<Boolean?>.GetInput(this, index, VariableTypes.NullableBooleanType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Byte? GetNullableByteInput(int index)
        {
            return InputsHelper<Byte?>.GetInput(this, index, VariableTypes.ByteType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public SByte? GetNullableSByteInput(int index)
        {
            return InputsHelper<SByte?>.GetInput(this, index, VariableTypes.NullableSByteType);
        }

        /// <summary>
        /// given the index, gets a character response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Char? GetNullableCharInput(int index)
        {
            return InputsHelper<Char?>.GetInput(this, index, VariableTypes.NullableCharType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt16? GetNullableUShortInput(int index)
        {
            return InputsHelper<UInt16?>.GetInput(this, index, VariableTypes.NullableUShortType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Int16? GetNullableShortInput(int index)
        {
            return InputsHelper<Int16?>.GetInput(this, index, VariableTypes.NullableShortType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt32? GetNullableUIntegerInput(int index)
        {
            return InputsHelper<UInt32?>.GetInput(this, index, VariableTypes.NullableUIntegerType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Int32? GetNullableIntegerInput(int index)
        {
            return InputsHelper<Int32?>.GetInput(this, index, VariableTypes.NullableIntegerType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public UInt64? GetNullableULongInput(int index)
        {
            return InputsHelper<UInt64?>.GetInput(this, index, VariableTypes.NullableULongType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Int64? GetNullableLongInput(int index)
        {
            return InputsHelper<Int64?>.GetInput(this, index, VariableTypes.NullableLongType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Single? GetNullableFloatInput(int index)
        {
            return InputsHelper<Single?>.GetInput(this, index, VariableTypes.NullableFloatType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Double? GetNullableDoubleInput(int index)
        {
            return InputsHelper<Double?>.GetInput(this, index, VariableTypes.NullableDoubleType);
        }

        /// <summary>
        /// given the index, gets a number response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Decimal? GetNullableDecimalInput(int index)
        {
            return InputsHelper<Decimal?>.GetInput(this, index, VariableTypes.NullableDecimalType);
        }

        /// <summary>
        /// given the index, gets a DateTime response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DateTime? GetNullableDateTimeInput(int index)
        {
            return InputsHelper<DateTime?>.GetInput(this, index, VariableTypes.NullableDateTimeType);
        }

        /// <summary>
        /// given the index, gets a TimeSpan response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TimeSpan? GetNullableTimeSpanInput(int index)
        {
            return InputsHelper<TimeSpan?>.GetInput(this, index, VariableTypes.NullableTimeSpanType);
        }

        /// <summary>
        /// given the index, gets a Guid response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Guid? GetNullableGuidInput(int index)
        {
            return InputsHelper<Guid?>.GetInput(this, index, VariableTypes.NullableGuidType);
        }

        /// <summary>
        /// given the index, gets a string response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public String GetStringInput(int index)
        {
            return InputsHelper<string>.GetInput(this, index, VariableTypes.StringType);
        }

        /// <summary>
        /// given the index, gets a boolean list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Boolean> GetBooleanListInput(int index)
        {
            return InputsHelper<IList<Boolean>>.GetInput(this, index, VariableTypes.ListOfBooleanType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Byte> GetByteListInput(int index)
        {
            return InputsHelper<IList<Byte>>.GetInput(this, index, VariableTypes.ListOfByteType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<SByte> GetSByteListInput(int index)
        {
            return InputsHelper<IList<SByte>>.GetInput(this, index, VariableTypes.ListOfSByteType);
        }

        /// <summary>
        /// given the index, gets a character list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Char> GetCharListInput(int index)
        {
            return InputsHelper<IList<Char>>.GetInput(this, index, VariableTypes.ListOfCharType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<UInt16> GetUShortListInput(int index)
        {
            return InputsHelper<IList<UInt16>>.GetInput(this, index, VariableTypes.ListOfUShortType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Int16> GetShortListInput(int index)
        {
            return InputsHelper<IList<Int16>>.GetInput(this, index, VariableTypes.ListOfShortType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<UInt32> GetUIntegerListInput(int index)
        {
            return InputsHelper<IList<UInt32>>.GetInput(this, index, VariableTypes.ListOfUIntegerType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Int32> GetIntegerListInput(int index)
        {
            return InputsHelper<IList<Int32>>.GetInput(this, index, VariableTypes.ListOfIntegerType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<UInt64> GetULongListInput(int index)
        {
            return InputsHelper<IList<UInt64>>.GetInput(this, index, VariableTypes.ListOfULongType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Int64> GetLongListInput(int index)
        {
            return InputsHelper<IList<Int64>>.GetInput(this, index, VariableTypes.ListOfLongType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Single> GetFloatListInput(int index)
        {
            return InputsHelper<IList<Single>>.GetInput(this, index, VariableTypes.ListOfFloatType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Double> GetDoubleListInput(int index)
        {
            return InputsHelper<IList<Double>>.GetInput(this, index, VariableTypes.ListOfDoubleType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Decimal> GetDecimalListInput(int index)
        {
            return InputsHelper<IList<Decimal>>.GetInput(this, index, VariableTypes.ListOfDecimalType);
        }

        /// <summary>
        /// given the index, gets a DateTime list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<DateTime> GetDateTimeListInput(int index)
        {
            return InputsHelper<IList<DateTime>>.GetInput(this, index, VariableTypes.ListOfDateTimeType);
        }

        /// <summary>
        /// given the index, gets a TimeSpan list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<TimeSpan> GetTimeSpanListInput(int index)
        {
            return InputsHelper<IList<TimeSpan>>.GetInput(this, index, VariableTypes.ListOfTimeSpanType);
        }

        /// <summary>
        /// given the index, gets a Guid list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Guid> GetGuidListInput(int index)
        {
            return InputsHelper<IList<Guid>>.GetInput(this, index, VariableTypes.ListOfGuidType);
        }

        /// <summary>
        /// given the index, gets a boolean list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Boolean?> GetNullableBooleanListInput(int index)
        {
            return InputsHelper<IList<Boolean?>>.GetInput(this, index, VariableTypes.ListOfNullableBooleanType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Byte?> GetNullableByteListInput(int index)
        {
            return InputsHelper<IList<Byte?>>.GetInput(this, index, VariableTypes.ListOfNullableByteType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<SByte?> GetNullableSByteListInput(int index)
        {
            return InputsHelper<IList<SByte?>>.GetInput(this, index, VariableTypes.ListOfNullableSByteType);
        }

        /// <summary>
        /// given the index, gets a character list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Char?> GetNullableCharListInput(int index)
        {
            return InputsHelper<IList<Char?>>.GetInput(this, index, VariableTypes.ListOfNullableCharType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<UInt16?> GetNullableUShortListInput(int index)
        {
            return InputsHelper<IList<UInt16?>>.GetInput(this, index, VariableTypes.ListOfNullableUShortType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Int16?> GetNullableShortListInput(int index)
        {
            return InputsHelper<IList<Int16?>>.GetInput(this, index, VariableTypes.ListOfNullableShortType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<UInt32?> GetNullableUIntegerListInput(int index)
        {
            return InputsHelper<IList<UInt32?>>.GetInput(this, index, VariableTypes.ListOfNullableUIntegerType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Int32?> GetNullableIntegerListInput(int index)
        {
            return InputsHelper<IList<Int32?>>.GetInput(this, index, VariableTypes.ListOfNullableIntegerType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IList<UInt64?> GetNullableULongListInput(int index)
        {
            return InputsHelper<IList<UInt64?>>.GetInput(this, index, VariableTypes.ListOfNullableULongType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Int64?> GetNullableLongListInput(int index)
        {
            return InputsHelper<IList<Int64?>>.GetInput(this, index, VariableTypes.ListOfNullableLongType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Single?> GetNullableFloatListInput(int index)
        {
            return InputsHelper<IList<Single?>>.GetInput(this, index, VariableTypes.ListOfNullableFloatType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Double?> GetNullableDoubleListInput(int index)
        {
            return InputsHelper<IList<Double?>>.GetInput(this, index, VariableTypes.ListOfNullableDoubleType);
        }

        /// <summary>
        /// given the index, gets a number list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Decimal?> GetNullableDecimalListInput(int index)
        {
            return InputsHelper<IList<Decimal?>>.GetInput(this, index, VariableTypes.ListOfNullableDecimalType);
        }

        /// <summary>
        /// given the index, gets a DateTime list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<DateTime?> GetNullableDateTimeListInput(int index)
        {
            return InputsHelper<IList<DateTime?>>.GetInput(this, index, VariableTypes.ListOfNullableDateTimeType);
        }

        /// <summary>
        /// given the index, gets a TimeSpan list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<TimeSpan?> GetNullableTimeSpanListInput(int index)
        {
            return InputsHelper<IList<TimeSpan?>>.GetInput(this, index, VariableTypes.ListOfNullableTimeSpanType);
        }

        /// <summary>
        /// given the index, gets a Guid list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<Guid?> GetNullableGuidListInput(int index)
        {
            return InputsHelper<IList<Guid?>>.GetInput(this, index, VariableTypes.ListOfNullableGuidType);
        }

        /// <summary>
        /// given the index, gets a string list response entered by the runtime user.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IList<String> GetStringListInput(int index)
        {
            return InputsHelper<IList<string>>.GetInput(this, index, VariableTypes.ListOfStringType);
        }*/

        /*/// <summary>
        /// Initializes the engine to run against the current module's ruleset.
        /// </summary>
        public void Start()
        {
            UpdateProgressList();

            RuleEngine ruleEngine = RulesCache.GetRuleEngine(moduleBeginName);
            if (ruleEngine == null)
                throw new InvalidOperationException(Strings.ruleSetCannotBeNull);

            SetCurrentBusinessBackupData();

            ruleEngine.Execute(this);
        }*/

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
            this._dialogClosed = flowBackupData.DialogClosed;
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
        /// Sets DialogClosed to true and sets the answers from the question form
        /// </summary>
        /// <param name="answers"></param>
        public void AnswerQuestions(Dictionary<int, int> answers)
        {
            DialogClosed = true;
            QuestionListAnswers.Clear();
            foreach (int key in answers.Keys)
                QuestionListAnswers.Add(key, answers[key]);
        }

        /// <summary>
        /// Sets DialogClosed to true and sets the answers from the question form
        /// </summary>
        /// <param name="answers"></param>
        public void AnswerQuestions(Dictionary<int, int> answers, string selection)
        {
            Selection = selection;
            QuestionListAnswers.Clear();
            foreach (int key in answers.Keys)
                QuestionListAnswers.Add(key, answers[key]);
        }

        /// <summary>
        /// Sets DialogClosed to true and sets the answers from the input question form
        /// </summary>
        /// <param name="answers"></param>
        public void AnswerInputQuestions(Dictionary<int, InputResponse> answers)
        {
            DialogClosed = true;
            InputQuestionsAnswers.Clear();
            foreach (int key in answers.Keys)
            {
                if (answers[key].Answer != null)
                    InputQuestionsAnswers.Add(key, answers[key].Answer);
                else
                {
                    if (answers[key].Type.Equals(typeof(string)) || (answers[key].Type.IsGenericType && answers[key].Type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
                        InputQuestionsAnswers.Add(key, answers[key].Answer);
                }
            }
        }

        /// <summary>
        /// Sets DialogClosed to true and sets the answers from the input question form
        /// </summary>
        /// <param name="answers"></param>
        public void AnswerInputQuestions(Dictionary<int, InputResponse> answers, string selection)
        {
            Selection = selection;
            InputQuestionsAnswers.Clear();
            foreach (int key in answers.Keys)
            {
                if (answers[key].Answer != null)
                    InputQuestionsAnswers.Add(key, answers[key].Answer);
                else
                {
                    if (answers[key].Type.Equals(typeof(string)) || (answers[key].Type.IsGenericType && answers[key].Type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
                        InputQuestionsAnswers.Add(key, answers[key].Answer);
                }
            }
        }

        /// <summary>
        /// Sets DialogClosed to true, updates the variables from the Update Variables Form, and executes the rules engine
        /// </summary>
        /// <param name="variables"></param>
        public void UpdateVariables(Dictionary<string, VariableInfo> variables)
        {
            DialogClosed = true;
            SetVariableValues(variables);
        }

        /// <summary>
        /// Sets DialogClosed to true and executes the rules engine
        /// </summary>
        public void CloseDialog()
        {
            DialogClosed = true;
        }

        /// <summary>
        /// This property is used by the auto-generated rules and should not be used otherwise. It is called when the rule generated by a Flow Begin shape executes.
        /// </summary>
        /// <param name="moduleName"></param>
        public void SetModuleName(string moduleName)
        {
            this._moduleBeginName = moduleName;
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
