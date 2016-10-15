using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Framework
{
    public abstract class ViewModelBase : ObservableObject, IDataErrorInfo
    {
        #region Fields

        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        private ViewModelBase selectedViewModel;   

        #endregion

        #region Properties

        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        public string Error
        {
            get { return _errors.FirstOrDefault(x => !string.IsNullOrEmpty(x.Value)).Value; }
        }

        public bool IsValidated
        {
            get { return string.IsNullOrEmpty(Error); }
        }

        public bool IsActive { get; private set; }

        public ViewModelBase SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                if (selectedViewModel == value)
                    return;

                selectedViewModel = value;

                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Methods 

        public virtual void Initialize()
        {
            IsActive = true;
        }

        public virtual void UnInitialize()
        {
            IsActive = false;
        }

        protected virtual string OnValidate(string propertyName)
        {

            if (!_errors.ContainsKey(propertyName))
                _errors.Add(propertyName, string.Empty);
            

            var propertyInfo = this.GetType().GetProperty(propertyName);

            var results = new List<ValidationResult>();

            if (!Validator.TryValidateProperty(
                                      propertyInfo.GetValue(this, null),
                                      new ValidationContext(this, null, null)
                                      {
                                          MemberName = propertyName
                                      },
                                      results))
            {
                var validationResult = results.First();

                _errors[propertyName] = validationResult.ErrorMessage;

                return validationResult.ErrorMessage;
            }

            _errors[propertyName] = string.Empty;

            return string.Empty;
        }


        #endregion
    }
}
