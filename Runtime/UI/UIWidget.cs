using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.Animations;

using System.Security.Cryptography;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public partial class UIWidget : MonoBehaviour
{
    [SerializeField] private Component[] array;

    public int Size => array.Length;

    public Component this[int index] => index < array.Length ? array[index] : default;

    public bool TryFindComponent<T>(int index, out T component) where T : Component
    {
        if (index < array.Length)
            component = array[index] as T;
        else
            component = null;

        if (component == null)
        {
            var msg = $"No Find Component > index = [{index}] , type = [{typeof(T).Name}]";
            //Debug.LogException(new XLua.LuaException(msg), this);
            //throw new XLua.LuaException(msg);
            return false;
        }
        else
        {
            return true;
        }
    }


    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {

    }

    protected virtual void OnDisplay()
    {

    }

    protected virtual void OnHidle()
    {

    }

    protected virtual void OnEnable()
    {

    }


    protected virtual void OnDisable()
    {

    }

    protected virtual void OnDestroy()
    {
        foreach (var component in array) //lua 注册事件清理
        {
            switch (component)
            {
                case Button but:
                    but.onClick.RemoveAllListeners();
                    break;
                case Toggle tog:
                    tog.onValueChanged.RemoveAllListeners();
                    break;
                case Slider slider:
                    slider.onValueChanged.RemoveAllListeners();
                    break;
                case ScrollRect scrollRect:
                    scrollRect.onValueChanged.RemoveAllListeners();
                    break;
            }
        }
    }

    private CanvasGroup m_canvasGroup;

    protected CanvasGroup canvasGroup
    {
        get
        {
            if (m_canvasGroup == null)
                m_canvasGroup = GetComponent<CanvasGroup>();
            return m_canvasGroup;
        }
    }

    public float GroupAlpha { get => canvasGroup.alpha; set => canvasGroup.alpha = value; }
    public bool GroupInteractable { get => canvasGroup.interactable; set => canvasGroup.interactable = value; }
    public bool GroupBlockEvent { get => canvasGroup.ignoreParentGroups; set => canvasGroup.ignoreParentGroups = value; }

    private WidgetStatus status = WidgetStatus.Display | WidgetStatus.Active;
    public byte Status
    {
        get => (byte)status;
        set
        {
            var target = (WidgetStatus)value;

            var isActive = (target & WidgetStatus.Active) != 0;
            this.gameObject.SetActive(isActive);

            if (((target & WidgetStatus.Display) != 0) != ((status & WidgetStatus.Display) != 0))
            {
                var dis = (target & WidgetStatus.Display) != 0;
                canvasGroup.alpha = dis ? 1 : 0;
                canvasGroup.interactable = dis;
                canvasGroup.blocksRaycasts = dis;
                if (dis)
                    OnDisplay();
                else
                    OnHidle();
            }

            status = target;
        }
    }
}

public partial class UIWidget //Graphic
{
    public static Color UintToColor(uint color)
    {
        Color32 c = new Color32(
              (byte)((color >> 24) & 0xFF),
              (byte)((color >> 16) & 0xFF),
              (byte)((color >> 8) & 0xFF),
              (byte)(color & 0xff)
           );
        return c;
    }

    public void SetColor(int key, uint color)
    {
        if (TryFindComponent<Graphic>(key, out var graphic))
        {
            graphic.color = UintToColor(color);
        }
    }

    public void SetAlpha(int key, float a)
    {
        if (TryFindComponent<Graphic>(key, out var graphic))
        {
            var color = graphic.color;
            color.a = a;
            graphic.color = color;
        }
    }

}

