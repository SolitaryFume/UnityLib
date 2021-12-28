using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if Guide
public partial class UIWidget//Guide
{
    public void SetGuideTag(int key, string tag)
    {
        if (TryFindComponent(key, out GuideTag guideTag))
        {
            guideTag.Tag = tag;
        }
    }

    public void RegisterTag(int key)
    {
        if (TryFindComponent(key, out GuideTag guideTag))
        {
            guideTag.RegisterTag();
        }
    }

    public void UnRegisterTag(int key)
    {
        if (TryFindComponent(key, out GuideTag guideTag))
        {
            guideTag.UnRegisterTag();
        }
    }
}
#endif