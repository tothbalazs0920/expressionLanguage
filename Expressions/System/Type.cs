namespace Expressions
{
    /// <summary>
    /// Types
    /// </summary>
    public abstract class Type
    {
        private static readonly Type Integer = new PrimitiveType("int");
        private static readonly Type Boolean = new PrimitiveType("bool");

        public static Type IntegerType
        {
            get { return Integer; }
        }

        public static Type BooleanType
        {
            get { return Boolean; }
        }
    }
}