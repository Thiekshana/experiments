using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    abstract class Values
    {
        private readonly string _title;
        private string _currentValue;
        private string _revisedValue;

        protected Values(string title, string currentValue, string revisedValue)
        {
            _title = title;
            _currentValue = currentValue;
            _revisedValue = revisedValue;
        }

        public void SetValue()
        {

            if(_currentValue == null && _revisedValue == null)
            {
                throw new ArgumentNullException("Can't be Empty");
            }

            else if(_currentValue == null && _revisedValue != null)
            {
                _revisedValue = "<Added>";
                _currentValue = "<Not Applicable>";
            }

            else if (_currentValue != null && _revisedValue == null)
            {
                _revisedValue = "<Removed>";
            }

            else
            {
                if (_currentValue == _revisedValue)
                    _revisedValue = "No Change";
            }

            //Console.WriteLine($"current: {_currentValue},Revised: {_revisedValue}");

        }

        public void SetValue(Type t, bool Isidentifier)
        {
            //define setValues logic here for complex cards
            //TODO: refactor this once identify all the common patterns for every card

            if(t == typeof(Property))
            {
                if(Isidentifier)
                {
                    _currentValue = _revisedValue;
                }
                else
                {
                    if (_currentValue == null && _revisedValue == null)
                    {
                        throw new ArgumentNullException("Can't be Empty");
                    }

                    else if (_currentValue == null && _revisedValue != null)
                    {
                        _currentValue = "<Not Applicable>";
                    }

                    else if (_currentValue != null && _revisedValue == null)
                    {
                        _revisedValue = "<Removed>";
                    }

                    else
                    {
                        if (_currentValue == _revisedValue)
                            _revisedValue = "No Change";
                    }
                }
            }
        }
    }
}
