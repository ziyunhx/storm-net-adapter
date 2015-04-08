namespace Storm
{
    public class Command
    {
        /// <summary>
        /// command nanme
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// id
        /// </summary>
        public string Id { set; get; }

        public Command(string command, string id = "")
        {
            this.Name = command;
            this.Id = id;
        }
    }
}