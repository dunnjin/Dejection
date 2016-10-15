using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Osc.Dejection.Framework
{
    /// <summary>
    /// Implements INotifyPropertyChanged... Is there anything that needs to be explained?
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods

        /// <summary>
        /// Notify view of Compiler services property name changes
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                // Need to find a better and convient way to refresh controls canexecute methods, instead of below
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
    }
}
