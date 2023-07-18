using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Foreman.UI.Components
{
    class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        Dictionary<string, string> _dictionary;
        object _key;

        internal DictionaryPropertyDescriptor(Dictionary<string, string> d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType
        {
            get { return _dictionary[_key.ToString()].GetType(); }
        }

        public override void SetValue(object component, object value)
        {
            _dictionary[_key.ToString()] = value.ToString();
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key.ToString()];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
