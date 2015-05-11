using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace slack_svn
{
    class Program
    {
        private static string svnpath = Environment.GetEnvironmentVariable("VISUALSVN_SERVER");
        static int Main(string[] args)
        {
            //Check if revision number and revision path have been supplied.
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Invalid arguments sent - <REPOSITORY> <REV> required");
                return 1;
            }

            //Check if VisualSVN is installed.
            if (string.IsNullOrEmpty(svnpath))
            {
                Console.Error.WriteLine("VISUALSVN_SERVER environment variable does not exist. VisualSVN installed?");
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Runs a command on svnlook.exe to get information
        /// about a particular repo and revision.
        /// </summary>
        /// <param name="command">The svnlook command e.g. log, author, message.</param>
        /// <param name="args">The arguments passed in to this exe (repo name and rev number).</param>
        /// <returns>The output of svnlook.exe</returns>
        private static string SVNLook(string command, string[] args)
        {
            StringBuilder output = new StringBuilder();
            Process procMessage = new Process();

            //Start svnlook.exe in a process and pass it the required command-line args.
            procMessage.StartInfo = new ProcessStartInfo(svnpath + @"bin\svnlook.exe", String.Format(@"{0} ""{1}"" -r ""{2}""", command, args[0], args[1]));
            procMessage.StartInfo.RedirectStandardOutput = true;
            procMessage.StartInfo.UseShellExecute = false;
            procMessage.Start();

            //While reading the output of svnlook, append it to the stringbuilder then
            //return the output.
            while (!procMessage.HasExited)
            {
                output.Append(procMessage.StandardOutput.ReadToEnd());
            }

            return output.ToString();
        }

    }
}