public partial class UIWidget //Text
{
    public void SetText(int key, string content)
    {
        if (TryFindComponent<Text>(key, out var text))
        {
            text.text = content;
        }
    }
    public void SetInPutText(int key,string content = "")
    {
        if (TryFindComponent<InputField>(key, out var text))
        {
            text.text = content;
        }
    }
    public string GetInputText(int key)
    {
        if (TryFindComponent<InputField>(key, out var text))
        {
            return text.text;
        }
        return "";
    }

}
public static class Image_EX
{
    public static void SetEff(this Image image, ImageFilterType type)
    {
        Material material = null;
        switch (type)
        {
            case ImageFilterType.none:
                material = null;
                break;
            case ImageFilterType.gray:
                material = Resources.Load<Material>("Materials/GrayUI");
                break;
        }
        image.material = material;
    }
}
public enum ImageFilterType
{
    none = 0,
    gray
}
public class SpriteLoader
{
    public bool isLoading = false;
    public Sprite sprite;
    public List<Action<Sprite>> loadCallbacks = new List<Action<Sprite>>();
}
public partial class UIWidget //Image
{
    private static Dictionary<string, SpriteLoader> SpriteComs = new Dictionary<string, SpriteLoader>();
    private Dictionary<int, string> LastSpriteComs;
    public void SetSprite(int key, string path, Action callback = null, bool isNativeSiva = false)
    {
        Debug.Assert(!string.IsNullOrEmpty(path), "SetSprite path is null or Empty", this);
        if (TryFindComponent<Image>(key, out var img))
        {
            if (img == null)
            {
                return;
            }
            if (LastSpriteComs == null)
            {
                LastSpriteComs = new Dictionary<int, string>();
            }
            if (LastSpriteComs.TryGetValue(key, out string val))
            {
                LastSpriteComs[key] = path;
            }
            else
            {
                LastSpriteComs.Add(key, path);
            }
            SpriteLoader spriteLoader;
            if (!SpriteComs.TryGetValue(path, out spriteLoader))
            {
                spriteLoader = new SpriteLoader();
                SpriteComs.Add(path, spriteLoader);
            }
            if (spriteLoader.sprite == null)
            {
                spriteLoader.loadCallbacks.Add((Sprite spr) =>
                {
                    if (img == null || spr == null)
                    {
                        return;
                    }
                    if (LastSpriteComs.TryGetValue(key, out string val2))
                    {
                        if (val2 != path)
                        {
                            return;
                        }
                    }
                    img.sprite = spriteLoader.sprite;
                    if (img && isNativeSiva)
                    {
                        img.SetNativeSize();
                    }
                    callback?.Invoke();
                    callback = null;
                });
                if (!spriteLoader.isLoading)
                {
                    spriteLoader.isLoading = true;
                    //ResourceManager.Load(AssetUtility.GetUIAsset(path), typeof(Sprite), (obj) =>
                    //{
                    //    if (null == obj) return;
                    //    var spr = obj as Sprite;
                    //    spriteLoader.sprite = spr;
                    //    foreach (Action<Sprite> action in spriteLoader.loadCallbacks)
                    //    {
                    //        if (action != null)
                    //        {
                    //            action(spr);
                    //        }
                    //    }
                    //    spriteLoader.loadCallbacks.Clear();
                    //});
                }
            }
            else
            {
                if (img == null)
                {
                    return;
                }
                img.sprite = spriteLoader.sprite;
                if (img && isNativeSiva)
                {
                    img.SetNativeSize();
                }
                callback?.Invoke();
                callback = null;
            }
        }
    }

    public void SetRemoteSprite(int key,string urlpath)
    {
        Debug.Assert(!string.IsNullOrEmpty(urlpath),"SetRemoteSprite urlpath isnull or empty",this);
        if (TryFindComponent<Image>(key,out var img))
        {
            //GameUtility.GetRemoteImg(urlpath,img);
            //TODO 设置图片
        }
    }
    /// <summary>
    /// 设置图片自适应背景框
    /// </summary>
    /// <param name="key"></param>
    /// <param name="path"></param>
    /// <param name="parentK"></param>
    public void SetSpriteAutoResizeWithParent(int key, string path,int parentK)
    {
        SetSprite(key, path, () => {
            if (TryFindComponent<RectTransform>(parentK, out var parent))
            {
                var width = parent.sizeDelta.x;
                var height = parent.sizeDelta.y;
                var temp = Mathf.Max(width, height);
                if (TryFindComponent<Image>(key, out var img))
                { 
                    var RT = img.GetComponent<RectTransform>();
                    if (null !=RT )
                    {
                        float max = Mathf.Max(RT.sizeDelta.x, RT.sizeDelta.y);
                        float scale = temp / max;
                        RT.sizeDelta = new Vector2(RT.sizeDelta.x * scale,RT.sizeDelta.y * scale);
                    }
                }
            }
        },true);
    }
    /// <summary>
    /// 设置图片滤镜效果
    /// </summary>
    /// <param name="key"></param>
    /// <param name="type"></param>
    public void SetImageFilter(int key, ImageFilterType type)
    {
        if (TryFindComponent<Image>(key, out var img))
        {
            img.SetEff(type);
        }
    }

