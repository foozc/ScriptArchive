using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Assets.Scripts.Logic;
using Assets.Scripts.Log;
using Assets.Scripts.Tools;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明：资源管理类
*Author:作者
*
*/
namespace Assets.Scripts.Controller
{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public class ResourceManager
    {
        private static ResourceManager instance = null;

        private SpawnPool spawnPoolUI = null;

        public static ResourceManager getInstance()
        {
            if (instance == null)
            {
                instance = new ResourceManager();
            }
            return instance;
        }
        public ResourceManager()
        {

        }

        public GameObjectNode load(IResourcable r, GameObject parent)
        {
            return load(r.getResource(), parent);
        }

        private GameObjectNode load(Resource resource, GameObject parent)
        {
            return load(resource, parent, null);
        }




        public GameObjectNode load(Resource res, GameObject parent, GameObjectNode parentNode)
        {
            UnityEngine.Object o = Resources.Load(res.getPath());


            if (o == null)
            {
                Log.Logger.warn(Module.Resource, "Resources.Load:" + res + " failed!");
                return null;
            }

            if (res.getType() == Resource.Type.Font)
            {
                GameObjectNode node = new GameObjectNode(o as GameObject);
                return node;
            }

            GameObject obj = GameObject.Instantiate(o) as GameObject;
            GameObjectNode gon = new GameObjectNode(obj);
            if (parentNode != null)
            {
                parentNode.addChild(gon);
            }
            List<Resource> ress = res.getChildren();

            if (ress != null && ress.Count > 0)
            {
                foreach (Resource r in ress)
                {
                    load(r, obj, gon);
                }
            }
            obj.transform.parent = parent.transform;
            obj.transform.name = Utils.subtractBracket(obj.transform.name, "(Clone)");
            obj.transform.localScale = res.getScale();
            obj.transform.localRotation = res.getAngle();
            obj.transform.localPosition = res.getPos();
            return gon;
        }

        /// <summary>
        /// 临时做法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObjectNode load(string path, GameObject parent)
        {
            UnityEngine.Object o = Resources.Load(path) as GameObject;
            if (o == null)
            {
                Log.Logger.warn(Module.Resource, "Resources.Load:" + path + " failed!");
                return null;
            }

            GameObject obj = GameObject.Instantiate(o) as GameObject;
            GameObjectNode gon = new GameObjectNode(obj);
            //GameObjectNode parentNode = new GameObjectNode(parent);
            //if (parentNode != null)
            //{
            //    parentNode.addChild(gon);
            //}
            obj.transform.parent = parent.transform;
            obj.transform.name = Utils.subtractBracket(obj.transform.name, "(Clone)");
            obj.transform.localScale = Vector3.one;
            //obj.transform.localPosition = Vector3.zero;
            return gon;
        }

        public void destroyUILoad(GameObject obj)
        {

            //if (getUIPool.prefabPools.ContainsKey(obj.name))
            if (getUIPool.IsSpawned(obj.transform))
            {
                getUIPool.Despawn(obj.transform, getUIPool.transform);
            }
            else
            {
                GameObject.Destroy(obj);
                Resources.UnloadAsset(obj);
            }

        }

        /// <summary>
        /// 从Resource文件夹中实例化物体，并由内存池管理
        /// </summary>
        /// <returns></returns>
        public GameObject loadUISpawnPool(string path, GameObject parent)
        {
            GameObject o = Resources.Load(path) as GameObject;
            if (o == null)
            {
                Log.Logger.warn(Module.Resource, "Resources.Load:" + path + " failed!");
                return null;
            }
            return loadUISpawnPool(o, parent);
        }

        public GameObject loadUISpawnPool(GameObject resourceObj, GameObject parent)
        {
            Transform obj;
            if (parent != null)
                obj = getUIPool.Spawn(resourceObj.transform, parent.transform);
            else obj = getUIPool.Spawn(resourceObj.transform);
            obj.transform.localScale = Vector3.one;
            return obj.gameObject;
        }
        

        public SpawnPool getUIPool
        {
            get
            {
                SpawnPool[] pools = GameObject.FindObjectsOfType<SpawnPool>();
                foreach (SpawnPool sp in pools)
                {
                    if (sp.transform.name.Equals("UIPool"))
                    {
                        this.spawnPoolUI = sp;
                    }
                }
                if (this.spawnPoolUI == null)
                {
                    this.spawnPoolUI = PoolManager.Pools.Create("UI");
                    //this.spawnPoolUI.transform.parent = GameObject.Find("UI Root").transform;
                    this.spawnPoolUI.transform.localScale = Vector3.one;
                    this.spawnPoolUI.matchPoolScale = true;
                    this.spawnPoolUI._dontDestroyOnLoad = true;
                }
                return this.spawnPoolUI;
            }
        }

    }
}
