/*

Developed By: HazyTube
Name: Touch The Rear
Released on: GitHub and LSPDFR

*/

using Rage;
using System.Windows.Forms;

namespace TouchTheRear
{
internal static class Settings
    {
        private static InitializationFile initialiseFile(string filepath)
        {
            InitializationFile ini = new InitializationFile(filepath);
            ini.Create();
            return ini;
        }

        public static void LoadSettings()
        {
            InitializationFile settings = initialiseFile(Globals.Application.ConfigPath + Globals.Application.ConfigFileName);

            //Makes a new key converter to convert a string to a key
            KeysConverter kc = new KeysConverter();

            string TouchRearKey, TouchRearModifier;

            //KEYS
            //Reads the keys from the ini file
            TouchRearKey = settings.ReadString("Keybindings", "TouchRearKey", "U");
            TouchRearModifier = settings.ReadString("Keybindings", "TouchRearModifier", "None");
            
            //KEY CONVERTERS
            //Converts a string to a key
            Globals.Keybindings.TouchRearKey = (Keys) kc.ConvertFromString(TouchRearKey);
            Globals.Keybindings.TouchRearModifier = (Keys) kc.ConvertFromString(TouchRearModifier);

            //GENERAL
            //Reads the values in the General section from the ini file
            Globals.GeneralConfig.DebugLogging = settings.ReadBoolean("General", "DebugLogging", false);
            
            //LOGGERS
            //Logs the values set in the ini file
            Logger.Log("[GENERAL] DebugLogging is set to " + Globals.GeneralConfig.DebugLogging);
            Logger.Log("[KEYBINDINGS] TouchRearKey is set to " + Globals.Keybindings.TouchRearKey);
            Logger.Log("[KEYBINDINGS] TouchRearModifier is set to " + Globals.Keybindings.TouchRearModifier);
            Game.LogTrivial("-----------------------------------------------------------------------------------------------------");
        }
    }
}