    public void SetButtonSprite(int key,string path,Action callback = null,bool isNativeSiva = false)
    {
        Debug.Assert(!string.IsNullOrEmpty(path), "SetSprite path is null or Empty", this);
        if (TryFindComponent<Button>(key,out var button))
        {
            //ResourceManager.Load(AssetUtility.GetUIAsset(path), typeof(Sprite), (obj) =>
            //{
            //    var image = button.GetComponent<Image>();
            //    image.sprite = obj as Sprite;
            //    if (isNativeSiva)
            //    {
            //        image.SetNativeSize();
            //    }
            //    callback?.Invoke();
            //});
        }
    }

    public void SetIcon(int key, int itemid, Action callback = null, bool isNativeSiva = false)
    {
        //var cfg = DataManager.Instance.GetItemById(itemid);
        //if (cfg == null)
        //    return;
        //var iconPath = $"Item/{cfg.icon}";
        //SetSprite(key, iconPath, callback, isNativeSiva);
    }

    public void SetNativeSize(int key)
    {
        if (TryFindComponent<Image>(key, out var img))
        {
            img.SetNativeSize();
        }
    }
}

public partial class UIWidget //Button
{
    public void AddClickListener(int key, UnityAction action, bool reset = false)
    {
        if (TryFindComponent<Button>(key, out var but))
        {
            if (reset)
            {
                but.onClick.RemoveAllListeners();
            }
            but.onClick.AddListener(action);
        }
    }

    public void RemoveClickListener(int key, UnityAction action)
    {
        if (TryFindComponent<Button>(key, out var but))
        {
            but.onClick.RemoveListener(action);
        }
    }

    public void RemoveAllClickListener(int key)
    {
        if (TryFindComponent<Button>(key, out var but))
        {
            but.onClick.RemoveAllListeners();
        }
    }

}

public partial class UIWidget //obj component
{
    public void SetObjActive(int key, bool active)
    {
        if (TryFindComponent<Component>(key, out var com))
        {
            com.gameObject.SetActive(active);
        }
    }

    public void SetEnabled(int key, bool enabled)
    {
        if (TryFindComponent<MonoBehaviour>(key, out var mono))
        {
            mono.enabled = enabled;
        }
    }
    public void GetObjActive(int key, out bool active)
    {
        if (TryFindComponent<Component>(key, out var com))
        {
            active = com.gameObject.activeSelf;
        }
        else
        {
            active = false;
        }
    }

    public void DelayRebuildLayout(int key)
    {
        if(TryFindComponent<Component>(key,out var com))
        {
            var childArray = com.transform.GetComponentsInChildren<RectTransform>();
            foreach (var item in childArray)
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            }
            CancelInvoke("DelayRebuildLayout");
        }
    }
}

public partial class UIWidget //Slider
{
    public void GetSliderValue(int key, out float value)
    {
        value = 0;
        if (TryFindComponent<Slider>(key, out var slider))
        {
            value = slider.value;
        }
    }

    public void SetSliderValue(int key, float value)
    {
        if (TryFindComponent<Slider>(key, out var slider))
        {
            slider.value = value;
        }
    }

    public void AddSliderListener(int key, UnityAction<float> action)
    {
        if (TryFindComponent<Slider>(key, out var slider))
        {
            slider.onValueChanged.AddListener(action);
        }
    }

