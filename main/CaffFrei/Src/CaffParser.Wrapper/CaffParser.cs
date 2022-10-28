using CaffParserWrapper.Data;
using System.Runtime.InteropServices;

namespace CaffParserWrapper;

public static class CaffParser
{
    [DllImport("CaffParser.Native.dll")]
    public static extern int Add(int a, int b);


    [DllImport("CaffParser.Native.dll")]
    public static extern CaffCredits ParseMeta(byte[] raw);

    [DllImport("CaffParser.Native.dll")]
    public static extern byte[] ParsePreview(byte[] raw);
}