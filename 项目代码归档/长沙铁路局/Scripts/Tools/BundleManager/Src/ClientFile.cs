using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClientFile
{
    private string filePath;
    private uint lzmaBeforeCRC;
    private long lzmaBeforeSize;
    private uint lzmaLaterCRC;
    private long lzmaLaterSize;

    public string FilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }

    public uint LzmaBeforeCRC
    {
        get { return lzmaBeforeCRC; }
        set { lzmaBeforeCRC = value; }
    }

    public long LzmaBeforeSize
    {
        get { return lzmaBeforeSize; }
        set { lzmaBeforeSize = value; }
    }

    public uint LzmaLaterCRC
    {
        get { return lzmaLaterCRC; }
        set { lzmaLaterCRC = value; }
    }

    public long LzmaLaterSize
    {
        get { return lzmaLaterSize; }
        set { lzmaLaterSize = value; }
    }



}