    public void RemoveSliderListener(int key, UnityAction<float> action)
    {
        if (TryFindComponent<Slider>(key, out var slider))
        {
            slider.onValueChanged.RemoveListener(action);
        }
    }

    public void RemoveSliderAllListener(int key)
    {
        if (TryFindComponent<Slider>(key, out var slider))
        {
            slider.onValueChanged.RemoveAllListeners();
        }
    }

    public void SetSliderInteractable(int key ,bool b)
    {
        if (TryFindComponent<Slider>(key, out var slider))
        {
            slider.interactable = b;
        }
    }

    public void GetSliderInteractable(int key, out bool b)
    {
        b = true;
        if (TryFindComponent<Slider>(key, out var slider))
        {
             b = slider.interactable ;
        }
    }

    public void SetSliderMaxValue(int key, int max)
    {
        if (TryFindComponent<Slider>(key, out var slider))
        {
            slider.maxValue = max;
        }
    }

}

public partial class UIWidget //ScrollRect
{
    public void AddScrollRectListener(int k, UnityAction<Vector2> action)
    {
        if (TryFindComponent<ScrollRect>(k, out var sc))
        {
            sc.onValueChanged.AddListener(action);
        }
    }
    public void RemoveScrollRectListener(int k, UnityAction<Vector2> action)
    {
        if (TryFindComponent<ScrollRect>(k, out var sc))
        {
            sc.onValueChanged.RemoveListener(action);
        }
    }

    public void RemoveAllScrollRectListener(int key)
    {
        if (TryFindComponent<ScrollRect>(key, out var sc))
        {
            sc.onValueChanged.RemoveAllListeners();
        }
    }
    public void ScrollMoveTo(int key,float v)
    {
        if (TryFindComponent<ScrollRect>(key, out var sc))
        {
            sc.verticalNormalizedPosition = v;
        }
    }
}

public partial class UIWidget //RectTransform
{
    public void SetInsetAndSizeFromParentEdge(int key, RectTransform.Edge edge,float inset,float size)
    {
        if (TryFindComponent<RectTransform>(key, out var rectTransform))
        {
            rectTransform.SetInsetAndSizeFromParentEdge(edge,inset,size);
        }
    }

    public void SetScale(int key, float scale)
    {
        if (TryFindComponent<Transform>(key, out var transform))
        {
            transform.localScale = Vector3.one * scale;
        }
    }

    public void SetScale(int key, Vector3 scale)
    {
        if (TryFindComponent<Transform>(key, out var transform))
        {
            transform.localScale = scale;
        }
    }

    public void SetAnchoredPosition(int key, Vector2 anchoredPosition)
    {
        if (TryFindComponent<RectTransform>(key, out var rect))
        {
            rect.anchoredPosition = anchoredPosition;
        }
    }

    public void SetAnchoredPosition(int key, float x, float y)
    {
        if (TryFindComponent<RectTransform>(key, out var rect))
        {
            rect.anchoredPosition = new Vector2(x, y);
        }
    }

    public void SetLocalPositionX(int key, float x)
    {
        if (TryFindComponent<RectTransform>(key, out var rect))
        {
            var newpositon = new Vector3(rect.localPosition.x, rect.localPosition.y, rect.localPosition.z);
            newpositon.x = x;
            rect.localPosition = newpositon;
        }
    }

    public void SetLocalPositionY(int key, float y)
    {
        if (TryFindComponent<RectTransform>(key, out var rect))
        {
            var newpositon = new Vector3(rect.localPosition.x, rect.localPosition.y, rect.localPosition.z);
            newpositon.y = y;
            rect.localPosition = newpositon;
        }
    }

    public void SetHeight(int k, float y)
    {
        if (TryFindComponent<RectTransform>(k, out var ret))
        {
            ret.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
        }
    }

    public void SetWidth(int k, float w)
    {
        if (TryFindComponent<RectTransform>(k, out var ret))
        {
            ret.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        }
    }

