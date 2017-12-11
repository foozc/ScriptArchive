using System;
using System.Collections.Generic;
using System.Text;
//using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class TextStyle
    {
        private static Dictionary<Color136, ColorValue> color136 = new Dictionary<Color136, ColorValue>();

        #region  QuickContent---给一段文字里设置多个超链接或只进行颜色等设置的方法
        public static string QuickContent(string content, Color136 bgTextColor, List<QuickText> contentProperty)
        {
            string newContent = content.Substring(0, contentProperty[0].BeginIndex);
            if (color136.Count == 0) AddColorValue();
            newContent = "[" + color136[bgTextColor].HEX + "]" + newContent + "[-]";            //设置颜色/
            for (int i = 0; i < contentProperty.Count; i++)
            {
                string childContent1 = "";             //由list分割出来的字符串/
                if (i + 1 < contentProperty.Count)     //排除掉当前字符串长度超过下一个字符串开头索引的情况/
                {
                    int childContent1Length = 0;
                    if (contentProperty[i].BeginIndex + contentProperty[i].Length >= contentProperty[i + 1].BeginIndex)
                    {
                        childContent1Length = contentProperty[i + 1].BeginIndex - contentProperty[i].BeginIndex;
                    }
                    else
                    {
                        childContent1Length = contentProperty[i].Length;
                    }
                    childContent1 = content.Substring(contentProperty[i].BeginIndex, childContent1Length);
                }
                else
                {
                    childContent1 = content.Substring(contentProperty[i].BeginIndex, contentProperty[i].Length);
                }
                if (!string.IsNullOrEmpty(contentProperty[i].Url))
                {
                    childContent1 = "[url=" + contentProperty[i].Url + "]" + childContent1 + "[/url][url=][/url]";        //设置超链接/
                }
                childContent1 = "[" + color136[contentProperty[i].Color].HEX + "]" + childContent1 + "[-]";   //设置颜色/
                if (contentProperty[i].IsBold) childContent1 = "[b]" + childContent1 + "[/b]";                //设置粗体/
                if (contentProperty[i].IsUnderline) childContent1 = "[u]" + childContent1 + "[/u]";           //设置下划线/
                if (contentProperty[i].IsItalic) childContent1 = "[i]" + childContent1 + "[/i]";              //设置斜体/

                string childContent2 = "";            //与被list分割出的字符串紧邻在后面的字符串/
                if (i + 1 < contentProperty.Count)
                {
                    int childContent2Length = 0;
                    if (contentProperty[i + 1].BeginIndex - (contentProperty[i].BeginIndex + contentProperty[i].Length) >= 0)      //排除掉字符串长度小于0的情况/
                    {
                        childContent2Length = contentProperty[i + 1].BeginIndex - (contentProperty[i].BeginIndex + contentProperty[i].Length);
                    }
                    childContent2 = content.Substring(contentProperty[i].BeginIndex + contentProperty[i].Length, childContent2Length);
                    childContent2 = "[" + color136[bgTextColor].HEX + "]" + childContent2 + "[-]";
                }
                else
                {
                    childContent2 = content.Substring(contentProperty[i].BeginIndex + contentProperty[i].Length);
                    childContent2 = "[" + color136[bgTextColor].HEX + "]" + childContent2 + "[-]";
                }

                newContent = newContent + childContent1 + childContent2;
            }
            return newContent;
        }
        #endregion

        #region   QuickContent---给一段文字设置超链接、颜色、粗体、下划线、斜体
        public static string QuickContent(string content, string urlPath, Color136 color, bool isBold = true, bool isUnderline = true, bool isItalic = false)
        {
            if (color136.Count == 0) AddColorValue();

            content = "[" + color136[color].HEX + "]" + content + "[-]";   //设置颜色/
            content = "[url=" + urlPath + "]" + content + "[/url][url=][/url]";        //设置超链接/
            if (isBold) content = "[b]" + content + "[/b]";                //设置粗体/
            if (isUnderline) content = "[u]" + content + "[/u]";           //设置下划线/
            if (isItalic) content = "[i]" + content + "[/i]";              //设置斜体/

            return content;
        }
        #endregion

        #region  QuickLinkText---快速设置超链接文本的方法，可以在一句话中给指定了索引和长度的文字添加超链接，并能设置颜色，粗体，斜体，下划线
        public static string QuickLinkText(string content, string urlPath, int beginIndex, int length, Color136 color, bool isBold = true, bool isUnderline = true, bool isItalic = false)
        {
            int contentLength = content.Length;

            if (beginIndex < 0)
            {
                beginIndex = 0;
            }
            else if (beginIndex > contentLength - 1)
            {
                beginIndex = contentLength - 1;
            }

            if (length < 0)
            {
                length = 0;
            }
            else if (length > contentLength - beginIndex)
            {
                length = contentLength - beginIndex;
            }

            string str1 = content.Substring(0, beginIndex);          //第一段字符串/
            string str2 = content.Substring(beginIndex, length);     //第二段字符串/
            string str3 = content.Substring(beginIndex + length);    //第三段字符串/

            //将截取出来的第二段字符串进行添加超链接、设置颜色、设置粗体等操作/
            if (color136.Count == 0) AddColorValue();
            str2 = "[" + color136[color].HEX + "]" + str2 + "[-]";       //设置颜色/
            str2 = "[url=" + urlPath + "]" + str2 + "[/url][url=][/url]";        //设置超链接/
            if (isBold) str2 = "[b]" + str2 + "[/b]";                //设置粗体/
            if (isUnderline) str2 = "[u]" + str2 + "[/u]";           //设置下划线/
            if (isItalic) str2 = "[i]" + str2 + "[/i]";              //设置斜体/

            return str1 + str2 + str3;
        }

        public static string QuickLinkText(string content, string urlPath, Color136 color, bool isBold = true, bool isUnderline = true, bool isItalic = false)
        {
            if (color136.Count == 0) AddColorValue();
            content = "[" + color136[color].HEX + "]" + content + "[-]";       //设置颜色/
            content = "[url=" + urlPath + "]" + content + "[/url][url=][/url]";        //设置超链接/
            if (isBold) content = "[b]" + content + "[/b]";                //设置粗体/
            if (isUnderline) content = "[u]" + content + "[/u]";           //设置下划线/
            if (isItalic) content = "[i]" + content + "[/i]";              //设置斜体/

            return content;
        }
        #endregion

        #region  QuickColorText---快速设置文本颜色的方法，可以在一句话中给指定了索引和长度的文字设置颜色，粗体，斜体，下划线
        public static string QuickColorText(string content, int beginIndex, int length, Color136 mainColor, Color136 otherColor, bool isBold = true, bool isUnderline = false, bool isItalic = false, bool isAllSame = false)
        {
            int contentLength = content.Length;

            if (beginIndex < 0)
            {
                beginIndex = 0;
            }
            else if (beginIndex > contentLength - 1)
            {
                beginIndex = contentLength - 1;
            }

            if (length < 0)
            {
                length = 0;
            }
            else if (length > contentLength - beginIndex)
            {
                length = contentLength - beginIndex;
            }

            string str1 = content.Substring(0, beginIndex);          //第一段字符串/
            string str2 = content.Substring(beginIndex, length);     //第二段字符串/
            string str3 = content.Substring(beginIndex + length);    //第三段字符串/

            //将截取出来的第二段字符串进行设置颜色、设置粗体等操作/
            if (color136.Count == 0) AddColorValue();
            if (!string.IsNullOrEmpty(str1)) str1 = "[" + color136[otherColor].HEX + "]" + str1 + "[-]";  //设置颜色/
            str2 = "[" + color136[mainColor].HEX + "]" + str2 + "[-]";   //设置颜色/
            str3 = "[" + color136[otherColor].HEX + "]" + str3 + "[-]";  //设置颜色/

            string str = "";

            if (isAllSame)
            {
                str = str1 + str2 + str3;
                if (isBold)                                           //设置粗体/
                {
                    str = "[b]" + str + "[/b]";
                }
                if (isUnderline)                                      //设置下划线/
                {
                    str = "[u]" + str + "[/u]";
                }
                if (isItalic)                                         //设置斜体/
                {
                    str = "[i]" + str + "[/i]";
                }
            }
            else
            {
                if (isBold) str2 = "[b]" + str2 + "[/b]";              //设置粗体/
                if (isUnderline) str2 = "[u]" + str2 + "[/u]";         //设置下划线/
                if (isItalic) str2 = "[i]" + str2 + "[/i]";            //设置斜体/
                str = str1 + str2 + str3;
            }

            return str;
        }

        public static string QuickColorText(string content, Color136 mainColor, bool isBold = true, bool isUnderline = false, bool isItalic = false)
        {
            if (color136.Count == 0) AddColorValue();
            content = "[" + color136[mainColor].HEX + "]" + content + "[-]";   //设置颜色/
            if (isBold) content = "[b]" + content + "[/b]";                //设置粗体/
            if (isUnderline) content = "[u]" + content + "[/u]";           //设置下划线/
            if (isItalic) content = "[i]" + content + "[/i]";              //设置斜体/

            return content;
        }
        #endregion

        #region  getColor136---获取136种颜色的字典
        public static  Dictionary<Color136, ColorValue> getColor136()
        {
            if (color136.Count == 0) AddColorValue();
            return color136;
        }
        #endregion

        #region   Color---设置文字颜色
        public static string Color(string content, Color136 color)     //设置文字颜色,貌似只有在uilabel中文字颜色默认不设置情况下代码才起作用/
        {
            if (color136.Count == 0) AddColorValue();
            return "[" + color136[color].HEX + "]" + content + "[-]";
        }
        #endregion

        #region   Hyperlinks---超链接
        public static string Hyperlinks(string content, string urlAddress) //超链接/
        {
            return "[url=" + urlAddress + "]" + content + "[/url][url=][/url]";
        }
        #endregion

        #region   Bold---粗体
        public static string Bold(string content)          //粗体/
        {
            return "[b]" + content + "[/b]";
        }
        #endregion

        #region   Italic---斜体
        public static string Italic(string content)        //斜体/
        {
            return "[i]" + content + "[/i]";
        }
        #endregion

        #region   Underline---下划线
        public static string Underline(string content)     //下划线/
        {
            return "[u]" + content + "[/u]";
        }
        #endregion

        #region   Strikethrough---直线穿过文字
        public static string Strikethrough(string content) //直线穿过文字/
        {
            return "[s]" + content + "[/s]";
        }
        #endregion

        #region   Sub---下标
        public static string Sub(string content)           //下标，文字显示在正常文字下标处/
        {
            return "[sub]" + content + "[/sub]";
        }
        #endregion

        #region   Sup---上标
        public static string Sup(string content)           //上标，文字显示在正常文字上标处/
        {
            return "[sup]" + content + "[/sup]";
        }
        #endregion

        #region  AddColorValue()---将hex颜色值添加到字典中
        private static void AddColorValue()
        {
            color136.Add(Color136.LightPink, new ColorValue("FFB6C1", new RGB(255, 182, 193)));                 //浅粉红/
            color136.Add(Color136.Pink, new ColorValue("FFC0CB", new RGB(255, 192, 203)));                      //粉红/
            color136.Add(Color136.Crimson, new ColorValue("DC143C", new RGB(220, 20, 60)));                     //猩红/
            color136.Add(Color136.LavenderBlush, new ColorValue("FFF0F5", new RGB(255, 240, 245)));             //脸红的淡紫色/
            color136.Add(Color136.PaleVioletRed, new ColorValue("DB7093", new RGB(219, 112, 147)));             //苍白的紫罗兰红色/
            color136.Add(Color136.HotPink, new ColorValue("FF69B4", new RGB(255, 105, 180)));                   //热情的粉红/
            color136.Add(Color136.DeepPink, new ColorValue("FF1493", new RGB(255, 20, 147)));                   //深粉色/
            color136.Add(Color136.MediumVioletRed, new ColorValue("C71585", new RGB(199, 21, 133)));            //适中的紫罗兰红色/
            color136.Add(Color136.Orchid, new ColorValue("DA70D6", new RGB(218, 112, 214)));                    //兰花的紫色/
            color136.Add(Color136.Thistle, new ColorValue("D8BFD8", new RGB(216, 191, 216)));                   //蓟/
            color136.Add(Color136.plum, new ColorValue("DDA0DD", new RGB(221, 160, 221)));                      //李子/
            color136.Add(Color136.Violet, new ColorValue("EE82EE", new RGB(238, 130, 238)));                    //紫罗兰/
            color136.Add(Color136.Magenta, new ColorValue("FF00FF", new RGB(255, 0, 255)));                     //洋红/
            color136.Add(Color136.Fuchsia, new ColorValue("FF00FF", new RGB(255, 0, 255)));                     //灯笼海棠(紫红色)/
            color136.Add(Color136.DarkMagenta, new ColorValue("8B008B", new RGB(139, 0, 139)));                 //深洋红色/
            color136.Add(Color136.Purple, new ColorValue("800080", new RGB(128, 0, 128)));                      //紫色/
            color136.Add(Color136.MediumOrchid, new ColorValue("BA55D3", new RGB(186, 85, 211)));               //适中的兰花紫/
            color136.Add(Color136.DarkVoilet, new ColorValue("9400D3", new RGB(148, 0, 211)));                  //深紫罗兰色/
            color136.Add(Color136.DarkOrchid, new ColorValue("9932CC", new RGB(153, 50, 204)));                 //深兰花紫/
            color136.Add(Color136.Indigo, new ColorValue("4B0082", new RGB(75, 0, 130)));                       //靛青/
            color136.Add(Color136.BlueViolet, new ColorValue("8A2BE2", new RGB(138, 43, 226)));                 //深紫罗兰的蓝色/
            color136.Add(Color136.MediumPurple, new ColorValue("9370DB", new RGB(147, 112, 219)));              //适中的紫色/
            color136.Add(Color136.MediumSlateBlue, new ColorValue("7B68EE", new RGB(123, 104, 238)));           //适中的板岩暗蓝灰色/
            color136.Add(Color136.SlateBlue, new ColorValue("6A5ACD", new RGB(106, 90, 205)));                  //板岩暗蓝灰色/
            color136.Add(Color136.DarkSlateBlue, new ColorValue("483D8B", new RGB(72, 61, 139)));               //深岩暗蓝灰色/
            color136.Add(Color136.Lavender, new ColorValue("E6E6FA", new RGB(230, 230, 250)));                  //熏衣草花的淡紫色/
            color136.Add(Color136.GhostWhite, new ColorValue("F8F8FF", new RGB(248, 248, 255)));                //幽灵的白色/
            color136.Add(Color136.Blue, new ColorValue("0000FF", new RGB(0, 0, 255)));                          //纯蓝/
            color136.Add(Color136.MediumBlue, new ColorValue("0000CD", new RGB(0, 0, 205)));                    //适中的蓝色/
            color136.Add(Color136.MidnightBlue, new ColorValue("191970", new RGB(25, 25, 112)));                //午夜的蓝色/
            color136.Add(Color136.DarkBlue, new ColorValue("00008B", new RGB(0, 0, 139)));                      //深蓝色/
            color136.Add(Color136.Navy, new ColorValue("000080", new RGB(0, 0, 128)));                          //海军蓝/
            color136.Add(Color136.RoyalBlue, new ColorValue("4169E1", new RGB(65, 105, 225)));                  //皇军蓝/
            color136.Add(Color136.CornflowerBlue, new ColorValue("6495ED", new RGB(100, 149, 237)));            //矢车菊的蓝色/
            color136.Add(Color136.LightSteelBlue, new ColorValue("B0C4DE", new RGB(176, 196, 222)));            //淡钢蓝/
            color136.Add(Color136.LightSlateGray, new ColorValue("778899", new RGB(119, 136, 153)));            //浅石板灰/
            color136.Add(Color136.SlateGray, new ColorValue("708090", new RGB(112, 128, 144)));                 //石板灰/
            color136.Add(Color136.DoderBlue, new ColorValue("1E90FF", new RGB(30, 144, 255)));                  //道奇蓝/
            color136.Add(Color136.AliceBlue, new ColorValue("F0F8FF", new RGB(240, 248, 255)));                 //爱丽丝蓝/
            color136.Add(Color136.SteelBlue, new ColorValue("4682B4", new RGB(70, 130, 180)));                  //钢蓝/
            color136.Add(Color136.LightSkyBlue, new ColorValue("87CEFA", new RGB(135, 206, 250)));              //淡蓝色/
            color136.Add(Color136.SkyBlue, new ColorValue("87CEEB", new RGB(135, 206, 235)));                   //天蓝色/
            color136.Add(Color136.DeepSkyBlue, new ColorValue("00BFFF", new RGB(0, 191, 255)));                 //深天蓝/
            color136.Add(Color136.LightBlue, new ColorValue("ADD8E6", new RGB(173, 216, 230)));                 //淡蓝/
            color136.Add(Color136.PowDerBlue, new ColorValue("B0E0E6", new RGB(176, 224, 230)));                //火药蓝/
            color136.Add(Color136.CadetBlue, new ColorValue("5F9EA0", new RGB(95, 158, 160)));                  //军校蓝/
            color136.Add(Color136.Azure, new ColorValue("F0FFFF", new RGB(240, 255, 255)));                     //蔚蓝色/
            color136.Add(Color136.LightCyan, new ColorValue("E1FFFF", new RGB(225, 255, 255)));                 //淡青色/
            color136.Add(Color136.PaleTurquoise, new ColorValue("AFEEEE", new RGB(175, 238, 238)));             //苍白的绿宝石/
            color136.Add(Color136.Cyan, new ColorValue("00FFFF", new RGB(0, 255, 255)));                        //青色/
            color136.Add(Color136.Aqua, new ColorValue("00FFFF", new RGB(0, 255, 255)));                        //水绿色/
            color136.Add(Color136.DarkTurquoise, new ColorValue("00CED1", new RGB(0, 206, 209)));               //深绿宝石/
            color136.Add(Color136.DarkSlateGray, new ColorValue("2F4F4F", new RGB(47, 79, 79)));                //深石板灰/
            color136.Add(Color136.DarkCyan, new ColorValue("008B8B", new RGB(0, 139, 139)));                    //深青色/
            color136.Add(Color136.Teal, new ColorValue("008080", new RGB(0, 128, 128)));                        //水鸭色/
            color136.Add(Color136.MediumTurquoise, new ColorValue("48D1CC", new RGB(72, 209, 204)));            //适中的绿宝石/
            color136.Add(Color136.LightSeaGreen, new ColorValue("20B2AA", new RGB(32, 178, 170)));              //浅海洋绿/
            color136.Add(Color136.Turquoise, new ColorValue("40E0D0", new RGB(64, 224, 208)));                  //绿宝石/
            color136.Add(Color136.Auqamarin, new ColorValue("7FFFAA", new RGB(127, 255, 170)));                 //绿玉\碧绿色/
            color136.Add(Color136.MediumAquamarine, new ColorValue("00FA9A", new RGB(0, 250, 154)));            //适中的碧绿色/
            color136.Add(Color136.MediumSpringGreen, new ColorValue("F5FFFA", new RGB(245, 255, 250)));         //适中的春天的绿色/
            color136.Add(Color136.MintCream, new ColorValue("00FF7F", new RGB(0, 255, 127)));                   //薄荷奶油/
            color136.Add(Color136.SpringGreen, new ColorValue("3CB371", new RGB(60, 179, 113)));                //春天的绿色/
            color136.Add(Color136.SeaGreen, new ColorValue("2E8B57", new RGB(46, 139, 87)));                    //海洋绿/
            color136.Add(Color136.Honeydew, new ColorValue("F0FFF0", new RGB(240, 255, 240)));                  //蜂蜜/
            color136.Add(Color136.LightGreen, new ColorValue("90EE90", new RGB(144, 238, 144)));                //淡绿色/
            color136.Add(Color136.PaleGreen, new ColorValue("98FB98", new RGB(152, 251, 152)));                 //苍白的绿色/
            color136.Add(Color136.DarkSeaGreen, new ColorValue("8FBC8F", new RGB(143, 188, 143)));              //深海洋绿/
            color136.Add(Color136.LimeGreen, new ColorValue("32CD32", new RGB(50, 205, 50)));                   //酸橙绿/
            color136.Add(Color136.Lime, new ColorValue("00FF00", new RGB(0, 255, 0)));                          //酸橙色/
            color136.Add(Color136.ForestGreen, new ColorValue("228B22", new RGB(34, 139, 34)));                 //森林绿/
            color136.Add(Color136.Green, new ColorValue("008000", new RGB(0, 128, 0)));                         //纯绿/
            color136.Add(Color136.DarkGreen, new ColorValue("006400", new RGB(0, 100, 0)));                     //深绿色/
            color136.Add(Color136.Chartreuse, new ColorValue("7FFF00", new RGB(127, 255, 0)));                  //查特酒绿/
            color136.Add(Color136.LawnGreen, new ColorValue("7CFC00", new RGB(124, 252, 0)));                   //草坪绿/
            color136.Add(Color136.GreenYellow, new ColorValue("ADFF2F", new RGB(173, 255, 47)));                //绿黄色/
            color136.Add(Color136.OliveDrab, new ColorValue("556B2F", new RGB(85, 107, 47)));                   //橄榄土褐色/
            color136.Add(Color136.Beige, new ColorValue("6B8E23", new RGB(107, 142, 35)));                      //米色(浅褐色)/
            color136.Add(Color136.LightGoldenrodYellow, new ColorValue("FAFAD2", new RGB(250, 250, 210)));      //浅秋麒麟黄/
            color136.Add(Color136.Ivory, new ColorValue("FFFFF0", new RGB(255, 255, 240)));                     //象牙/
            color136.Add(Color136.LightYellow, new ColorValue("FFFFE0", new RGB(255, 255, 224)));               //浅黄色/
            color136.Add(Color136.Yellow, new ColorValue("FFFF00", new RGB(255, 255, 0)));                      //纯黄/
            color136.Add(Color136.Olive, new ColorValue("808000", new RGB(128, 128, 0)));                       //橄榄/
            color136.Add(Color136.DarkKhaki, new ColorValue("BDB76B", new RGB(189, 183, 107)));                 //深卡其布/
            color136.Add(Color136.LemonChiffon, new ColorValue("FFFACD", new RGB(255, 250, 205)));              //柠檬薄纱/
            color136.Add(Color136.PaleGodenrod, new ColorValue("EEE8AA", new RGB(238, 232, 170)));              //灰秋麒麟/
            color136.Add(Color136.Khaki, new ColorValue("F0E68C", new RGB(240, 230, 140)));                     //卡其布/
            color136.Add(Color136.Gold, new ColorValue("FFD700", new RGB(255, 215, 0)));                        //金/
            color136.Add(Color136.Cornislk, new ColorValue("FFF8DC", new RGB(255, 248, 220)));                  //玉米色/
            color136.Add(Color136.GoldEnrod, new ColorValue("DAA520", new RGB(218, 165, 32)));                  //秋麒麟/
            color136.Add(Color136.FloralWhite, new ColorValue("FFFAF0", new RGB(255, 250, 240)));               //花的白色/
            color136.Add(Color136.OldLace, new ColorValue("FDF5E6", new RGB(253, 245, 230)));                   //老饰带/
            color136.Add(Color136.Wheat, new ColorValue("F5DEB3", new RGB(245, 222, 179)));                     //小麦色/
            color136.Add(Color136.Moccasin, new ColorValue("FFE4B5", new RGB(255, 228, 181)));                  //鹿皮鞋/
            color136.Add(Color136.Orange, new ColorValue("FFA500", new RGB(255, 165, 0)));                      //橙色/
            color136.Add(Color136.PapayaWhip, new ColorValue("FFEFD5", new RGB(255, 239, 213)));                //番木瓜/
            color136.Add(Color136.BlanchedAlmond, new ColorValue("FFEBCD", new RGB(255, 235, 205)));            //漂白的杏仁/
            color136.Add(Color136.NavajoWhite, new ColorValue("FFDEAD", new RGB(255, 222, 173)));               //Navajo白/
            color136.Add(Color136.AntiqueWhite, new ColorValue("FAEBD7", new RGB(250, 235, 215)));              //古代的白色/
            color136.Add(Color136.Tan, new ColorValue("D2B48C", new RGB(210, 180, 140)));                       //晒黑/
            color136.Add(Color136.BrulyWood, new ColorValue("DEB887", new RGB(222, 184, 135)));                 //结实的树/
            color136.Add(Color136.Bisque, new ColorValue("FFE4C4", new RGB(255, 228, 196)));                    //(浓汤)乳脂,番茄等/
            color136.Add(Color136.DarkOrange, new ColorValue("FF8C00", new RGB(255, 140, 0)));                  //深橙色/
            color136.Add(Color136.Linen, new ColorValue("FAF0E6", new RGB(250, 240, 230)));                     //亚麻布/
            color136.Add(Color136.Peru, new ColorValue("CD853F", new RGB(205, 133, 63)));                       //秘鲁/
            color136.Add(Color136.PeachPuff, new ColorValue("FFDAB9", new RGB(255, 218, 185)));                 //桃色/
            color136.Add(Color136.SandyBrown, new ColorValue("F4A460", new RGB(244, 164, 96)));                 //沙棕色/
            color136.Add(Color136.Chocolate, new ColorValue("D2691E", new RGB(210, 105, 30)));                  //巧克力/
            color136.Add(Color136.SaddleBrown, new ColorValue("8B4513", new RGB(139, 69, 19)));                 //马鞍棕色/
            color136.Add(Color136.SeaShell, new ColorValue("FFF5EE", new RGB(255, 245, 238)));                  //海贝壳/
            color136.Add(Color136.Sienna, new ColorValue("A0522D", new RGB(160, 82, 45)));                      //黄土赭色/
            color136.Add(Color136.LightSalmon, new ColorValue("FFA07A", new RGB(255, 160, 122)));               //浅鲜肉(鲑鱼)色/
            color136.Add(Color136.Coral, new ColorValue("FF7F50", new RGB(255, 127, 80)));                      //珊瑚/
            color136.Add(Color136.OrangeRed, new ColorValue("FF4500", new RGB(255, 69, 0)));                    //橙红色/
            color136.Add(Color136.DarkSalmon, new ColorValue("E9967A", new RGB(233, 150, 122)));                //深鲜肉(鲑鱼)色/
            color136.Add(Color136.Tomato, new ColorValue("FF6347", new RGB(255, 99, 71)));                      //番茄/
            color136.Add(Color136.MistyRose, new ColorValue("FFE4E1", new RGB(255, 228, 225)));                 //薄雾玫瑰/
            color136.Add(Color136.Salmon, new ColorValue("FA8072", new RGB(250, 128, 114)));                    //鲜肉(鲑鱼)色/
            color136.Add(Color136.Snow, new ColorValue("FFFAFA", new RGB(255, 250, 250)));                      //雪/
            color136.Add(Color136.LightCoral, new ColorValue("F08080", new RGB(240, 128, 128)));                //淡珊瑚色/
            color136.Add(Color136.RosyBrown, new ColorValue("BC8F8F", new RGB(188, 143, 143)));                 //玫瑰棕色/
            color136.Add(Color136.IndianRed, new ColorValue("CD5C5C", new RGB(205, 92, 92)));                   //印度红/
            color136.Add(Color136.Red, new ColorValue("FF0000", new RGB(255, 0, 0)));                           //纯红/
            color136.Add(Color136.Brown, new ColorValue("A52A2A", new RGB(165, 42, 42)));                       //棕色/
            color136.Add(Color136.FireBrick, new ColorValue("B22222", new RGB(178, 34, 34)));                   //耐火砖/
            color136.Add(Color136.DarkRed, new ColorValue("8B0000", new RGB(139, 0, 0)));                       //深红色/
            color136.Add(Color136.Maroon, new ColorValue("800000", new RGB(128, 0, 0)));                        //栗色/
            color136.Add(Color136.White, new ColorValue("FFFFFF", new RGB(255, 255, 255)));                     //纯白/
            color136.Add(Color136.WhiteSmoke, new ColorValue("F5F5F5", new RGB(245, 245, 245)));                //白烟/
            color136.Add(Color136.Gainsboro, new ColorValue("DCDCDC", new RGB(220, 220, 220)));                 //Gainsboro/
            color136.Add(Color136.LightGrey, new ColorValue("D3D3D3", new RGB(211, 211, 211)));                 //浅灰色/
            color136.Add(Color136.Silver, new ColorValue("C0C0C0", new RGB(192, 192, 192)));                    //银白色/
            color136.Add(Color136.DarkGray, new ColorValue("A9A9A9", new RGB(169, 169, 169)));                  //深灰色/
            color136.Add(Color136.Gray, new ColorValue("808080", new RGB(128, 128, 128)));                      //灰色/
            color136.Add(Color136.DimGray, new ColorValue("696969", new RGB(105, 105, 105)));                   //暗淡的灰色/
            color136.Add(Color136.Black, new ColorValue("000000", new RGB(0, 0, 0)));                           //纯黑/
        }

        #endregion

    }

    public class QuickText            //要设置的文本的属性类，里面的字段都与文本要设置为的某属性相关/
    {
        private string url = "";
        private int beginIndex = 0;
        private int length = 0;
        private Color136 color = Color136.Black;
        private bool isBold = false;
        private bool isUnderline = false;
        private bool isItalic = false;

        public QuickText(string _url, int _beginIndex, int _length, Color136 _color, bool _isBold, bool _isUnderline, bool _isItalic)
        {
            url = _url;
            beginIndex = _beginIndex;
            length = _length;
            color = _color;
            isBold = _isBold;
            isUnderline = _isUnderline;
            isItalic = _isItalic;
        }

        public string Url { get { return url; } }                   //url地址/
        public int BeginIndex { get { return beginIndex; } }        //要设置的文本开始的索引/
        public int Length { get { return length; } }                //要设置的文本的长度/
        public Color136 Color { get { return color; } }             //文本要设置成的颜色/
        public bool IsBold { get { return isBold; } }               //要设置的文本是否加粗/
        public bool IsUnderline { get { return isUnderline; } }     //要设置的文本是否加下划线/
        public bool IsItalic { get { return isItalic; } }           //要设置的文本是否使用斜体/
    }

    public class ColorValue           //色值,分Hex和RGB两种类型/
    {
        public ColorValue(string hex, RGB rgb)
        {
            HEX = hex;
            RGB = rgb;
        }
        public string HEX { private set; get; }
        public RGB RGB { private set; get; }
    }

    public class RGB                  //RGB色值类型/
    {
        public RGB(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
    }

    public enum Color136              //136种颜色的称呼/
    {
        LightPink, Pink, Crimson, LavenderBlush, PaleVioletRed, HotPink, DeepPink, MediumVioletRed, Orchid, Thistle, plum, Violet, Magenta, Fuchsia, DarkMagenta, Purple, MediumOrchid, DarkVoilet, DarkOrchid, Indigo, BlueViolet, MediumPurple, MediumSlateBlue, SlateBlue, DarkSlateBlue, Lavender, GhostWhite,
        Blue, MediumBlue, MidnightBlue, DarkBlue, Navy, RoyalBlue, CornflowerBlue, LightSteelBlue, LightSlateGray, SlateGray, DoderBlue, AliceBlue, SteelBlue, LightSkyBlue, SkyBlue, DeepSkyBlue, LightBlue, PowDerBlue, CadetBlue, Azure, LightCyan, PaleTurquoise, Cyan, Aqua, DarkTurquoise, DarkSlateGray, DarkCyan, Teal, MediumTurquoise, LightSeaGreen, Turquoise, Auqamarin, MediumAquamarine, MediumSpringGreen, MintCream,
        SpringGreen, SeaGreen, Honeydew, LightGreen, PaleGreen, DarkSeaGreen, LimeGreen, Lime, ForestGreen, Green, DarkGreen, Chartreuse, LawnGreen, GreenYellow, OliveDrab, Beige, LightGoldenrodYellow, Ivory,
        LightYellow, Yellow, Olive, DarkKhaki, LemonChiffon, PaleGodenrod, Khaki, Gold, Cornislk, GoldEnrod, FloralWhite, OldLace, Wheat, Moccasin, Orange, PapayaWhip, BlanchedAlmond, NavajoWhite, AntiqueWhite, Tan, BrulyWood, Bisque, DarkOrange, Linen,
        Peru, PeachPuff, SandyBrown, Chocolate, SaddleBrown, SeaShell, Sienna, LightSalmon, Coral, OrangeRed, DarkSalmon, Tomato, MistyRose, Salmon, Snow,
        LightCoral, RosyBrown, IndianRed, Red, Brown, FireBrick, DarkRed, Maroon, White, WhiteSmoke, Gainsboro, LightGrey, Silver, DarkGray, Gray, DimGray, Black
    }
}
