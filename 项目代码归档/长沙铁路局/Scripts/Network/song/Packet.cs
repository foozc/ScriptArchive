using System.IO;
using System.Text;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Network;

//using Google.ProtocolBuffers.Descriptors;
//using Google.ProtocolBuffers;
//using PBMsg = Google.ProtocolBuffers.IMessage;

using ProtoBuf;

namespace Assets.Scripts.Network
{
    public delegate T PacketByteStringDecodeHandler<T>(MemoryStream stream, ref Packet packet);
    public class Packet : INetMessage
    {
        private const byte INIT_BODY_SIZE = 8;
        public const byte CLIENT_HEAD_SIZE = 12;
        public const byte SERVER_HEAD_SIZE = 24;
        public const int SIZE_MAX = short.MaxValue;

        public const short RESULT_SUCCESS = 0;
        public const short RESULT_ERROR = -1;

        private uint id;
        public void setID(uint v)
        {
            this.id = v;
        }

        public uint getID()
        {
            return id;
        }
        private string name;
        public void setName(string v)
        {
            this.name = v;
        }

        public string getName()
        {
            return name;
        }


        private long clientId = 0;
        /// <summary>
        /// 客户端编号（临时）
        /// </summary>
        public long ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }

        private short result = 0;
        /// <summary>
        /// 返回值
        /// </summary>
        public short Result
        {
            get { return result; }
            set { result = value; }
        }
        private short option = 0;
        /// <summary>
        /// 选项参数
        /// </summary>
        public short Option
        {
            get { return option; }
            set { option = value; }
        }
        private short opcode = 0;
        /// <summary>
        /// 操作码
        /// </summary>
        public short Opcode
        {
            get { return opcode; }
            set { opcode = value; }
        }
        private int key = 0;
        /// <summary>
        /// 键：通常为玩家编号
        /// </summary>
        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        public byte[] body = new byte[INIT_BODY_SIZE];
        private int rpos = 0;
        private int wpos = 0;


        public int Size
        {
            get
            {
                return this.Available + CLIENT_HEAD_SIZE;
            }
        }

        public Packet()
        {

        }



        private short getShort()
        {
            short value = 0;
            value += (short)((this.body[rpos++] & byte.MaxValue) << 8);
            value += (short)(this.body[rpos++] & byte.MaxValue);
            return value;
        }

        private void putShort(int value)
        {
            extCapacity(wpos + 2);
            this.body[wpos++] = (byte)((value >> 8) & byte.MaxValue);
            this.body[wpos++] = (byte)(value & byte.MaxValue);
        }

        private void extCapacity(int minCapacity)
        {
            int curCapacity = this.body.Length;
            if (minCapacity < curCapacity)
                return;
            while (curCapacity < minCapacity)
                curCapacity <<= 1;
            this.setCapacity(curCapacity);
        }

        private void setCapacity(int capacity)
        {
            byte[] new_body = new byte[capacity];
            if (this.body != null)
                Array.Copy(this.body, this.rpos, new_body, 0, this.wpos);
            this.body = new_body;
        }

        private int getInt()
        {
            int value = 0;
            value += (this.body[rpos++] & byte.MaxValue) << 24;
            value += (this.body[rpos++] & byte.MaxValue) << 16;
            value += (this.body[rpos++] & byte.MaxValue) << 8;
            value += (this.body[rpos++] & byte.MaxValue);
            return value;
        }
        /// <summary>
        /// 写入字节数组
        /// </summary>
        private void put(byte[] date)
        {
            int putLength = date.Length;
            this.extCapacity(this.wpos + putLength);
            Array.Copy(date, 0, this.body, this.wpos, putLength);
            this.wpos += putLength;
        }

        /******************************ProtocolBuffers写法***************************/
        //private void put<T>(T msg) where T : IMessage
        //{
        //    setName(typeof(T).Name);
        //    setID(PacketCodecManager<INetMessage, PacketDecodeHandler<IList>, PacketEncodeHandler>.getHashID(typeof(T)));
        //    this.put(msg.ToByteString());
        //}
        ///// <summary>
        ///// 写入字节串
        ///// </summary>
        //private void put(ByteString irw)
        //{
        //    this.putInt(irw.Length);
        //    this.put(irw.ToByteArray());
        //}
        ///// <summary>
        ///// 写入消息
        ///// </summary>
        //private void put(IMessage message)
        //{
        //    this.put(message.ToByteString());
        //}
        ///// <summary>
        ///// 写入消息构造器中的消息
        ///// </summary>
        //private void put(IBuilder builder)
        //{
        //    this.put(builder.WeakBuild());
        //}

