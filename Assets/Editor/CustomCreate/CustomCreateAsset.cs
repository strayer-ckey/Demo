using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomCreateAsset : MonoBehaviour
{
    public GameObject t;

    public static void CreateNewAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Not select files, select files first! ");
            return;
        }
        else if (!string.IsNullOrEmpty(Path.GetExtension(path)))
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName =
            AssetDatabase.GenerateUniqueAssetPath(path + "/New" + typeof(T).ToString() + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
