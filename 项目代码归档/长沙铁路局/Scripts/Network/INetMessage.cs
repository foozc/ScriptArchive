using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Tools;


namespace Assets.Scripts.Network
{
    public interface INetMessage : IPoolObject
    {
        bool decode(byte[] data, int offset, int length);
        bool encode(byte[] data, int offset, out int length);
    }
}