    public void GetWidth(int key, out float width)
    {
        width = default;
        if (TryFindComponent<RectTransform>(key, out var ret))
        {
            width = ret.rect.width;
        }
    }

    public void GetHeight(int key, out float height)
    {
        height = default;
        if (TryFindComponent<RectTransform>(key, out var ret))
        {
            height = ret.rect.height;
        }
    }

    public void SetAnchoredPositionOffset(int k, float x, float y)
    {
        if (TryFindComponent<RectTransform>(k, out var ret))
        {
            var pos = ret.anchoredPosition;
            pos.x += x;
            pos.y += y;
            ret.anchoredPosition = pos;
        }
    }

    public void SetFollowTarget(int k, Transform tra)
    {
        if (TryFindComponent<PositionConstraint>(k, out var pos))
        {
            if (pos.sourceCount > 0)
                pos.RemoveSource(0);
            pos.AddSource(new ConstraintSource()
            {
                weight = 1,
                sourceTransform = tra
            });
        }
    }

    public void SetFollowTargetOffSet(int k, Vector3 v3)
    {
        if (TryFindComponent<PositionConstraint>(k, out var pos))
        {
            pos.translationOffset = v3;
        }
    }

    public void DestoryObj()
    {
        Destroy(this.gameObject);
    }
    public UIWidget InstantiateObj(int k , int k1)
    {
        UIWidget wid = null;
        if (TryFindComponent<UIWidget>(k,out var widget))
        {
            if (TryFindComponent<RectTransform>(k1,out var tra))
            {

               var obj = Instantiate(widget.gameObject,tra);
               wid = obj.GetComponent<UIWidget>();
            }
        }
        return wid;
    }
 /// <summary>
 /// 设置子节点索引位置
 /// </summary>
 /// <param name="k"></param>
 /// <param name="i"></param>
    public void SetChaildIndex(int k, int i)
    {
        if (TryFindComponent<RectTransform>(k, out var ret))
        {
            ret.SetSiblingIndex(i);
        }
    }
}

public partial class UIWidget //Toggle
{
    public void ToggleSetGroup(int key, ToggleGroup toggleGroup)
    {
        if (TryFindComponent(key, out Toggle toggle))
        {
            Debug.Assert(toggleGroup, "toggleGroup null");
            toggle.group = toggleGroup;
        }
    }
    public void AddToggleEvent(int key, UnityAction<bool> callBack)
    {
        if (TryFindComponent(key, out Toggle tg))
        {
            tg.onValueChanged.AddListener(callBack);
        }
    }

    public void RemoveToggleListener(int key, UnityAction<bool> callBack)
    {
        if (TryFindComponent(key, out Toggle tg))
        {
            tg.onValueChanged.RemoveListener(callBack);
        }
    }

    public void RemoveAllListener(int key)
    {
        //if (TryFindComponent(key, out Toggle tg))
        //{
        //    tg.onValueChanged.RemoveAllListeners();
        //}
        var component = array[key];
        switch (component)
        {
            case Button but:
                but.onClick.RemoveAllListeners();
                break;
            case Toggle tog:
                tog.onValueChanged.RemoveAllListeners();
                break;
            case Slider slider:
                slider.onValueChanged.RemoveAllListeners();
                break;
            case ScrollRect scrollRect:
                scrollRect.onValueChanged.RemoveAllListeners();
                break;
        }
    }
    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="key"></param>
    /// <param name="isOn"></param>
    public void SetToggleIsOn(int key,bool isOn)
    {
        if (TryFindComponent(key, out Toggle tg))
        {
            tg.isOn = isOn;
        }
    }
}


public partial class UIWidget //layout
{
     public void SetHSpacing(int key, float spacing)
    {
        if (TryFindComponent<HorizontalLayoutGroup>(key, out var layout))
        {
            layout.spacing = spacing;
        }
    }
    public void SetHRight(int key, int n)
    {
        if (TryFindComponent<HorizontalLayoutGroup>(key, out var layout))
        {
            layout.padding.right = n;
        }
    }
}