        /// <summary>
        /// 
        /// </summary>
        //public void get(IBuilder builder)
        //{
        //    try
        //    {
        //        int length = this.getInt();
        //        if (length <= this.Available)
        //        {
        //            MemoryStream bufferStream = new MemoryStream(this.body, this.rpos, length);
        //            byte[] buffer = bufferStream.ToArray();
        //            bufferStream.Close();
        //            ByteString byteString = ByteString.CopyFrom(buffer);
        //            builder.WeakMergeFrom(byteString);
        //            this.rpos += length;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        Console.WriteLine(e.StackTrace);
        //    }
        //}

        /***********************************************************/


        /******************************ProtoBuf-Net写法***************************/
        public void put<T>(T obj)
            where T : ProtoBuf.IExtensible
        {
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize<T>(ms, (T)obj);
            ms.Position = 0;
            byte[] temp = ms.ToArray();
            ms.Close();

            this.putInt(temp.Length);
            this.put(temp);
        }

        /// <summary>
        /// 获取包体数据信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T get<T>()
            where T : ProtoBuf.IExtensible
        {
            try
            {
                int length = this.getInt();
                if (length <= this.Available)
                {
                    MemoryStream bufferStream = new MemoryStream(this.body, this.rpos, length);
                    byte[] buffer = bufferStream.ToArray();
                    bufferStream.Close();
                    MemoryStream ms = new MemoryStream(buffer, true);
                    T rp = Serializer.Deserialize<T>(ms);

                    ms.Close();
                    return rp;
                }
            }
            catch (Exception e)
            {
                Log.Logger.error(Log.Module.Network, e.Message);
                Log.Logger.error(Log.Module.Network, e.StackTrace);
            }
            return default(T);
        }

        /***********************************************************/


        /// <summary>
        /// 写入整数
        /// </summary>
        public void putInt(int value)
        {
            this.extCapacity(wpos + 4);
            this.body[wpos++] = (byte)((value >> 24) & byte.MaxValue);
            this.body[wpos++] = (byte)((value >> 16) & byte.MaxValue);
            this.body[wpos++] = (byte)((value >> 8) & byte.MaxValue);
            this.body[wpos++] = (byte)(value & byte.MaxValue);
        }


        public bool readPacket(MemoryStream dis)
        {
            // 分配长度
            int readLength = (int)dis.Length;
            this.extCapacity(readLength + this.wpos);
            // 获取包体
            dis.Read(this.body, this.wpos, readLength);
            this.wpos += readLength;
            //
            return true;
        }
        public bool readPacket(MemoryStream dis, int readLength)
        {
            // 分配长度
            this.extCapacity(readLength + this.wpos);
            // 获取包体
            dis.Read(this.body, this.wpos, readLength);
            this.wpos += readLength;
            //
            return true;
        }

        //public bool readClientPacket(MemoryStream dis)
        //{
        //    int readLength = dis.Capacity + 2;
        //    this.extCapacity(readLength + this.wpos);

        //    this.wpos += 2;
        //    dis.Read(this.body, this.wpos, readLength - 2);
        //    this.wpos += (readLength - 2);
        //    //
        //    return true;
        //}

        public void writePacket(BinaryWriter dos)
        {
            dos.Write(this.body, this.rpos, this.Available);
        }

        //public void writeClientPacket(BinaryWriter dos)
        //{
        //    if (this.Available < 2)
        //        dos.Write(this.body, this.rpos, this.Available);
        //    else
        //        dos.Write(this.body, this.rpos + 2, this.Available - 2);
        //}

        public void reset()
        {
            this.id = 0;
            this.name = null;
            this.clientId = 0;
            this.result = 0;
            this.Opcode = 0;
            this.option = 0;
            this.key = 0;
            Array.Clear(body, 0, body.Length);
            body = new byte[INIT_BODY_SIZE];
            this.rpos = 0;
            this.wpos = 0;
        }

        /// <summary>
        /// 有效数据长度
        /// </summary>
        public int Available
        {
            get
            {
                return this.wpos - this.rpos;
            }
        }

