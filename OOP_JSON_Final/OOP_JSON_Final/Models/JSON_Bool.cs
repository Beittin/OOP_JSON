namespace OOP_JSON_Final.Models
{
    public class JSON_Bool : JSON_Base
    {
        /// <summary>
        /// A <c>bool?</c> field
        /// </summary>
        private bool? _value;

        /// <summary>
        /// A <c>bool?</c> parameter
        /// </summary>
        public bool? Value
        {
            get { return _value; }
            set { _value = value; }
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
        public override string ToString()
        {
            if (Value.HasValue)
            {
                if (Value.Value)
                    return "true";
                return "false";
            }
            return "null";
        }
    }
}
