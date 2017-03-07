using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP_JSON_Final.Models
{
    class JSON_Object : JSON_Base
    {
        /// <summary>
        /// A <c>Dictionary&lt;string, JSON_Base&gt;</c> field
        /// </summary>
        private Dictionary<string, JSON_Base> _value;

        /// <summary>
        /// A <c>Dictionary&lt;string, JSON_Base&gt;</c> parameter
        /// </summary>
        public Dictionary<string, JSON_Base> Value
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
        public override int GetWeight()
        {
            int i = 1;
            foreach (KeyValuePair<string, JSON_Base> val in Value ?? new Dictionary<string, Models.JSON_Base>())
                i += val.Value.GetWeight();
            return i;
        }

        /// <summary>
        /// Gets the count of <see cref="JSON_Array"/> objects within this object
        /// </summary>
        /// <returns>
        /// The number of <see cref="JSON_Array"/> objects within this object
        /// </returns>
        public int ArrayCount()
        {
            int i = 0;
            foreach (KeyValuePair<string, JSON_Base> val in Value ?? new Dictionary<string, JSON_Base>())
            {
                if (val.Value is JSON_Array)
                    i += (val.Value as JSON_Array).ArrayCount();
                else if (val.Value is JSON_Object)
                    i += (val.Value as JSON_Object).ArrayCount();
            }
            return i;
        }

        /// <summary>
        /// Gets the count of <see cref="JSON_Object"/> objects within this object
        /// </summary>
        /// <returns>
        /// The number of <see cref="JSON_Object"/> objects within this object
        /// </returns>
        public int ObjectCount()
        {
            int i = 1;
            foreach (KeyValuePair<string, JSON_Base> val in Value ?? new Dictionary<string, JSON_Base>())
            {
                if (val.Value is JSON_Object)
                    i += (val.Value as JSON_Object).ObjectCount();
                else if (val.Value is JSON_Array)
                    i += (val.Value as JSON_Array).ObjectCount();
            }
            return i;
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            if (Value == null || Value.Count == 0)
                return "{}";
            StringBuilder sb = new StringBuilder();
            string[] sArr;
            sb.AppendLine("{");
            for (int i = 0; i < Value.Count - 1; ++i)
            {
                sArr = Value.ElementAt(i).Value.ToString()
                    .Split(new string[] { Environment.NewLine },
                    StringSplitOptions.None);
                if (sArr.Length == 1)
                {
                    sb.AppendLine("\t\"" + Value.ElementAt(i).Key + "\": " + sArr[0] + ",");
                    continue;
                }
                sb.AppendLine("\t\"" + Value.ElementAt(i).Key + "\": " + sArr[0]);
                for (int j = 1; j < (sArr.Length) - 1; ++j)
                    sb.AppendLine("\t" + sArr[j]);
                sb.AppendLine("\t" + sArr.Last() + ",");
            }
            if (Value.Count > 0)
            {
                sArr = Value.Last().Value.ToString()
                    .Split(new string[] { Environment.NewLine },
                    StringSplitOptions.None);
                sb.AppendLine("\t\"" + Value.Last().Key + "\": " + sArr[0]);
                for (int j = 1; j < sArr.Length; ++j)
                    sb.AppendLine("\t" + sArr[j]);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