        /// <summary>
        /// packet总长度，包括包头文件和数据文件
        /// </summary>
        public int PacketAllAvailable
        {
            get
            {
                return Available + Packet.CLIENT_HEAD_SIZE;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Pak[")
                .Append("c").Append(this.opcode)
                .Append(",o").Append(this.option)
                .Append(",s").Append(this.Size)
                .Append(",k").Append(this.key)
                .Append("]");
            return sb.ToString();
        }

        public Packet copyHead()
        {
            Packet packet = new Packet();
            packet.ClientId = this.clientId;
            packet.Key = this.key;
            packet.Opcode = this.opcode;
            packet.Option = this.option;
            packet.Result = this.result;

            return packet;
        }


        //编码（序列化）
        public static byte[] encodeHttp(Packet packet)
        {
            MemoryStream outputStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(outputStream);

            /*************************************Head*************************************/
            int size = Packet.CLIENT_HEAD_SIZE + packet.Available;//总长度
            sbyte[] headBytes = new sbyte[Packet.CLIENT_HEAD_SIZE];//只写入包头的长度
            int index = 0;

            headBytes[index++] = (sbyte)((packet.opcode >> 8) & byte.MaxValue);
            headBytes[index++] = (sbyte)(packet.opcode & byte.MaxValue);
            //
            headBytes[index++] = (sbyte)((size >> 8) & byte.MaxValue);
            headBytes[index++] = (sbyte)(size & byte.MaxValue);
            //
            headBytes[index++] = (sbyte)((packet.key >> 24) & byte.MaxValue);
            headBytes[index++] = (sbyte)((packet.key >> 16) & byte.MaxValue);
            headBytes[index++] = (sbyte)((packet.key >> 8) & byte.MaxValue);
            headBytes[index++] = (sbyte)(packet.key & byte.MaxValue);
            //
            headBytes[index++] = (sbyte)((packet.result >> 8) & byte.MaxValue);
            headBytes[index++] = (sbyte)(packet.result & byte.MaxValue);
            //
            headBytes[index++] = (sbyte)((packet.option >> 8) & byte.MaxValue);
            headBytes[index++] = (sbyte)(packet.option & byte.MaxValue);
            /*************************************Head*************************************/
            //
            foreach (sbyte b in headBytes)
            {
                writer.Write(b);
            }

            /*************************************Body*************************************/
            packet.writePacket(writer);
            /*************************************Body*************************************/
            byte[] data = outputStream.ToArray();
            return data;
        }

        //解码（反序列化）
        private const int BufferSize = short.MaxValue;
        private static byte[] buffer = new byte[BufferSize];

        public static List<Packet> decodeHttp(Stream responseStream)
        {

            List<Packet> list = new List<Packet>();
            Array.Clear(buffer, 0, buffer.Length);
            try
            {
                //读取全部字节
                MemoryStream bufferStream = new MemoryStream();
                int dataLength;
                int count = 0;
                while ((dataLength = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    bufferStream.Write(buffer, 0, dataLength);
                    count += dataLength;
                }
                bufferStream.Seek(0, SeekOrigin.Begin);
                //循环读取包
                while (bufferStream.Position < bufferStream.Length)
                {
                    Packet packet = new Packet();
                    decodeHttps(bufferStream, ref packet);

                    list.Add(packet);
                }

                bufferStream.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }


            return list;
        }


        public static Packet decodeHttps(MemoryStream bufferStream, ref Packet packet)
        {
            //Packet packet = new Packet();

            if (bufferStream.Length >= Packet.CLIENT_HEAD_SIZE)
            {
                /*************************************Head*************************************/
                packet.opcode = (short)((bufferStream.ReadByte() & byte.MaxValue) << 8);
                packet.opcode |= (short)(bufferStream.ReadByte() & byte.MaxValue);

                int size = (bufferStream.ReadByte() & byte.MaxValue) << 8;
                size |= (bufferStream.ReadByte() & byte.MaxValue);

                packet.key = (bufferStream.ReadByte() & byte.MaxValue) << 24;
                packet.key |= (bufferStream.ReadByte() & byte.MaxValue) << 16;
                packet.key |= (bufferStream.ReadByte() & byte.MaxValue) << 8;
                packet.key |= (bufferStream.ReadByte() & byte.MaxValue);
                //
                packet.result = (short)((bufferStream.ReadByte() & byte.MaxValue) << 8);
                packet.result = (short)(bufferStream.ReadByte() & byte.MaxValue);
                //
                packet.option = (short)((bufferStream.ReadByte() & byte.MaxValue) << 8);
                packet.option |= (short)(bufferStream.ReadByte() & byte.MaxValue);
                /*************************************Head*************************************/

                /*************************************Body*************************************/
                //判断包是否是否完整，不完整则不解析
                if (bufferStream.Length - bufferStream.Position >= size - Packet.CLIENT_HEAD_SIZE)
                    packet.readPacket(bufferStream, size - Packet.CLIENT_HEAD_SIZE);
                else 
                    return packet = null;
                /*************************************Body*************************************/

                return packet;
            }
            else
            {
                return packet = null;
            }
        }

        /// <summary>
        /// 创建packet的解码委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="decode"></param>
        /// <returns></returns>
        public static PacketDecodeHandler<Packet> createPBDecodeHandler<T>(PacketByteStringDecodeHandler<T> decode) where T : Packet
        {
            return delegate(MemoryStream stream, ref Packet packet)
            {
                return decode(stream, ref packet);
            };
        }

        /// <summary>
        /// 创建packet编码委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PacketEncodeHandler createPBEncodeHandler()
        {
            return delegate(Packet msg)
            {
                return encodeHttp(msg);
            };
        }

        public static Packet getNewPacket()
        {
            return MyGameNetwork.getInstance().getPoolPacket();
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
    }
}