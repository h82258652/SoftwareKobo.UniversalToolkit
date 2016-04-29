using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public class ModelVerifyErrors : ObservableCollection<string>
    {
        private IList<ValidationResult> _results = new List<ValidationResult>();

        internal ModelVerifyErrors(object verifiableObject)
        {
            var context = new ValidationContext(verifiableObject);
            Validator.TryValidateObject(verifiableObject, context, _results, true);
            foreach (var result in _results)
            {
                Add(result.ErrorMessage);
            }
        }

        public string this[string propertyName]
        {
            get
            {
                foreach (var result in _results)
                {
                    if (result.MemberNames.Contains(propertyName))
                    {
                        return result.ErrorMessage;
                    }
                }
                return string.Empty;
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Items);
        }
    }
}