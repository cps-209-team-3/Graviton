/*
File: Logger.cs
Desc: Class to log server side messages
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GravitonServer
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
