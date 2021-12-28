using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DOTween
/// <summary>
/// TW¶¯»­
/// </summary>
public partial class UIWidget
{
    public void DOPlay(int k)
    {
        if (TryFindComponent<DOTweenAnimation>(k, out var dt))
        {
            dt.DOPlay();
        }
    }
    public void DORestart(int k)
    {
        if (TryFindComponent<DOTweenAnimation>(k, out var dt))
        {
            dt.DORestart();
        }
    }
    public void DORewind(int k)
    {
        if (TryFindComponent<DOTweenAnimation>(k, out var dt))
        {
            dt.DORewind();
        }
    }
}
#endif