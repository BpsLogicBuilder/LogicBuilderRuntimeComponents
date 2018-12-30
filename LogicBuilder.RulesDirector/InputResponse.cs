using System;

namespace LogicBuilder.RulesDirector
{
    [Serializable]
    public class InputResponse
    {
        public InputResponse(object answer, bool unanswered, Type type)
        {
            this.answer = answer;
            this.unanswered = unanswered;
            this.type = type;
        }

        #region Variables
        private object answer;
        private bool unanswered;
        private Type type;
        #endregion Variables

        #region Properties
        public object Answer
        {
            get { return answer; }
        }

        public bool Unanswered
        {
            get { return unanswered; }
        }

        public Type Type
        {
            get { return type; }
        }
        #endregion Properties
    }
}
