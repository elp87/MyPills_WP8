namespace My_Pills.Classes
{
    public class Pill
    {
        public Pill(string name, string info)
        {
            this.Name = name;
            this.Info = info;
        }
        public string Name { get; private set; }

        public string Info { get; private set; }
    }
}
