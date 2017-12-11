using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Assets.Scripts.Network
{


    public delegate T DecodeHandler<T> (byte [] data, int offset , int length) ;

    public delegate byte[] EncodeHandler<T> (T  t);

    public class CodecManager<T,D,E>
    {
        private Dictionary<uint, D> decoders = null;

        private Dictionary<uint, E> encoders = null;

        private static CodecManager<T, D, E> instance = null;

        private CodecManager()
        {
            decoders = new Dictionary<uint, D>();
            encoders = new Dictionary<uint, E>();
        }

        public static CodecManager<T, D, E> getInstance()
        {
            if (instance == null)
            {
                instance = new CodecManager<T, D, E>();
            }
            return instance;
        }

        public void registerCoder(Type t, D decoder, E encoder)
        {
            uint id = getHashID(t);
            if (!decoders.ContainsKey(id))
            {
                decoders.Add(id, decoder);
            }
            else
            {
                Log.Logger.warn(Log.Module.Network, "already exist decoder:" + t.ToString() + ", ignore registration");
            }

            if(!encoders.ContainsKey(id))
            {
                encoders.Add(id,encoder);
            }
            else
            {
                Log.Logger.warn(Log.Module.Network,"already exist encoder:" + t.ToString() + ", ignore registration");
            }
        }

        private static uint getHashID(string str)
        {
            uint hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = ((hash * 16777619) & 0xffffffff) ^ (char.ToUpper(str[i]));
            }
            return hash;
        }


        public static uint getHashID(Type t)
        {
            return getHashID(t.Name);
        }

        public T decode(uint id, byte[] bytes, int offset, int length)
        {
            if (decoders.ContainsKey(id))
            {
                DecodeHandler<T> decodeHandler = decoders[id] as DecodeHandler<T>;
                if (decodeHandler == null)
                {
                    Log.Logger.warn(Log.Module.Network, "DecodeHandler cast from " + decoders[id] + " is null");
                    return default(T);
                }
                return decodeHandler.Invoke(bytes, offset, length);
            }
            Log.Logger.warn(Log.Module.Network, "do not exist decoder with msg type:" + id + ", ignore encode");
            return default(T);
        }

        public byte[] encode(Type t, T r)
        {
            uint id = getHashID(t);
            if (encoders.ContainsKey(id))
            {
                EncodeHandler<T> encodeHandler = encoders[id] as EncodeHandler<T>;
                if (encodeHandler == null)
                {
                    Log.Logger.warn(Log.Module.Network, "EncodeHandler cast from " + encoders[id] + " is null");
                    return null;
                }
                return encodeHandler.Invoke(r);
            }
            Log.Logger.warn(Log.Module.Network, "do not exist encoder with msg type:" + t + ", ignore encode");
            return null;
        }
    }
}
