using System;
using System.IO;
using NUnit.Framework;

namespace IosAndroidSpecflowExample.Helpers
{
    // See https://github.com/nunit/nunit/issues/2182#issuecomment-445461787
    public static class TestLogger
    {
        public static TextWriter CurrentTextWriter { get; set; }

        public static void WriteLineToTestOutput(string line)
        {
            (CurrentTextWriter ?? TestContext.Progress).WriteLine(LineWithTimeInformation(line));
        }

        public static void WriteLineToConsoleOutput(string line)
        {
            Console.Out.WriteLine(LineWithTimeInformation(line));
        }

        private static string LineWithTimeInformation(string line) => $"{DateTime.Now} {line}";
    }
}
