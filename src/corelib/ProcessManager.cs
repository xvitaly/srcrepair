/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Security.Principal;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with processes.
    /// </summary>
    public static class ProcessManager
    {
        /// <summary>
        /// Terminates process with specified name and return it's PID.
        /// If multiple processes with the same image name detected,
        /// will return PID of the last terminated process.
        /// If no processes matches criteria, will return 0.
        /// </summary>
        /// <param name="ProcessName">Process name.</param>
        /// <returns>PID of terminated process or 0.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static int ProcessTerminate(string ProcessName)
        {
            // Setting default PID to 0...
            int ProcID = 0;

            // Filtering processes by name and creating array...
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);

            // Using loop to terminate all processes from this array...
            foreach (Process ResName in LocalByName)
            {
                // Saving PID...
                ProcID = ResName.Id;
                
                // Terminating process...
                ResName.Kill();
            }

            // Returning PID of terminated process or default value...
            return ProcID;
        }

        /// <summary>
        /// Checks if process with specified name is running.
        /// </summary>
        /// <param name="ProcessName">Process name.</param>
        /// <returns>Returns True if process exists.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static bool IsProcessRunning(string ProcessName)
        {
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);
            return LocalByName.Length > 0;
        }

        /// <summary>
        /// Checks if current user has local adminstrators access rights
        /// (permissions).
        /// </summary>
        /// <returns>Returns True if current user has admin rights.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static bool IsCurrentUserAdmin()
        {
            bool Result;
            try
            {
                // Checking access rights of currently logged-in user...
                WindowsPrincipal UP = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                // Checking if current user is in Administrators group...
                Result = UP.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                // Exception detected. User has no admin rights.
                Result = false;
            }
            return Result;
        }

        /// <summary>
        /// Runs external executable in hidden state with optional command-line
        /// options and waits for its completion.
        /// </summary>
        /// <param name="SAppName">Full path to external executable.</param>
        /// <param name="SParameters">Command-line options.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void StartProcessAndWait(string SAppName, string SParameters)
        {
            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = SAppName,
                Arguments = SParameters,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Waiting for completion...
            NewProcess.WaitForExit();
        }

        /// <summary>
        /// Runs external executable in hidden state with optional command-line
        /// options, waits for its completion and returns its stdout output.
        /// </summary>
        /// <param name="SAppName">Full path to external executable.</param>
        /// <param name="SParameters">Command-line options.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static string StartProcessWithStdOut(string SAppName, string SParameters)
        {
            // Creating local variable for saving result...
            string Result;

            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = SAppName,
                Arguments = SParameters,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Reading stdout output of running process...
            Result = NewProcess.StandardOutput.ReadToEnd();

            // Waiting for completion...
            NewProcess.WaitForExit();

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Adds quotes to path.
        /// </summary>
        /// <param name="Source">Source string with path.</param>
        /// <returns>Quoted string with path.</returns>
        public static string AddQuotesToPath(string Source)
        {
            return String.Format(Properties.Resources.AppOpenHandlerEscapeTemplate, Source);
        }
    }
}
