using CaffParserWrapper.Data;
using System.Runtime.InteropServices;

namespace CaffParserWrapper;

public static class CaffParser
{
    [DllImport("CaffParser.Native.dll")]
    public static extern int Add(int a, int b);


    [DllImport("CaffParser.Native.dll")]
    private static extern CaffCredits ParseMeta(byte[] raw, long size);
    public static CaffCredits ParseMeta(byte[] raw) => ParseMeta(raw, raw.LongLength);

    [DllImport("CaffParser.Native.dll")]
    private static extern byte[] ParsePreview(byte[] raw, long size);
    public static byte[] ParsePreview(byte[] raw) => ParsePreview(raw, raw.LongLength);
}