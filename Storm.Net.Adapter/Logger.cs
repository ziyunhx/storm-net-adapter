namespace Storm
{
    public class Logger
    {
        private string classMsg = "";

        public Logger(string className = "")
        {
            if (!string.IsNullOrEmpty(className))
                classMsg = className + ": ";
        }

        public void Trace(string Message, params object[] args)
        {
            SendLog(FromatMessage(Message, args), 0);
        }
        public void Debug(string Message, params object[] args)
        {
            SendLog(FromatMessage(Message, args), 1);
        }
        public void Info(string Message, params object[] args)
        {
            SendLog(FromatMessage(Message, args), 2);
        }
        public void Warn(string Message, params object[] args)
        {
            SendLog(FromatMessage(Message, args), 3);
        }
        public void Error(string Message, params object[] args)
        {
            SendLog(FromatMessage(Message, args), 4);
        }
        private void SendLog(string Message, int level = 2)
        {
            ApacheStorm.SendMsgToParent("{\"command\": \"log\", \"msg\": \"" + classMsg + Message + "\", \"level\":" + level + "}");
        }

        private string FromatMessage(string Message, params object[] args)
        {
            string message = Message;
            try
            {
                if (args != null && args.Length > 0)
                    message = string.Format(Message, args);
            }
            catch { }

            return message;
        }
    }
}