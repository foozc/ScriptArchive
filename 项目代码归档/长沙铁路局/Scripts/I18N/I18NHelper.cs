using UnityEngine;
using System.Collections;

using Assets.Scripts.I18N;

namespace Assets.Scripts.I18N
{
    public class I18NHelper : MonoBehaviour
    {

        void Awake()
        {
            UILabel[] labels = GetComponentsInChildren<UILabel>();
            foreach (UILabel l in labels)
                l.gameObject.AddComponent<I18N>();
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}