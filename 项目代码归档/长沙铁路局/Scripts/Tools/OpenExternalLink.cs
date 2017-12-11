using UnityEngine;
using System.Collections;

public class OpenExternalLink
{


    public enum LinkType
    {
        ppt = 1,
        excel = 2,
        word = 3,
        baofeng = 4,
        ie = 5,
    }
    public OpenExternalLink()
    {
    }

    public void OpenLinksByType(LinkType type, string link)
    {
        switch (type)
        {
            case LinkType.ppt:

                break;
            case LinkType.excel:

                break;
            case LinkType.word:

                break;
            case LinkType.baofeng:

                break;
            case LinkType.ie:
                if (link.IndexOf("http://") != 0 && link.IndexOf("https://") != 0)
                    link = "http://" + link;
                break;
        }
        Application.OpenURL(link);
    }
}
