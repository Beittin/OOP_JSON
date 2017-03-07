using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOP_JSON_Final.Models
{
    class JSON_Array : JSON_Base
    {
        /// <summary>
        /// A <c>List&lt;JSON_Base&gt;</c> field
        /// </summary>
        private List<JSON_Base> _value;

        /// <summary>
        /// A <c>List&lt;JSON_Base&gt;</c> parameter
        /// </summary>
        public List<JSON_Base> Value
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
            foreach (JSON_Base val in Value ?? new List<JSON_Base>())
                i += val.GetWeight();
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
            int i = 1;
            foreach (JSON_Base val in Value ?? new List<JSON_Base>())
            {
                if (val is JSON_Array)
                    i += (val as JSON_Array).ArrayCount();
                else if (val is JSON_Object)
                    i += (val as JSON_Object).ArrayCount();
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
            int i = 0;
            foreach (JSON_Base val in Value ?? new List<JSON_Base>())
            {
                if (val is JSON_Object)
                    i += (val as JSON_Object).ObjectCount();
                else if (val is JSON_Array)
                    i += (val as JSON_Array).ObjectCount();
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
                return "[]";
            StringBuilder sb = new StringBuilder();
            string[] sArr;
            sb.AppendLine("[");
            for (int i = 0; i < (Value.Count) - 1; ++i)
            {
                sArr = Value[i].ToString()
                    .Split(new string[] { Environment.NewLine },
                    StringSplitOptions.None);
                for (int j = 0; j < (sArr.Length) - 1; ++j)
                    sb.AppendLine("\t" + sArr[j]);
                sb.AppendLine("\t" + sArr.Last() + ",");
            }
            if ((Value.Count) > 0)
            {
                sArr = Value.Last().ToString()
                    .Split(new string[] { Environment.NewLine },
                    StringSplitOptions.None);
                for (int i = 0; i < sArr.Length; ++i)
                    sb.AppendLine("\t" + sArr[i]);
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
