using System.Reflection;

namespace jwtmanualauthentication.Common
{
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        protected Enumeration(int Value, string Name)
        {
            this.Value = Value;
            this.Name = Name;
        }

        //we are creating dictionary to store values for the strongly typed enums
        public static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

        public int Value { get; protected init; } // this Value can be only initialized in same class or derived class that is why it is being protected init
        public string Name { get; protected init; }


        public bool Equals(Enumeration<TEnum> other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() && Value == other.Value;
        }

        public override bool Equals(object? obj) // this overide equals method will gets exeuted first and internally calls real equal method
        {
            return obj is Enumeration<TEnum> other && this.Equals(other);
        }

        // we are getting enumeration(credit card) by the help of value and name

        public static TEnum FromValue(int Value)
        {
            return Enumerations.TryGetValue(Value, out TEnum? enumeration) ? enumeration : default;
        }

        public static TEnum FromName(string Name)
        {
            return Enumerations.Values.SingleOrDefault(value => value.Name == Name);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public Dictionary<int, TEnum> GetValues() {
            return Enumerations;
        }

        private static Dictionary<int, TEnum> CreateEnumerations()
        {

            var enumerationType = typeof(TEnum); // Gets the type information of the class.So now C# can inspect the class metadata.

            var fieldsForType = enumerationType
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy) // This gets all fields that are: Public, Static and Includes inherited static fields too
                .Where(fieldInfo =>
                    enumerationType.IsAssignableFrom(fieldInfo.FieldType)) // This filters only fields whose type matches TEnum. ie if TEnum is credit card then this field is not taken - public static string Test = "Hello"; because this is only string
                .Select(fieldInfo =>
                    (TEnum)fieldInfo.GetValue(default)!); // it extracts the actual object: like new CreditCard(2, "Premium")

            // now we return dictionary with key as enumeration values
            return fieldsForType.ToDictionary(x => x.Value);
        }

    }
}
