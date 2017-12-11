using System;

namespace Assets.Scripts.Network
{
    public class DummyNetMessage : INetMessage
    {
        byte[] data = null;

        int offset = 0;

        int length = 0;

        public void reset()
        { 
            //do something
        }

        public bool decode(byte[] data, int offset, int length)
        {
            this.data = data;
            this.offset = offset;
            this.length = length;
            return true;
        }

        public bool encode(byte[] data, int offset, out int length)
        {
            for (int i = 0; i < this.length; i++)
            {
                data[offset + i] = this.data[this.offset + i];   
            }
            length = this.length;
            return true;
        }
    }
}
