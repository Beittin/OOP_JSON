﻿namespace OOP_JSON_Final.Models
{
    public class JSON_Number : JSON_Base
    {
        /// <summary>
        /// A <c>double</c> field
        /// </summary>
        private double _value;
        /// <summary>
        /// A <c>string</c> field
        /// </summary>
        private string _stringValue;

        /// <summary>
        /// A <c>double</c> parameter
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
        /// <summary>
        /// A <c>string</c> parameter
        /// </summary>
        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }

        /// <summary>
        /// Gets the weight of the JSON object, with the following parameters:
        /// <para/>
        /// String, Bools, and Numbers have a weight of 1
        /// <para/>
        /// Objects have a weight of 1 + the weight of all their contents
        /// <para/>
        /// Arrays have a weight of 1 + the weight of all their contents
        /// </summary>
        /// <returns>The calculated weight</returns>
        public override int GetWeight() { return 1; }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString() { return StringValue; }
    }
}
