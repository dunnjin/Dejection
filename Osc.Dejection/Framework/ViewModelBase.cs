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

        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private ViewModelBase selectedViewModel;   

        #endregion

        #region Properties

        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        public string Error
        {
            get { return errors.FirstOrDefault(x => !string.IsNullOrEmpty(x.Value)).Value; }
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

            if (!errors.ContainsKey(propertyName))
                errors.Add(propertyName, string.Empty);
            

            PropertyInfo propertyInfo = this.GetType().GetProperty(propertyName);

            List<ValidationResult> results = new List<ValidationResult>();

            if (!Validator.TryValidateProperty(
                                      propertyInfo.GetValue(this, null),
                                      new ValidationContext(this, null, null)
                                      {
                                          MemberName = propertyName
                                      },
                                      results))
            {
                ValidationResult validationResult = results.First();

                errors[propertyName] = validationResult.ErrorMessage;

                return validationResult.ErrorMessage;
            }

            errors[propertyName] = string.Empty;

            return string.Empty;
        }


        #endregion
    }
}
