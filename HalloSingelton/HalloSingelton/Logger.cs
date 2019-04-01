using System;

namespace HalloSingelton
{
    class Logger
    {
        private static Logger logger;
        private static object _syncObj = new object();
        public static Logger Instance
        {
            get
            {
                lock (_syncObj)
                {
                    if (logger == null)
                        logger = new Logger();
                }
                return logger;
            }
        }
        public void Log(string msg)
        {
            Console.WriteLine($"({counter})[{DateTime.Now:T}] {msg}");
        }

        static int counter = 0;
        private Logger()
        {
            counter++;
        }
    }
}
