using UnityEngine;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(UIWidget))]
public class UIWidgetInspector : Editor
{
    SerializedProperty arrayProperty;
    ReorderableList reorderableList;

    [SerializeField] private bool foldout = true;

    protected virtual void OnEnable()
    {
        arrayProperty = serializedObject.FindProperty("array");

        reorderableList = new ReorderableList(serializedObject, arrayProperty, true, false, true, true);

        reorderableList.drawElementCallback = DrawElementCallback;
        reorderableList.drawHeaderCallback = DrawHeaderCallback;
        reorderableList.onRemoveCallback = OnRemoveCallback;
        reorderableList.onAddCallback = OnAddCallback;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        foldout = EditorGUILayout.Foldout(foldout, new GUIContent("Component Array"));
        if (foldout)
        {
            reorderableList.DoLayoutList();
        }
        if (GUILayout.Button("复制Key"))
        {
            var str = new StringBuilder(50);
            str.AppendLine("local KEY = {");
            var site = target as UIWidget;
            for (int i = 0; i < site.Size; i++)
            {
                var com = site[i];
                if (com != null)
                {
                    str.AppendFormat("\t{0}_{1} = {2},\r\n", com.gameObject.name, com.GetType().Name, i);
                }
                else
                {
                    Debug.LogError($"请检查list ID = {i} 值为空");
                }
            }
            str.AppendLine("}");
            // str.Append("return KEY");

            TextEditor t = new TextEditor();
            t.text = str.ToString();
            t.OnFocus();
            t.Copy();
        }

        if (GUILayout.Button("InitUIComponet"))
        {
            var str = new StringBuilder(50);
            str.AppendLine("local function  InitUIComponet (self)");
            str.AppendLine("\tlocal uiWidget = self.m_uiWidget");
            var site = target as UIWidget;
            for (int i = 0; i < site.Size; i++)
            {
                var com = site[i];
                str.AppendFormat("\tself.{0}_{1} = uiWidget[KEY.{0}_{1}] \r\n", com.gameObject.name, com.GetType().Name);
                // str.AppendFormat("\t{0}_{1} = {2},\r\n",com.gameObject.name,com.GetType().Name,i);
            }
            str.AppendLine("end");
            // str.Append("return KEY");

            TextEditor t = new TextEditor();
            t.text = str.ToString();
            t.OnFocus();
            t.Copy();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = arrayProperty.GetArrayElementAtIndex(index);
        var obj = element.objectReferenceValue as Component;
        var lableName = obj == null ? "Null" : $"{obj.name}_{obj.GetType().Name}";

        var labRect = new Rect(rect.x,rect.y,rect.width*0.7f,rect.height);
        var tyRect = new Rect(rect.x+labRect.width,rect.y,rect.width*0.3f,rect.height);
        EditorGUI.PropertyField(labRect, element, new GUIContent($"[{index}]{lableName}"));
        if(obj==null)
        {
            EditorGUI.Popup(tyRect,0,new string[]{typeof(RectTransform).Name});
        }
        else
        {
            var components = obj.GetComponents<Component>().ToList();
            var oldIndex = components.IndexOf(obj);
            var temp = components.Select(com=>com.GetType().Name).ToArray();
            var array = new string[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                var name = temp[i];
                var tempindex = 1;
                while(array.Contains(name))
                {
                    name = $"{temp[i]}_{tempindex++}";
                }
                array[i] = name;
            }

            var newIndex = EditorGUI.Popup(tyRect,oldIndex,array);
            if(oldIndex!=newIndex)
            {
                element.objectReferenceValue = components[newIndex];
            }
        }
    }

    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, arrayProperty.displayName);
    }

    private void OnRemoveCallback(ReorderableList list)
    {
        arrayProperty.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
        list.serializedProperty.DeleteArrayElementAtIndex(list.index);
    }

    private void OnAddCallback(ReorderableList list)
    {
        list.serializedProperty.arraySize++;
        list.index = list.serializedProperty.arraySize - 1;

        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
        itemData.objectReferenceValue = default;
    }
}
