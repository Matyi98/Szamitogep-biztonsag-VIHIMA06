namespace CaffParserWrapper.Data;

// Initial stub implementation of the CAFF data classes
// TODO: refactor for C++ integration

public class Caff
{
    public CaffCredits Credits { get; set; }
    public List<CaffFrame> Frames { get; set; }
}

public class CaffFrame
{
    public long Duration { get; set; }
    public Ciff Ciff { get; set; }
}

public class Ciff
{
    public long Width { get; set; }
    public long Height { get; set; }
    public string Caption { get; set; }
    public List<string> Tags { get; set; }
    public byte[] Content { get; set; }
}

public class CaffCredits
{
    public short YY { get; set; }
    public byte M { get; set; }
    public byte D { get; set; }
    public byte h { get; set; }
    public byte m { get; set; }
    public string Creator { get; set; }
}
