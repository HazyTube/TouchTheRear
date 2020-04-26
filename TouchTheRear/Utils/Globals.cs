/*

Developed By: HazyTube
Name: Touch The Rear
Released on: GitHub and LSPDFR

*/

using System;
using System.Reflection;
using System.Windows.Forms;
using LSPD_First_Response.Mod.API;
using Rage;

namespace TouchTheRear
{
    class Globals
    {
        internal static class Application
        {
            public static string ConfigPath { get; set; }
            public static string ConfigFileName { get; set; }
            public static string CurrentVersion { get; set; }
            public static string LatestVersion { get; set; }
            public static string PluginName { get; set; }
        }
        
        internal static class GeneralConfig
        {
            public static bool DebugLogging { get; set; }
        }

        internal static class Keybindings
        {
            public static Keys TouchRearKey { get; set; }
            public static Keys TouchRearModifier { get; set; }
        }

        internal static class Status
        {
            public static bool Firsteventdistancetoobject { get; set; }
            public static bool Secondeventdistancetoobject { get; set; }
        }
        
        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly allUserPlugin in Functions.GetAllUserPlugins())
            {
                AssemblyName name = allUserPlugin.GetName();
                if (name.Name.ToLower() == Plugin.ToLower() && (minversion == (Version) null || name.Version.CompareTo(minversion) >= 0))
                    return true;
            }
            return false;
        }
        
        internal static bool IsKeyDown(Keys ModifierKey, Keys Key)
        {
            return (Game.IsKeyDownRightNow(ModifierKey) || ModifierKey == Keys.None) && Game.IsKeyDown(Key);
        }
    }
}