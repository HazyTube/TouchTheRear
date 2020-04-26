/*

Developed By: HazyTube
Name: First Callouts
Released on: GitHub and LSPDFR

This version checker class is made by Albo1125, all credits for this class go to him!
Note: This class is not exactly how Albo made it, I did change some stuff to better integrate it with my plugin.

*/

using Rage;
using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace TouchTheRear
{
class RPHVersionChecker
    {
        private static bool correctVersion;

        /// <summary>
        /// Checks whether the person has the specified minimum version or higher. 
        /// </summary>
        /// <param name="minimumVersion">Provide in the format of a float i.e.: 0.22</param>
        /// <returns></returns>
        public static bool checkForRageVersion(float minimumVersion)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo("RAGEPluginHook.exe");
            float Rageversion;
            try
            {
                Rageversion = float.Parse(versionInfo.ProductVersion.Substring(0, 4), CultureInfo.InvariantCulture);
                Logger.Log("Detected RAGEPluginHook version: " + Rageversion.ToString());

                //If user's RPH version is older than the minimum
                if (Rageversion < minimumVersion)
                {
                    correctVersion = false;
                    GameFiber.StartNew(delegate
                    {
                        while (Game.IsLoading)
                        {
                            GameFiber.Yield();
                        }

                        Notifier.DisplayNotification("RPH Version", "RPH ~r~v" + Rageversion.ToString() +
                                                                "~s~ detected. ~b~Touch The Rear~s~ requires ~b~v" + minimumVersion.ToString() +
                                                                "~s~ or higher.");
                        GameFiber.Sleep(5000);
                        Logger.Log("RAGEPluginHook version " + Rageversion.ToString() +
                                   " detected. Touch The Rear requires v" + minimumVersion.ToString() + " or higher.");
                        Logger.Log("Preparing redirect...");
                        Notifier.DisplayNotification("RPH Version", "You are being redirected to the RagePluginHook website so you can download the latest version.");
                        Notifier.DisplayNotification("RPH Version", "Press Backspace to cancel the redirect.");

                        int count = 0;
                        while (true)
                        {
                            GameFiber.Sleep(10);
                            count++;
                            if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Back))
                            {
                                break;
                            }

                            if (count >= 300)
                            {
                                //URL to the RPH download page.
                                System.Diagnostics.Process.Start("http://bit.ly/RPHDownloadPage");
                                break;
                            }
                        }
                    });
                }
                //If user's RPH version is (above) the specified minimum
                else
                {
                    correctVersion = true;
                }
            }
            catch (Exception e)
            {
                //If for whatever reason the version couldn't be found.
                Logger.Log(e.ToString());
                Logger.Log("Unable to detect your Rage installation.");
                if (File.Exists("RAGEPluginHook.exe"))
                {
                    Logger.Log("RAGEPluginHook.exe exists");
                }
                else
                {
                    Logger.Log("RAGEPluginHook doesn't exist.");
                }

                Logger.Log("Rage Version: " + versionInfo.ProductVersion.ToString());
                Notifier.DisplayNotification("ERROR", "Unable to detect RPH installation. Please send your logfile in the support channel on HazyTube's discord server.");
                correctVersion = false;
            }

            return correctVersion;
        }
    }
}