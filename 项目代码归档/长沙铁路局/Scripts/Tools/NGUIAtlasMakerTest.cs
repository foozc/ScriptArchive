using UnityEngine;
using System.Collections;

public class NGUIAtlasMakerTest : MonoBehaviour {

    public UISprite sprite;
    private Texture2D[] texs;
    private UIAtlas atlas;

    void Start()
    {
        StartCoroutine(GetTupian());

        Invoke("TokeImage", 3.0f);
   
    }

    public void TokeImage()
    {
        NGUIAtlasMaker.atlasTrimming = true;
        NGUIAtlasMaker.atlasPMA = atlas != null ? atlas.premultipliedAlpha : false;
        NGUIAtlasMaker.unityPacking = false;
        NGUIAtlasMaker.atlasPadding = 1;
        NGUIAtlasMaker.allow4096 = true;
        NGUIAtlasMaker.UITexturePacker.forceSquareAtlas = true;

        if (atlas == null)
        {
            atlas = this.gameObject.AddComponent<UIAtlas>();
        }
        string lastName = string.Empty;
        foreach (var tex in texs)
        {
            NGUIAtlasMaker.AddOrUpdate(atlas, tex);
            lastName = tex.name;
        }
        sprite.atlas = atlas;
        sprite.spriteName = lastName;
    }


    IEnumerator GetTupian()
    {
        WWW www = new WWW("http://pic.baike.soso.com/p/20090711/20090711101754-314944703.jpg");
        yield return www;
        Texture2D tu = www.texture;
        tu.name = www.texture.name;
        texs = new Texture2D[] { tu };
        Debug.Log(www.texture.name);
    }

}
