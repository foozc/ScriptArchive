using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Assets.Scripts.Log;


namespace Assets.Scripts.Logic
{
    public class GameObjectNode
    {
        GameObject gameObject = null;

        private List<GameObjectNode> children = null;

        public GameObject getGameObject()
        {
            return gameObject;
        }

        public List<GameObjectNode> getChildren()
        {
            return children;
        }

        public GameObjectNode getChild(int index)
        {
            return children[index];
        }

        public void setActive(bool on)
        {
            getGameObject().SetActive(on);
        }

        public void setColor(Color color, bool recursion = true)
        {
            if (getGameObject().GetComponent<Renderer>() != null)
            {
                getGameObject().GetComponent<Renderer>().material.color = color;
            }
            if (!recursion)
            {
                return;
            }
            if (children != null)
            {
                foreach (GameObjectNode node in children)
                {
                    node.setColor(color, true);
                }
            }
        }

        public void addChild(GameObjectNode node)
        {
            if (children == null)
            {
                children = new List<GameObjectNode>();
            }
            if (node == null)
            {
                Log.Logger.exception(Module.Resource,"GameObject of node is null");
                return;
            }
            children.Add(node);
        }

        public GameObjectNode(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}
