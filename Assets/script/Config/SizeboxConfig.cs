using System;

public class SizeboxConfig
{
    private static int majorVersion = 0;
    private static int minorVersion = 0;
    private static int release = 0;
    private static int build = 1;
    private enum CheckVersion
    {
        Alpha,
        Beta,
        Final
    }
    public static string GetVersionNumber()
    {
        return majorVersion + "." + minorVersion + "." + release + "." + CheckVersion.Alpha.ToString();
    }
}
