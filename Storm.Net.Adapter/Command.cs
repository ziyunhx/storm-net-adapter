namespace Storm
{
    public class Command
    {
        /// <summary>
        /// command nanme
        /// </summary>
        private string command { set; get; }
        /// <summary>
        /// id
        /// </summary>
        private string id { set; get; }

        public Command(string command, string id = "")
        {
            this.command = command;
            this.id = id;
        }

        public string GetCommand()
        {
            return this.command;
        }

        public string GetId()
        {
            return this.id;
        }
    }
}
