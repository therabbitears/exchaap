using Plugin.ValidationRules.Interfaces;
using System;

namespace Xaminals.Validations
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value.ToString();
            return !string.IsNullOrWhiteSpace(str);
        }
    }

    public class MustBeTrueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            return Convert.ToBoolean(value) == true;
        }
    }
}
