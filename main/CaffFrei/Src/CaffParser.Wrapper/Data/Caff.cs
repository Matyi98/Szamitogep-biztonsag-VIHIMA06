using System.Runtime.InteropServices;

namespace CaffParserWrapper.Data;

// Initial stub implementation of the CAFF data classes
// TODO: refactor for C++ integration

[StructLayout(LayoutKind.Sequential)]
public struct Caff
{
    public CaffCredits Credits;
    public List<CaffFrame> Frames;
}

[StructLayout(LayoutKind.Sequential)]
public struct CaffFrame
{
    public long Duration;
    public Ciff Ciff;
}

[StructLayout(LayoutKind.Sequential)]
public struct Ciff
{
    public long Width;
    public long Height;
    [MarshalAs(UnmanagedType.BStr)]
    public string Caption;
    public List<string> Tags;
    public byte[] Content;
}

[StructLayout(LayoutKind.Sequential)]
public struct CaffCredits
{
    public short YY;
    public byte M;
    public byte D;
    public byte h;
    public byte m;
    [MarshalAs(UnmanagedType.BStr)]
    public string Creator;
}
