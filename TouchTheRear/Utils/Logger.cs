/*

Developed By: HazyTube
Name: Touch The Rear
Released on: GitHub and LSPDFR

*/

using Rage;

namespace TouchTheRear
{
    internal static class Logger
    {
        private const string LogPrefix = "TouchTheRear";

        internal static void Log(string LogLine)
        {
            string log = string.Format("[{0}]: {1}", LogPrefix, LogLine);

            Game.LogTrivial(log);
        }

        internal static void DebugLog(string LogLine)
        {
            if (Globals.GeneralConfig.DebugLogging)
            {
                string log = string.Format("[{0}][DEBUG]: {1}", LogPrefix, LogLine);

                Game.LogTrivial(log);
            }
        }
    }
}