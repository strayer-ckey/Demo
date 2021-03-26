using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomCreateRenderTextureWizard : ScriptableWizard
{
    public int width;
    public int height;
    public string defaultStorePath = "Assets/Custom";
    void OnWizardUpdate()
    {
        isValid = defaultStorePath != null && width > 0 && height > 0;
    }

    void OnWizardCreate()
    {
        RenderTexture texture = new RenderTexture(width, height, 0);
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = texture;
    }

    [MenuItem("Custom/CreateRenderTexture")]
    static void CreateNormal()
    {
        ScriptableWizard.DisplayWizard<CustomCreateNormalWizard>(
            "Create RenderTexture", "Run");
    }
}
