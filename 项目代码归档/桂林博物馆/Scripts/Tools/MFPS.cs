using UnityEngine;

namespace Assets.Scripts.Tools
{
    /// <summary>
    /// 设置运行帧数
    /// </summary>
    public class MFPS : MonoBehaviour
    {

        void Awake()
        {
            Application.targetFrameRate = 300;
        }
    }
}