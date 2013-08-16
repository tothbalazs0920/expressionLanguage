namespace Expressions
{
    public class PrimitiveType : Type
    {
        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }

        public PrimitiveType(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}