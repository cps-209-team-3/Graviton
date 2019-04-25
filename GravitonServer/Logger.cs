/*
File: Logger.cs
Desc: Class to log server side messages
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GravitonServer
{
    //Contains one method that logs messages
    public static class Logger
    {
        //prints a message to the console
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
