using UnityEngine;
using System.Collections;

public class MySpriteAnimation : MonoBehaviour
{
    // 这两个图集 也可以是一个，手动拖拽进来任意两个图集 两图集的 sprite 要尽量一致  
    //public UIAtlas atlas;  
    //public UIAtlas atlasNormal;  
    // 图片第一帧所在的位置  
    public int spriteCount;
    public string theNamePrefix;
    // 需要添加的精灵动画组件，你可以在 Componen 中的 NGUI 的下级 UI里面找到它  
    //UISpriteAnimation animation;  
    public UISprite sprite;
    // Use this for initialization  
    void Start()
    {
        //sprite = GetComponent<UISprite>();  
        // 如果是按钮，可以在 他的孩子中找到 UISprite 或者你还可以直接拖拽你想要改变的任一个 UISprite 到 sprite（你要保持它为 public）  
        if (sprite == null)
            sprite = GetComponentInChildren<UISprite>();
        //animation = GetComponentInChildren<UISpriteAnimation>();  


        string name = sprite.atlas.spriteList[spriteCount].name;
        sprite.spriteName = name;
        // 使精灵 sprite 规模化 也可以自己调整它的 localScale 使符合要播放的图集动画大小  
        //sprite.MakePixelPerfect();  
        //sprite.transform.localScale = new Vector3(108f, 107f, 0);  
    }

    // Update is called once per frame  
    void Update()
    {
    }

    void OnHover(bool isOver)
    {
        if (isEnabled || sprite != null)
        {
            //base.OnHover(isOver);  
            //sprite.spriteName = isOver ? hoverSprite : normalSprite;  
            if (isOver)
            {
                UISpriteAnimation animation = sprite.gameObject.AddComponent<UISpriteAnimation>();
                animation.framesPerSecond = 25;
                // 当鼠标悬停的时候，改变 sprite 的精灵图集  
                //sprite.atlas = atlasNormal;  
                // 名字可以手动写，或者像下面一样自动获取  
                sprite.spriteName = sprite.atlas.spriteList[spriteCount].name;
                animation.namePrefix = theNamePrefix;
                animation.enabled = true;
                //sprite.MakePixelPerfect();  
            }
            else
            {
                Destroy(sprite.transform.GetComponent<UISpriteAnimation>());
                //animation.enabled = false;  
                //sprite.atlas = atlasNormal;  
                sprite.spriteName = sprite.atlas.spriteList[spriteCount].name;
                //sprite.MakePixelPerfect();  
                //sprite.transform.localScale = new Vector3(108f, 107f, 0);  
            }
        }

    }

    public bool isEnabled
    {
        get
        {
            Collider col = GetComponent<Collider>();
            return col && col.enabled;
        }
        set
        {
            Collider col = GetComponent<Collider>();
            if (!col)
                return;

            if (col.enabled != value)
            {
                col.enabled = value;
                //UpdateColor(value, false);  
            }
        }
    }
}