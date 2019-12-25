/*

Developed By: HazyTube
Name: Touch The Rear
Released on: GitHub and LSPDFR

*/

using Rage;
using System.Reflection;

namespace TouchTheRear
{
internal static class Notifier
    {
        private const string NotificationPrefix = "Touch The Rear";

        internal static void DisplayNotification(string Subtitle, string Body)
        {
            Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", NotificationPrefix, $"~b~{Subtitle}~s~",
                Body);
            Logger.DebugLog("Notification Displayed");
        }

        internal static void StartUpNotification()
        {
            Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", NotificationPrefix, "~y~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~o~by HazyTube", "~b~Has been loaded.");
            Logger.DebugLog("Startup Notification Sent.");
        }

        internal static void StartUpNotificationOutdated()
        {
            Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", NotificationPrefix, "~y~v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ~o~by HazyTube", $"~r~Plugin is out of date! Please update the plugin.~s~ \nLatest Version: {Globals.Application.LatestVersion}");
            Logger.DebugLog("Startup Notification (Outdated) Sent.");
        }
    }
}