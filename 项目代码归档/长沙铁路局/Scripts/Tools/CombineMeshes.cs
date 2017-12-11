using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Tools
{
    public class CombineMeshes : MonoBehaviour
    {
        void Start()
        {


            ///有待优化，等正式角色出来以后我们再优化
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length)
            {

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            if (transform.GetComponent<MeshFilter>().sharedMesh != null)
            {
                transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
                transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
                transform.gameObject.SetActive(transform);
            }
        }
    }
}