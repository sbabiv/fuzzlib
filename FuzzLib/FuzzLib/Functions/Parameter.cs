namespace FuzzLib.Functions
{
    public class Parameter
    {
        /// <summary>
        /// Loop name
        /// </summary>
        public string Loop { get; private set; }

        /// <summary>
        /// Variable name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Variable index
        /// </summary>
        public int Index { get; set; }

        public Parameter(string name)
        {
            Name = name;
            var values = name.Split('.');
            if (values.Length == 2)
            {
                Loop = values[0];
                Name = values[1];
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Loop) ? Name : string.Format("{0}/*/{1}", Loop, Name);
        }

        public override bool Equals(object obj)
        {
            var parameter = obj as Parameter;
            if (parameter == null) return false;

            return parameter.Name == Name && parameter.Index == Index;
        }
    }
}
