using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Google.ProtocolBuffers;
using PBMsg = Google.ProtocolBuffers.IMessage;


namespace Assets.Scripts.Network
{

    public delegate T ByteStringDecodeHandler<T>(ByteString input);
    public class PBNetMessage :INetMessage
    {

        public static DecodeHandler<PBMsg> createPBDecodeHandler<T>(ByteStringDecodeHandler<T> decode) where T : PBMsg
        { 
            return delegate (byte[] bytes , int offset , int length)
            {
                var bs = ByteString.CopyFrom(bytes, offset , length);
                return decode(bs);
            };
        }

        public static EncodeHandler<PBMsg> createPBEncodeHandler<T>() where T : PBMsg
        {
            return delegate(PBMsg msg)
            {
                MemoryStream stream = new MemoryStream();
                msg.WriteTo(stream);
                return stream.ToArray();
            };
        }

        private PBMsg msg;

        private uint id;

        private string name;

        private byte[] data;

        public void setID(uint id)
        {
            this.id = id;
        }

        public uint getID()
        {
            return this.id;
        }

        public void setMessage(PBMsg msg)
        {
            this.msg = msg;
        }

        public PBMsg getMessage()
        {
            return msg;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return this.name;
        }

        public void setData(byte[] data)
        {
            this.data = data;
        }

        public byte[] getData()
        {
            return this.data;
        }


        public bool decode(byte[] data, int offset, int length)
        {
            return true;
        }

        public bool encode(byte[] data, int offset, out int length)
        {
            length = 0;
            return true;
        }

        public void reset()
        {
            msg = null;
            id = 0;
            name = null;
            data = null;
        }

    }
}
