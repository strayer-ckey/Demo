using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomCreateNormalWizard : ScriptableWizard
{
    public Texture2D sourceTexture;
    public string defaultStorePath = "Assets/Custom";

    void OnWizardUpdate()
    {
        helpString = "Default store path : ";
        isValid = sourceTexture != null && defaultStorePath != null;
    }
    void OnWizardCreate()
    {
        Shader createNormalShader = Shader.Find("Custom/CustomeCreateNormalShader");
        Material material = new Material(createNormalShader);
        material.SetTexture("_MainTex", sourceTexture);

        RenderTexture texture = new RenderTexture(sourceTexture.width, sourceTexture.height, 0);
        Graphics.Blit(sourceTexture, texture, material);

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = texture;
        Texture2D png = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(defaultStorePath))
        {
            Directory.CreateDirectory(defaultStorePath);
        }
        string filePath = "Normal" + System.DateTime.Now + ".png";
        filePath = filePath.Replace("/", "-");
        filePath = filePath.Replace(" ", "_");
        filePath = filePath.Replace(":", ".");
        filePath = defaultStorePath + "/" + filePath;
        FileStream file = File.Open(filePath, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;

        Debug.Log("Already Create, path : " + filePath);
    }

    [MenuItem("Custom/CreateNormal")]
    static void CreateNormal()
    {
        ScriptableWizard.DisplayWizard<CustomCreateNormalWizard>(
            "Create Normal", "Run");
    }
}
