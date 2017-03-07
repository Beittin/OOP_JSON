namespace OOP_JSON_Final.Models
{
    /// <summary>
    /// The abstract class from which all other JSON objects inherit
    /// </summary>
    public abstract class JSON_Base
    {
        /// <summary>
        /// A function to calculate the weight of the inheriting object
        /// </summary>
        public abstract int GetWeight();
    }
}
