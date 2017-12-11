using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.UI;

public class HighLightModel : MonoBehaviour
{
    public float Distance;
    public Camera nCamera;
    private GameObject model;
    private bool isHighLight = false;
    private string textStr = null;

    private float size_y;
    private float scal_y;
    private float modelHight;
    private List<Highlighter> Highlighters = new List<Highlighter>();
    private void Update()
    {
        if (model != null && isHighLight)
        {
            HighLight();
            setTextPosition();
        }
    }

    /// <summary>
    /// faguagn
    /// </summary>
    /// <param name="model"></param>
    /// <param name="isHight"></param>
    public void SetHighLight(GameObject model, bool isHight)
    {
        this.model = model;
        isHighLight = isHight;
        size_y = model.GetComponent<Collider>().bounds.size.y;
        scal_y = transform.localScale.y;
        modelHight = (size_y * scal_y);
        for (int i = 0; i < Highlighters.Count; i++)
        {
            Highlighters[i].Off();
        }
        Highlighters.Clear();
        if (DataManager.instance.getModeData(model.name) == null)
        {
            return;
        }
        if (!DataManager.instance.getModeData(model.name).IsShow)
            addHighlighter(model.transform, Highlighters);
    }

    private void addHighlighter(Transform tr, List<Highlighter> hs)
    {
        if (tr.GetComponent<MeshRenderer>() != null)
        {
            if (tr.GetComponent<Highlighter>() == null)
                tr.gameObject.AddComponent<Highlighter>();
            hs.Add(tr.GetComponent<Highlighter>());
        }
        if (tr.childCount > 0)
            for (int i = 0; i < tr.childCount; i++)
                addHighlighter(tr.GetChild(i), hs);
    }

    private void HighLight()
    {
        for (int i = 0; i < Highlighters.Count; i++)
        {
            Highlighter h = Highlighters[i];
            if (h != null && isHighLight)
            {
                h.On(new Color(0, 199, 234));
            }
            else if (h != null)
            {
                h.Off();
            }
        }
    }
    //public void setText(string str)
    //{
    //    if (textStr != null || textStr != "")
    //    {
    //        text.text = str;
    //    }
    //}
    public void setTextPosition()
    {
        if (model != null)
        {
            Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + modelHight, transform.position.z);
            Vector2 position = nCamera.WorldToScreenPoint(worldPosition);
            position = new Vector2(position.x - Screen.width / 2, position.y - Screen.height / 2 + Distance);
            //eadImage.rectTransform.localPosition = position;
        }
    }
}
