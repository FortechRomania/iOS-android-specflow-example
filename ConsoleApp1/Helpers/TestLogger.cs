using System;
using System.IO;
using NUnit.Framework;

namespace Just4Fun.ConsoleApp1.Helpers
{
    // See https://github.com/nunit/nunit/issues/2182#issuecomment-445461787
    public static class TestLogger
    {
        public static TextWriter CurrentTextWriter { get; set; }

        public static void WriteLine(string line)
        {
            (CurrentTextWriter ?? TestContext.Out).WriteLine($"{DateTime.Now} {line}");
        }
    }
}
