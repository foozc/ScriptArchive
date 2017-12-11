using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:游戏对象渐变透明
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class FadeGameObject : MonoBehaviour
    {

        private AnimationCurve alphaCurve = AnimationCurve.Linear(0, 0, 1f, 0);  //透明度曲线/
        private Dictionary<Transform, List<Material>> materialDic;
        private List<Material> alphaMaterial;
        private bool startFade = true;
        private float startTime;
        private float duration = 0f;
        private bool isShow;

        public void setFadeObjects(List<GameObject> objs, bool show)
        {
            isShow = show;
            if (materialDic == null)
            {
                materialDic = new Dictionary<Transform, List<Material>>();
                alphaMaterial = new List<Material>();
            }
            int frameCount = alphaCurve.length;
            duration = alphaCurve[frameCount - 1].time;
            foreach (GameObject obj in objs)
            {
                if(obj.GetComponent<MeshRenderer>() != null)
                {
                    Material[] materials = obj.GetComponent<MeshRenderer>().materials;
                    List<Material> mates = new List<Material>();
                    List<Material> newMaterial = new List<Material>();
                    for (int i = 0; i < materials.Length; i++)
                    {
                        Material ma = new Material(Shader.Find("Legacy Shaders/Transparent/Cutout/Soft Edge Unlit"));
                        ma.mainTexture = materials[i].mainTexture;
                        if (show)
                            ma.color = new Color(ma.color.r, ma.color.g, ma.color.b, 0);
                        mates.Add(materials[i]);
                        alphaMaterial.Add(ma);
                        newMaterial.Add(ma);
                    }
                    obj.GetComponent<MeshRenderer>().materials = newMaterial.ToArray();
                    materialDic.Add(obj.transform, mates);
                    obj.GetComponent<MeshRenderer>().enabled = true;
                }
                StartCoroutine(alphaTween());
            }
        }

        public void setFadeObject(GameObject obj, bool show)
        {
            isShow = show;
            if (materialDic == null)
            {
                materialDic = new Dictionary<Transform, List<Material>>();
                alphaMaterial = new List<Material>();
            }
            int frameCount = alphaCurve.length;
            duration = alphaCurve[frameCount - 1].time;
            List<Transform> meshs = new List<Transform>();
            getAllChildMeshRenderer(obj.transform, meshs);

            foreach(Transform tr in meshs)
            {
                Material[] materials = tr.GetComponent<MeshRenderer>().materials;
                List<Material> mates = new List<Material>();
                List<Material> newMaterial = new List<Material>();
                for(int i = 0; i < materials.Length; i++)
                {
                    Material ma = new Material(Shader.Find("Legacy Shaders/Transparent/Cutout/Soft Edge Unlit"));
                    ma.mainTexture = materials[i].mainTexture;
                    if(show)
                        ma.color = new Color(ma.color.r, ma.color.g, ma.color.b, 0);
                    mates.Add(materials[i]);
                    alphaMaterial.Add(ma);
                    newMaterial.Add(ma);
                }
                tr.GetComponent<MeshRenderer>().materials = newMaterial.ToArray();
                materialDic.Add(tr, mates);
                tr.GetComponent<MeshRenderer>().enabled = true;
            }
            StartCoroutine(alphaTween());
        }

        private void getAllChildMeshRenderer(Transform tr, List<Transform> trs)
        {
            if (tr.GetComponent<BoxCollider>() != null)
                tr.GetComponent<BoxCollider>().enabled = isShow;
            if (tr.GetComponent<MeshRenderer>() == null)
            {
                if (tr.childCount != 0)
                    foreach (Transform mytr in tr.transform)
                        getAllChildMeshRenderer(mytr, trs);
            }
            else
                trs.Add(tr);
        }

        private IEnumerator alphaTween()
        {
            startTime = Time.time;
            while (startFade)
            {
                float t = Time.time - startTime;
                if (t < duration)
                {
                    float alpha = t / duration;
                    foreach(Material ma in alphaMaterial)
                    {
                        if(isShow)
                            ma.SetColor("_Color", new Color(ma.color.r, ma.color.g, ma.color.b, alpha));
                        else
                            ma.SetColor("_Color", new Color(ma.color.r, ma.color.g, ma.color.b, 1 - alpha));
                    }
                }
                else
                {
                    startFade = false;
                }
                yield return null;
            }
            //渐变完成
            foreach(Transform tr in materialDic.Keys)
            {
                if (!isShow)
                    tr.GetComponent<MeshRenderer>().enabled = false;
                tr.GetComponent<MeshRenderer>().materials = materialDic[tr].ToArray();
            }
            Destroy(this);
        }


        public static void showHiddenObject(string objName, bool isShow)
        {
            GameObject obj = GameObject.Find(objName);
            showHidehildMeshRenderer(obj.transform, isShow);
        }

        public static void showHideTagObject(string tag, bool isShow)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].GetComponent<MeshRenderer>() != null)
                {
                    objs[i].GetComponent<MeshRenderer>().enabled = isShow;
                    if (objs[i].GetComponent<BoxCollider>() != null)
                        objs[i].GetComponent<BoxCollider>().enabled = isShow;
                }
            }
        }

        private static void showHidehildMeshRenderer(Transform tr, bool isShow)
        {
            if (tr.GetComponent<BoxCollider>() != null)
                tr.GetComponent<BoxCollider>().enabled = isShow;
            if (tr.GetComponent<MeshRenderer>() == null)
            {
                if (tr.childCount != 0)
                    foreach (Transform mytr in tr.transform)
                        showHidehildMeshRenderer(mytr, isShow);
            }
            else
            {
                tr.GetComponent<MeshRenderer>().enabled = isShow;
            }
			if (tr.GetComponent<SkinnedMeshRenderer>() == null)
			{
				if (tr.childCount != 0)
					foreach (Transform mytr in tr.transform)
						showHidehildMeshRenderer(mytr, isShow);
			}
			else
			{
				tr.GetComponent<SkinnedMeshRenderer>().enabled = isShow;
			}
		}

        public static void alphaShowHideTagObject(string tag, bool isShow)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            if (objs != null)
            {
                FadeGameObject fade = objs[0].AddComponent<FadeGameObject>();
                fade.setFadeObjects(new List<GameObject>(objs), isShow);
            }
        }

        public static void alphaShowHideObject(string objName, bool isShow)
        {
            GameObject obj = GameObject.Find(objName);
            FadeGameObject fade = obj.AddComponent<FadeGameObject>();
            fade.setFadeObject(obj, isShow);
        }
		public static void PlayAudio(string path)
		{
			AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.groundMusic);
			AudioManager.getInstance().AudioPlay(path, AudioManager.MusicNumType.groundMusic);
		}
	}
}
