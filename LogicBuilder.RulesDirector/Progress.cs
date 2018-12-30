using System;
using System.Collections.ObjectModel;

namespace LogicBuilder.RulesDirector
{
    [Serializable()]
    public class Progress
    {
        public Progress()
        {
        }

        #region Constants
        #endregion Constants

        #region Variables
        private Collection<ProgressInfo> progressItems = new Collection<ProgressInfo>();
        public event EventHandler ItemAdded;
        public event EventHandler ListCleared;
        #endregion Variables

        #region Properties
        public Collection<ProgressInfo> ProgressItems
        {
            get { return progressItems; }
        }
        #endregion Properties

        #region Methods
        internal void AddProgressItem(string description)
        {
            ProgressInfo progressInfo = new ProgressInfo(description);
            progressItems.Add(progressInfo);

            RaiseItemAdded(progressInfo);
        }

        public void ClearProgressList()
        {
            progressItems.Clear();
            RaiseListCleared();
        }

        private void RaiseItemAdded(ProgressInfo item)
        {
            ItemAdded?.Invoke(item, EventArgs.Empty);
        }

        private void RaiseListCleared()
        {
            ListCleared?.Invoke(this, EventArgs.Empty);
        }
        #endregion Methods
    }
}
