using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.RulesDirector
{
    [Serializable]
    public class FlowBackupData : IEquatable<FlowBackupData>
    {
        internal FlowBackupData(string driver, string selection, Stack callingModuleDriverStack, Stack callingModuleStack, string moduleBeginName, string moduleEndName)
        {
            this.driver = driver;
            this.selection = selection;
            this.callingModuleDriverStack = callingModuleDriverStack;
            this.callingModuleStack = callingModuleStack;
            this.moduleBeginName = moduleBeginName;
            this.moduleEndName = moduleEndName;
        }

        #region Variables
        private string driver;
        private string selection;
        private Stack callingModuleDriverStack;
        private Stack callingModuleStack;
        private string moduleBeginName;
        private string moduleEndName;
        #endregion Variables

        #region Properties
        internal string Driver
        {
            get { return driver; }
        }

        internal string Selection
        {
            get { return selection; }
        }

        internal Stack CallingModuleDriverStack
        {
            get { return callingModuleDriverStack; }
        }

        internal Stack CallingModuleStack
        {
            get { return callingModuleStack; }
        }

        internal string ModuleBeginName
        {
            get { return moduleBeginName; }
        }

        internal string ModuleEndName
        {
            get { return moduleEndName; }
        }
        #endregion Properties

        #region Methods
        public override int GetHashCode()
        {
            return this.driver.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            return this.Equals((FlowBackupData)obj);
        }
        #endregion Methods

        #region IEquatable<FlowBackupData> Members
        public bool Equals(FlowBackupData other)
        {
            if (other == null)
                return false;

            return this.driver == other.driver
                && this.selection == other.selection
                && this.moduleBeginName == other.moduleBeginName
                && this.moduleEndName == other.moduleEndName;
        }
        #endregion
    }
}
