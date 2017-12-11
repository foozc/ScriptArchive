using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NGUIAtlasMaker : MonoBehaviour {

    public static bool atlasTrimming = true;
    public static bool atlasPMA = false;
    public static bool unityPacking = false;
    public static int atlasPadding = 1;
    public static bool allow4096 = true;

    public class SpriteEntry : UISpriteData
    {
        // Sprite texture -- original texture or a temporary texture
        public Texture2D tex;

        // Whether the texture is temporary and should be deleted
        public bool temporaryTexture = false;
    }

    /// <summary>
    /// Used to sort the sprites by pixels used
    /// </summary>

    static int Compare(SpriteEntry a, SpriteEntry b)
    {
        // A is null b is not b is greater so put it at the front of the list
        if (a == null && b != null) return 1;

        // A is not null b is null a is greater so put it at the front of the list
        if (a != null && b == null) return -1;

        // Get the total pixels used for each sprite
        int aPixels = a.width * a.height;
        int bPixels = b.width * b.height;

        if (aPixels > bPixels) return -1;
        else if (aPixels < bPixels) return 1;
        return 0;
    }

    /// <summary>
    /// Pack all of the specified sprites into a single texture, updating the outer and inner rects of the sprites as needed.
    /// </summary>

    static bool PackTextures(Texture2D tex, List<SpriteEntry> sprites)
    {
        Texture2D[] textures = new Texture2D[sprites.Count];
        Rect[] rects;

#if UNITY_3_5 || UNITY_4_0
    int maxSize = 4096;
#else
        int maxSize = SystemInfo.maxTextureSize;
#endif

#if UNITY_ANDROID || UNITY_IPHONE
    maxSize = Mathf.Min(maxSize, allow4096 ? 4096 : 2048);
#endif
        if (unityPacking)
        {
            for (int i = 0; i < sprites.Count; ++i) textures[i] = sprites[i].tex;
            rects = tex.PackTextures(textures, atlasPadding, maxSize);
        }
        else
        {
            sprites.Sort(Compare);
            for (int i = 0; i < sprites.Count; ++i) textures[i] = sprites[i].tex;
            rects = UITexturePacker.PackTextures(tex, textures, 4, 4, atlasPadding, maxSize);
        }

        for (int i = 0; i < sprites.Count; ++i)
        {
            Rect rect = NGUIMath.ConvertToPixels(rects[i], tex.width, tex.height, true);

            // Make sure that we don't shrink the textures
            if (Mathf.RoundToInt(rect.width) != textures[i].width) return false;

            SpriteEntry se = sprites[i];
            se.x = Mathf.RoundToInt(rect.x);
            se.y = Mathf.RoundToInt(rect.y);
            se.width = Mathf.RoundToInt(rect.width);
            se.height = Mathf.RoundToInt(rect.height);
        }
        return true;
    }

    static public void AddOrUpdate(UIAtlas atlas, Texture2D tex)
    {
        if (atlas != null && tex != null)
        {
            List<Texture> textures = new List<Texture>();
            textures.Add(tex);
            List<SpriteEntry> sprites = CreateSprites(textures);
            ExtractSprites(atlas, sprites);
            UpdateAtlas(atlas, sprites);
        }
    }

    /// <summary>
    /// Update the sprite atlas, keeping only the sprites that are on the specified list.
    /// </summary>

    static public void UpdateAtlas(UIAtlas atlas, List<SpriteEntry> sprites)
    {
        if (sprites.Count > 0)
        {
            // Combine all sprites into a single texture and save it
            if (UpdateTexture(atlas, sprites))
            {
                // Replace the sprites within the atlas
                ReplaceSprites(atlas, sprites);
            }

            // Release the temporary textures
            ReleaseSprites(sprites);
            return;
        }
        else
        {
            atlas.spriteList.Clear();
            NGUITools.Destroy(atlas.spriteMaterial.mainTexture);
            atlas.spriteMaterial.mainTexture = null;
        }

        atlas.MarkAsChanged();
    }

    /// <summary>
    /// Add a new sprite to the atlas, given the texture it's coming from and the packed rect within the atlas.
    /// </summary>

    static public UISpriteData AddSprite(List<UISpriteData> sprites, SpriteEntry se)
    {
        // See if this sprite already exists
        foreach (UISpriteData sp in sprites)
        {
            if (sp.name == se.name)
            {
                sp.CopyFrom(se);
                return sp;
            }
        }

        UISpriteData sprite = new UISpriteData();
        sprite.CopyFrom(se);
        sprites.Add(sprite);
        return sprite;
    }

    /// <summary>
    /// Create a list of sprites using the specified list of textures.
    /// </summary>
    /// 
    static public List<SpriteEntry> CreateSprites(List<Texture> textures)
    {
        List<SpriteEntry> list = new List<SpriteEntry>();

        foreach (Texture tex in textures)
        {
            Texture2D oldTex = tex as Texture2D;

            // If we aren't doing trimming, just use the texture as-is
            if (!atlasTrimming && !atlasPMA)
            {
                SpriteEntry sprite = new SpriteEntry();
                sprite.SetRect(0, 0, oldTex.width, oldTex.height);
                sprite.tex = oldTex;
                sprite.name = oldTex.name;
                sprite.temporaryTexture = false;
                list.Add(sprite);
                continue;
            }

            // If we want to trim transparent pixels, there is more work to be done
            Color32[] pixels = oldTex.GetPixels32();

            int xmin = oldTex.width;
            int xmax = 0;
            int ymin = oldTex.height;
            int ymax = 0;
            int oldWidth = oldTex.width;
            int oldHeight = oldTex.height;

            // Find solid pixels
            if (atlasTrimming)
            {
                for (int y = 0, yw = oldHeight; y < yw; ++y)
                {
                    for (int x = 0, xw = oldWidth; x < xw; ++x)
                    {
                        Color32 c = pixels[y * xw + x];

                        if (c.a != 0)
                        {
                            if (y < ymin) ymin = y;
                            if (y > ymax) ymax = y;
                            if (x < xmin) xmin = x;
                            if (x > xmax) xmax = x;
                        }
                    }
                }
            }
            else
            {
                xmin = 0;
                xmax = oldWidth - 1;
                ymin = 0;
                ymax = oldHeight - 1;
            }

            int newWidth = (xmax - xmin) + 1;
            int newHeight = (ymax - ymin) + 1;

            if (newWidth > 0 && newHeight > 0)
            {
                SpriteEntry sprite = new SpriteEntry();
                sprite.x = 0;
                sprite.y = 0;
                sprite.width = oldTex.width;
                sprite.height = oldTex.height;

                // If the dimensions match, then nothing was actually trimmed
                if (!atlasPMA && (newWidth == oldWidth && newHeight == oldHeight))
                {
                    sprite.tex = oldTex;
                    sprite.name = oldTex.name;
                    sprite.temporaryTexture = false;
                }
                else
                {
                    // Copy the non-trimmed texture data into a temporary buffer
                    Color32[] newPixels = new Color32[newWidth * newHeight];

                    for (int y = 0; y < newHeight; ++y)
                    {
                        for (int x = 0; x < newWidth; ++x)
                        {
                            int newIndex = y * newWidth + x;
                            int oldIndex = (ymin + y) * oldWidth + (xmin + x);
                            if (atlasPMA) newPixels[newIndex] = NGUITools.ApplyPMA(pixels[oldIndex]);
                            else newPixels[newIndex] = pixels[oldIndex];
                        }
                    }

                    // Create a new texture
                    sprite.temporaryTexture = true;
                    sprite.name = oldTex.name;
                    sprite.tex = new Texture2D(newWidth, newHeight);
                    sprite.tex.SetPixels32(newPixels);
                    sprite.tex.Apply();

                    // Remember the padding offset
                    sprite.SetPadding(xmin, ymin, oldWidth - newWidth - xmin, oldHeight - newHeight - ymin);
                }
                list.Add(sprite);
            }
        }
        return list;
    }

    /// <summary>
    /// Release all temporary textures created for the sprites.
    /// </summary>

    static public void ReleaseSprites(List<SpriteEntry> sprites)
    {
        foreach (SpriteEntry se in sprites)
        {
            if (se.temporaryTexture)
            {
                NGUITools.Destroy(se.tex);
                se.tex = null;
            }
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Replace the sprites within the atlas.
    /// </summary>

    static public void ReplaceSprites(UIAtlas atlas, List<SpriteEntry> sprites)
    {
        // Get the list of sprites we'll be updating
        List<UISpriteData> spriteList = atlas.spriteList;
        List<UISpriteData> kept = new List<UISpriteData>();

        // Run through all the textures we added and add them as sprites to the atlas
        for (int i = 0; i < sprites.Count; ++i)
        {
            SpriteEntry se = sprites[i];
            UISpriteData sprite = AddSprite(spriteList, se);
            kept.Add(sprite);
        }

        // Remove unused sprites
        for (int i = spriteList.Count; i > 0; )
        {
            UISpriteData sp = spriteList[--i];
            if (!kept.Contains(sp)) spriteList.RemoveAt(i);
        }

        // Sort the sprites so that they are alphabetical within the atlas
        atlas.SortAlphabetically();
        atlas.MarkAsChanged();
    }

    /// <summary>
    /// Extract the specified sprite from the atlas.
    /// </summary>
    /// 
    static public SpriteEntry ExtractSprite(UIAtlas atlas, string spriteName)
    {
        if (atlas.texture == null) return null;
        UISpriteData sd = atlas.GetSprite(spriteName);
        if (sd == null) return null;

        Texture2D tex = atlas.texture as Texture2D;
        SpriteEntry se = ExtractSprite(sd, tex);
        return se;
    }

    /// <summary>
    /// Extract the specified sprite from the atlas texture.
    /// </summary>

    static SpriteEntry ExtractSprite(UISpriteData es, Texture2D tex)
    {
        return (tex != null) ? ExtractSprite(es, tex.GetPixels32(), tex.width, tex.height) : null;
    }

    /// <summary>
    /// Extract the specified sprite from the atlas texture.
    /// </summary>

    static SpriteEntry ExtractSprite(UISpriteData es, Color32[] oldPixels, int oldWidth, int oldHeight)
    {
        int xmin = Mathf.Clamp(es.x, 0, oldWidth);
        int ymin = Mathf.Clamp(es.y, 0, oldHeight);
        int xmax = Mathf.Min(xmin + es.width, oldWidth - 1);
        int ymax = Mathf.Min(ymin + es.height, oldHeight - 1);
        int newWidth = Mathf.Clamp(es.width, 0, oldWidth);
        int newHeight = Mathf.Clamp(es.height, 0, oldHeight);

        if (newWidth == 0 || newHeight == 0) return null;

        Color32[] newPixels = new Color32[newWidth * newHeight];

        for (int y = 0; y < newHeight; ++y)
        {
            int cy = ymin + y;
            if (cy > ymax) cy = ymax;

            for (int x = 0; x < newWidth; ++x)
            {
                int cx = xmin + x;
                if (cx > xmax) cx = xmax;

                int newIndex = (newHeight - 1 - y) * newWidth + x;
                int oldIndex = (oldHeight - 1 - cy) * oldWidth + cx;

                newPixels[newIndex] = oldPixels[oldIndex];
            }
        }

        // Create a new sprite
        SpriteEntry sprite = new SpriteEntry();
        sprite.CopyFrom(es);
        sprite.SetRect(0, 0, newWidth, newHeight);
        sprite.temporaryTexture = true;
        sprite.tex = new Texture2D(newWidth, newHeight);
        sprite.tex.SetPixels32(newPixels);
        sprite.tex.Apply();
        return sprite;
    }

    /// <summary>
    /// Extract sprites from the atlas, adding them to the list.
    /// </summary>

    static public void ExtractSprites(UIAtlas atlas, List<SpriteEntry> finalSprites)
    {
        Texture2D tex = atlas.texture as Texture2D;

        if (tex != null)
        {
            Color32[] pixels = null;
            int width = tex.width;
            int height = tex.height;
            List<UISpriteData> sprites = atlas.spriteList;
            float count = sprites.Count;
            int index = 0;

            foreach (UISpriteData es in sprites)
            {
                bool found = false;

                foreach (SpriteEntry fs in finalSprites)
                {
                    if (es.name == fs.name)
                    {
                        fs.CopyBorderFrom(es);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (pixels == null) pixels = tex.GetPixels32();
                    SpriteEntry sprite = ExtractSprite(es, pixels, width, height);
                    if (sprite != null) finalSprites.Add(sprite);
                }
            }
        }
    }

    static public bool UpdateTexture(UIAtlas atlas, List<SpriteEntry> sprites)
    {
        // Get the texture for the atlas
        Texture2D tex = atlas.texture as Texture2D;

        bool newTexture = tex == null;

        if (newTexture)
        {
            // Create a new texture for the atlas
            tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        }

        // Pack the sprites into this texture
        if (PackTextures(tex, sprites) && tex != null)
        {
            // Update the atlas texture
            if (newTexture)
            {
                if (atlas.spriteMaterial == null)
                {
                    Shader shader = Shader.Find(atlasPMA ? "Unlit/Premultiplied Colored" : "Unlit/Transparent Colored");
                    atlas.spriteMaterial = new Material(shader);
                }
                atlas.spriteMaterial.mainTexture = tex;
                ReleaseSprites(sprites);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // save as ngui's
    public class UITexturePacker
    {
        // sz modify
        public static bool forceSquareAtlas = true;
        public int binWidth = 0;
        public int binHeight = 0;
        public bool allowRotations;

        public List<Rect> usedRectangles = new List<Rect>();
        public List<Rect> freeRectangles = new List<Rect>();

        public enum FreeRectChoiceHeuristic
        {
            RectBestShortSideFit, //< -BSSF: Positions the rectangle against the short side of a free rectangle into which it fits the best.
            RectBestLongSideFit, //< -BLSF: Positions the rectangle against the long side of a free rectangle into which it fits the best.
            RectBestAreaFit, //< -BAF: Positions the rectangle into the smallest free rect into which it fits.
            RectBottomLeftRule, //< -BL: Does the Tetris placement.
            RectContactPointRule //< -CP: Choosest the placement where the rectangle touches other rects as much as possible.
        };

        public UITexturePacker(int width, int height, bool rotations)
        {
            Init(width, height, rotations);
        }

        public void Init(int width, int height, bool rotations)
        {
            binWidth = width;
            binHeight = height;
            allowRotations = rotations;

            Rect n = new Rect();
            n.x = 0;
            n.y = 0;
            n.width = width;
            n.height = height;

            usedRectangles.Clear();

            freeRectangles.Clear();
            freeRectangles.Add(n);
        }

        private struct Storage
        {
            public Rect rect;
            public bool paddingX;
            public bool paddingY;
        }

        public static Rect[] PackTextures(Texture2D texture, Texture2D[] textures, int width, int height, int padding, int maxSize)
        {
            if (width > maxSize && height > maxSize) return null;
            if (width > maxSize || height > maxSize) { int temp = width; width = height; height = temp; }

            // Force square by sizing up
            // sz modify
            //if (NGUISettings.forceSquareAtlas)
            if (forceSquareAtlas)
            {
                if (width > height)
                    height = width;
                else if (height > width)
                    width = height;
            }
            UITexturePacker bp = new UITexturePacker(width, height, false);
            Storage[] storage = new Storage[textures.Length];

            for (int i = 0; i < textures.Length; i++)
            {
                Texture2D tex = textures[i];
                if (!tex) continue;

                Rect rect = new Rect();

                int xPadding = 1;
                int yPadding = 1;

                for (xPadding = 1; xPadding >= 0; --xPadding)
                {
                    for (yPadding = 1; yPadding >= 0; --yPadding)
                    {
                        rect = bp.Insert(tex.width + (xPadding * padding), tex.height + (yPadding * padding),
                                         UITexturePacker.FreeRectChoiceHeuristic.RectBestAreaFit);
                        if (rect.width != 0 && rect.height != 0) break;

                        // After having no padding if it still doesn't fit -- increase texture size.
                        else if (xPadding == 0 && yPadding == 0)
                        {
                            return PackTextures(texture, textures, width * (width <= height ? 2 : 1),
                                                height * (height < width ? 2 : 1), padding, maxSize);
                        }
                    }
                    if (rect.width != 0 && rect.height != 0) break;
                }

                storage[i] = new Storage();
                storage[i].rect = rect;
                storage[i].paddingX = (xPadding != 0);
                storage[i].paddingY = (yPadding != 0);
            }

            texture.Resize(width, height);
            texture.SetPixels(new Color[width * height]);

            // The returned rects
            Rect[] rects = new Rect[textures.Length];

            for (int i = 0; i < textures.Length; i++)
            {
                Texture2D tex = textures[i];
                if (!tex) continue;

                Rect rect = storage[i].rect;
                int xPadding = (storage[i].paddingX ? padding : 0);
                int yPadding = (storage[i].paddingY ? padding : 0);
                Color[] colors = tex.GetPixels();

                // Would be used to rotate the texture if need be.
                if (rect.width != tex.width + xPadding)
                {
                    Color[] newColors = tex.GetPixels();

                    for (int x = 0; x < rect.width; x++)
                    {
                        for (int y = 0; y < rect.height; y++)
                        {
                            int prevIndex = ((int)rect.height - (y + 1)) + x * (int)tex.width;
                            newColors[x + y * (int)rect.width] = colors[prevIndex];
                        }
                    }

                    colors = newColors;
                }

                texture.SetPixels((int)rect.x, (int)rect.y, (int)rect.width - xPadding, (int)rect.height - yPadding, colors);
                rect.x /= width;
                rect.y /= height;
                rect.width = (rect.width - xPadding) / width;
                rect.height = (rect.height - yPadding) / height;
                rects[i] = rect;
            }
            texture.Apply();
            return rects;
        }

        public Rect Insert(int width, int height, FreeRectChoiceHeuristic method)
        {
            Rect newNode = new Rect();
            int score1 = 0; // Unused in this function. We don't need to know the score after finding the position.
            int score2 = 0;
            switch (method)
            {
                case FreeRectChoiceHeuristic.RectBestShortSideFit: newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2); break;
                case FreeRectChoiceHeuristic.RectBottomLeftRule: newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2); break;
                case FreeRectChoiceHeuristic.RectContactPointRule: newNode = FindPositionForNewNodeContactPoint(width, height, ref score1); break;
                case FreeRectChoiceHeuristic.RectBestLongSideFit: newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1); break;
                case FreeRectChoiceHeuristic.RectBestAreaFit: newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2); break;
            }

            if (newNode.height == 0)
                return newNode;

            int numRectanglesToProcess = freeRectangles.Count;
            for (int i = 0; i < numRectanglesToProcess; ++i)
            {
                if (SplitFreeNode(freeRectangles[i], ref newNode))
                {
                    freeRectangles.RemoveAt(i);
                    --i;
                    --numRectanglesToProcess;
                }
            }

            PruneFreeList();

            usedRectangles.Add(newNode);
            return newNode;
        }

        public void Insert(List<Rect> rects, List<Rect> dst, FreeRectChoiceHeuristic method)
        {
            dst.Clear();

            while (rects.Count > 0)
            {
                int bestScore1 = int.MaxValue;
                int bestScore2 = int.MaxValue;
                int bestRectIndex = -1;
                Rect bestNode = new Rect();

                for (int i = 0; i < rects.Count; ++i)
                {
                    int score1 = 0;
                    int score2 = 0;
                    Rect newNode = ScoreRect((int)rects[i].width, (int)rects[i].height, method, ref score1, ref score2);

                    if (score1 < bestScore1 || (score1 == bestScore1 && score2 < bestScore2))
                    {
                        bestScore1 = score1;
                        bestScore2 = score2;
                        bestNode = newNode;
                        bestRectIndex = i;
                    }
                }

                if (bestRectIndex == -1)
                    return;

                PlaceRect(bestNode);
                rects.RemoveAt(bestRectIndex);
            }
        }

        void PlaceRect(Rect node)
        {
            int numRectanglesToProcess = freeRectangles.Count;
            for (int i = 0; i < numRectanglesToProcess; ++i)
            {
                if (SplitFreeNode(freeRectangles[i], ref node))
                {
                    freeRectangles.RemoveAt(i);
                    --i;
                    --numRectanglesToProcess;
                }
            }

            PruneFreeList();

            usedRectangles.Add(node);
        }

        Rect ScoreRect(int width, int height, FreeRectChoiceHeuristic method, ref int score1, ref int score2)
        {
            Rect newNode = new Rect();
            score1 = int.MaxValue;
            score2 = int.MaxValue;
            switch (method)
            {
                case FreeRectChoiceHeuristic.RectBestShortSideFit: newNode = FindPositionForNewNodeBestShortSideFit(width, height, ref score1, ref score2); break;
                case FreeRectChoiceHeuristic.RectBottomLeftRule: newNode = FindPositionForNewNodeBottomLeft(width, height, ref score1, ref score2); break;
                case FreeRectChoiceHeuristic.RectContactPointRule: newNode = FindPositionForNewNodeContactPoint(width, height, ref score1);
                    score1 = -score1; // Reverse since we are minimizing, but for contact point score bigger is better.
                    break;
                case FreeRectChoiceHeuristic.RectBestLongSideFit: newNode = FindPositionForNewNodeBestLongSideFit(width, height, ref score2, ref score1); break;
                case FreeRectChoiceHeuristic.RectBestAreaFit: newNode = FindPositionForNewNodeBestAreaFit(width, height, ref score1, ref score2); break;
            }

            // Cannot fit the current rectangle.
            if (newNode.height == 0)
            {
                score1 = int.MaxValue;
                score2 = int.MaxValue;
            }

            return newNode;
        }

        /// Computes the ratio of used surface area.
        public float Occupancy()
        {
            ulong usedSurfaceArea = 0;
            for (int i = 0; i < usedRectangles.Count; ++i)
                usedSurfaceArea += (uint)usedRectangles[i].width * (uint)usedRectangles[i].height;

            return (float)usedSurfaceArea / (binWidth * binHeight);
        }

        Rect FindPositionForNewNodeBottomLeft(int width, int height, ref int bestY, ref int bestX)
        {
            Rect bestNode = new Rect();
            //memset(bestNode, 0, sizeof(Rect));

            bestY = int.MaxValue;

            for (int i = 0; i < freeRectangles.Count; ++i)
            {
                // Try to place the rectangle in upright (non-flipped) orientation.
                if (freeRectangles[i].width >= width && freeRectangles[i].height >= height)
                {
                    int topSideY = (int)freeRectangles[i].y + height;
                    if (topSideY < bestY || (topSideY == bestY && freeRectangles[i].x < bestX))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = width;
                        bestNode.height = height;
                        bestY = topSideY;
                        bestX = (int)freeRectangles[i].x;
                    }
                }
                if (allowRotations && freeRectangles[i].width >= height && freeRectangles[i].height >= width)
                {
                    int topSideY = (int)freeRectangles[i].y + width;
                    if (topSideY < bestY || (topSideY == bestY && freeRectangles[i].x < bestX))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = height;
                        bestNode.height = width;
                        bestY = topSideY;
                        bestX = (int)freeRectangles[i].x;
                    }
                }
            }
            return bestNode;
        }

        Rect FindPositionForNewNodeBestShortSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
        {
            Rect bestNode = new Rect();
            //memset(&bestNode, 0, sizeof(Rect));

            bestShortSideFit = int.MaxValue;

            for (int i = 0; i < freeRectangles.Count; ++i)
            {
                // Try to place the rectangle in upright (non-flipped) orientation.
                if (freeRectangles[i].width >= width && freeRectangles[i].height >= height)
                {
                    int leftoverHoriz = Mathf.Abs((int)freeRectangles[i].width - width);
                    int leftoverVert = Mathf.Abs((int)freeRectangles[i].height - height);
                    int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);
                    int longSideFit = Mathf.Max(leftoverHoriz, leftoverVert);

                    if (shortSideFit < bestShortSideFit || (shortSideFit == bestShortSideFit && longSideFit < bestLongSideFit))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = width;
                        bestNode.height = height;
                        bestShortSideFit = shortSideFit;
                        bestLongSideFit = longSideFit;
                    }
                }

                if (allowRotations && freeRectangles[i].width >= height && freeRectangles[i].height >= width)
                {
                    int flippedLeftoverHoriz = Mathf.Abs((int)freeRectangles[i].width - height);
                    int flippedLeftoverVert = Mathf.Abs((int)freeRectangles[i].height - width);
                    int flippedShortSideFit = Mathf.Min(flippedLeftoverHoriz, flippedLeftoverVert);
                    int flippedLongSideFit = Mathf.Max(flippedLeftoverHoriz, flippedLeftoverVert);

                    if (flippedShortSideFit < bestShortSideFit || (flippedShortSideFit == bestShortSideFit && flippedLongSideFit < bestLongSideFit))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = height;
                        bestNode.height = width;
                        bestShortSideFit = flippedShortSideFit;
                        bestLongSideFit = flippedLongSideFit;
                    }
                }
            }
            return bestNode;
        }

        Rect FindPositionForNewNodeBestLongSideFit(int width, int height, ref int bestShortSideFit, ref int bestLongSideFit)
        {
            Rect bestNode = new Rect();
            //memset(&bestNode, 0, sizeof(Rect));

            bestLongSideFit = int.MaxValue;

            for (int i = 0; i < freeRectangles.Count; ++i)
            {
                // Try to place the rectangle in upright (non-flipped) orientation.
                if (freeRectangles[i].width >= width && freeRectangles[i].height >= height)
                {
                    int leftoverHoriz = Mathf.Abs((int)freeRectangles[i].width - width);
                    int leftoverVert = Mathf.Abs((int)freeRectangles[i].height - height);
                    int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);
                    int longSideFit = Mathf.Max(leftoverHoriz, leftoverVert);

                    if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = width;
                        bestNode.height = height;
                        bestShortSideFit = shortSideFit;
                        bestLongSideFit = longSideFit;
                    }
                }

                if (allowRotations && freeRectangles[i].width >= height && freeRectangles[i].height >= width)
                {
                    int leftoverHoriz = Mathf.Abs((int)freeRectangles[i].width - height);
                    int leftoverVert = Mathf.Abs((int)freeRectangles[i].height - width);
                    int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);
                    int longSideFit = Mathf.Max(leftoverHoriz, leftoverVert);

                    if (longSideFit < bestLongSideFit || (longSideFit == bestLongSideFit && shortSideFit < bestShortSideFit))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = height;
                        bestNode.height = width;
                        bestShortSideFit = shortSideFit;
                        bestLongSideFit = longSideFit;
                    }
                }
            }
            return bestNode;
        }

        Rect FindPositionForNewNodeBestAreaFit(int width, int height, ref int bestAreaFit, ref int bestShortSideFit)
        {
            Rect bestNode = new Rect();
            //memset(&bestNode, 0, sizeof(Rect));

            bestAreaFit = int.MaxValue;

            for (int i = 0; i < freeRectangles.Count; ++i)
            {
                int areaFit = (int)freeRectangles[i].width * (int)freeRectangles[i].height - width * height;

                // Try to place the rectangle in upright (non-flipped) orientation.
                if (freeRectangles[i].width >= width && freeRectangles[i].height >= height)
                {
                    int leftoverHoriz = Mathf.Abs((int)freeRectangles[i].width - width);
                    int leftoverVert = Mathf.Abs((int)freeRectangles[i].height - height);
                    int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);

                    if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = width;
                        bestNode.height = height;
                        bestShortSideFit = shortSideFit;
                        bestAreaFit = areaFit;
                    }
                }

                if (allowRotations && freeRectangles[i].width >= height && freeRectangles[i].height >= width)
                {
                    int leftoverHoriz = Mathf.Abs((int)freeRectangles[i].width - height);
                    int leftoverVert = Mathf.Abs((int)freeRectangles[i].height - width);
                    int shortSideFit = Mathf.Min(leftoverHoriz, leftoverVert);

                    if (areaFit < bestAreaFit || (areaFit == bestAreaFit && shortSideFit < bestShortSideFit))
                    {
                        bestNode.x = freeRectangles[i].x;
                        bestNode.y = freeRectangles[i].y;
                        bestNode.width = height;
                        bestNode.height = width;
                        bestShortSideFit = shortSideFit;
                        bestAreaFit = areaFit;
                    }
                }
            }
            return bestNode;
        }

        /// Returns 0 if the two intervals i1 and i2 are disjoint, or the length of their overlap otherwise.
        int CommonIntervalLength(int i1start, int i1end, int i2start, int i2end)
        {
            if (i1end < i2start || i2end < i1start)
                return 0;
            return Mathf.Min(i1end, i2end) - Mathf.Max(i1start, i2start);
        }

        int ContactPointScoreNode(int x, int y, int width, int height)
        {
            int score = 0;

            if (x == 0 || x + width == binWidth)
                score += height;
            if (y == 0 || y + height == binHeight)
                score += width;

            for (int i = 0; i < usedRectangles.Count; ++i)
            {
                if (usedRectangles[i].x == x + width || usedRectangles[i].x + usedRectangles[i].width == x)
                    score += CommonIntervalLength((int)usedRectangles[i].y, (int)usedRectangles[i].y + (int)usedRectangles[i].height, y, y + height);
                if (usedRectangles[i].y == y + height || usedRectangles[i].y + usedRectangles[i].height == y)
                    score += CommonIntervalLength((int)usedRectangles[i].x, (int)usedRectangles[i].x + (int)usedRectangles[i].width, x, x + width);
            }
            return score;
        }

        Rect FindPositionForNewNodeContactPoint(int width, int height, ref int bestContactScore)
        {
            Rect bestNode = new Rect();
            //memset(&bestNode, 0, sizeof(Rect));

            bestContactScore = -1;

            for (int i = 0; i < freeRectangles.Count; ++i)
            {
                // Try to place the rectangle in upright (non-flipped) orientation.
                if (freeRectangles[i].width >= width && freeRectangles[i].height >= height)
                {
                    int score = ContactPointScoreNode((int)freeRectangles[i].x, (int)freeRectangles[i].y, width, height);
                    if (score > bestContactScore)
                    {
                        bestNode.x = (int)freeRectangles[i].x;
                        bestNode.y = (int)freeRectangles[i].y;
                        bestNode.width = width;
                        bestNode.height = height;
                        bestContactScore = score;
                    }
                }
                if (allowRotations && freeRectangles[i].width >= height && freeRectangles[i].height >= width)
                {
                    int score = ContactPointScoreNode((int)freeRectangles[i].x, (int)freeRectangles[i].y, height, width);
                    if (score > bestContactScore)
                    {
                        bestNode.x = (int)freeRectangles[i].x;
                        bestNode.y = (int)freeRectangles[i].y;
                        bestNode.width = height;
                        bestNode.height = width;
                        bestContactScore = score;
                    }
                }
            }
            return bestNode;
        }

        bool SplitFreeNode(Rect freeNode, ref Rect usedNode)
        {
            // Test with SAT if the rectangles even intersect.
            if (usedNode.x >= freeNode.x + freeNode.width || usedNode.x + usedNode.width <= freeNode.x ||
                usedNode.y >= freeNode.y + freeNode.height || usedNode.y + usedNode.height <= freeNode.y)
                return false;

            if (usedNode.x < freeNode.x + freeNode.width && usedNode.x + usedNode.width > freeNode.x)
            {
                // New node at the top side of the used node.
                if (usedNode.y > freeNode.y && usedNode.y < freeNode.y + freeNode.height)
                {
                    Rect newNode = freeNode;
                    newNode.height = usedNode.y - newNode.y;
                    freeRectangles.Add(newNode);
                }

                // New node at the bottom side of the used node.
                if (usedNode.y + usedNode.height < freeNode.y + freeNode.height)
                {
                    Rect newNode = freeNode;
                    newNode.y = usedNode.y + usedNode.height;
                    newNode.height = freeNode.y + freeNode.height - (usedNode.y + usedNode.height);
                    freeRectangles.Add(newNode);
                }
            }

            if (usedNode.y < freeNode.y + freeNode.height && usedNode.y + usedNode.height > freeNode.y)
            {
                // New node at the left side of the used node.
                if (usedNode.x > freeNode.x && usedNode.x < freeNode.x + freeNode.width)
                {
                    Rect newNode = freeNode;
                    newNode.width = usedNode.x - newNode.x;
                    freeRectangles.Add(newNode);
                }

                // New node at the right side of the used node.
                if (usedNode.x + usedNode.width < freeNode.x + freeNode.width)
                {
                    Rect newNode = freeNode;
                    newNode.x = usedNode.x + usedNode.width;
                    newNode.width = freeNode.x + freeNode.width - (usedNode.x + usedNode.width);
                    freeRectangles.Add(newNode);
                }
            }

            return true;
        }

        void PruneFreeList()
        {
            for (int i = 0; i < freeRectangles.Count; ++i)
                for (int j = i + 1; j < freeRectangles.Count; ++j)
                {
                    if (IsContainedIn(freeRectangles[i], freeRectangles[j]))
                    {
                        freeRectangles.RemoveAt(i);
                        --i;
                        break;
                    }
                    if (IsContainedIn(freeRectangles[j], freeRectangles[i]))
                    {
                        freeRectangles.RemoveAt(j);
                        --j;
                    }
                }
        }

        bool IsContainedIn(Rect a, Rect b)
        {
            return a.x >= b.x && a.y >= b.y
              && a.x + a.width <= b.x + b.width
                && a.y + a.height <= b.y + b.height;
        }
    }
}
