using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
#if UNITY_EDITOR
public class ColorSwatchMaker : MonoBehaviour
{
    public string fileName = "New ColorSwatch";
    public string textInputField;

    public void CreateSwatch()
    {
        string data =
        "%YAML 1.1" + "\n" +
        "%TAG !u! tag:unity3d.com,2011:" + "\n" +
        "--- !u!114 &1" + "\n" +
        "MonoBehaviour:" + "\n" +
        "  m_ObjectHideFlags: 52" + "\n" +
        "  m_CorrespondingSourceObject: {fileID: 0}" + "\n" +
        "  m_PrefabInstance: {fileID: 0}" + "\n" +
        "  m_PrefabAsset: {fileID: 0}" + "\n" +
        "  m_GameObject: {fileID: 0}" + "\n" +
        "  m_Enabled: 1" + "\n" +
        "  m_EditorHideFlags: 0" + "\n" +
        "  m_Script: {fileID: 12323, guid: 0000000000000000e000000000000000, type: 0}" + "\n" +
        "  m_Name: " + fileName + "\n" +
        "  m_EditorClassIdentifier: " + "\n" +
        "  m_Presets:" + "\n";

        string[] hexes = textInputField.Split('\n');
        for (int i = 0; i < hexes.Length; i++)
        {
            if(hexes[i].Length == 7)
                hexes[i] = hexes[i].Remove(6);

            if(hexes[i].Length == 6 || hexes[i].Length == 8)
            {
                data += "  - m_Name: \n";
                data += "    m_Color: { ";
                data += HexToRGBA(hexes[i]);
                data += "}" + "\n";
            }
            
        }

        string dataPath = Application.dataPath + "/Editor/";
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        dataPath += fileName + ".colors";
        try
        {
            System.IO.File.WriteAllText(dataPath, data);
            AssetDatabase.Refresh();
        }
        catch (System.Exception ex)
        {
            string ErrorMessages = "File Write Error\n" + ex.Message;
            Debug.LogError(ErrorMessages);
        }
    }

    private string HexToRGBA(string hexColor)
    {
        string returnStr = "";
        Vector4 returnColor = Vector4.one;

        //Color conversion
        string rS = "", gS = "", bS = "", aS = "FF";
        rS = hexColor.Substring(0, 2);
        gS = hexColor.Substring(2, 2);
        bS = hexColor.Substring(4, 2);
        if (hexColor.Length == 8)
            aS = hexColor.Substring(6, 2);

        int intValue = Int32.Parse(rS, System.Globalization.NumberStyles.HexNumber);
        returnStr += "r: " + ((float)intValue / 255f).ToString().Replace(',', '.') + ", ";
        intValue = Int32.Parse(gS, System.Globalization.NumberStyles.HexNumber);
        returnStr += "g: " + ((float)intValue / 255f).ToString().Replace(',', '.') + ", ";
        intValue = Int32.Parse(bS, System.Globalization.NumberStyles.HexNumber);
        returnStr += "b: " + ((float)intValue / 255f).ToString().Replace(',', '.') + ", ";
        intValue = Int32.Parse(aS, System.Globalization.NumberStyles.HexNumber);
        returnStr += "a: " + ((float)intValue / 255f).ToString().Replace(',', '.');

        return returnStr;
    }
}

[CustomEditor(typeof(ColorSwatchMaker))]
public class ColorSwatchMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as ColorSwatchMaker;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("File Name");
        myScript.fileName = EditorGUILayout.TextField(myScript.fileName);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Hex Colors");
        myScript.textInputField = EditorGUILayout.TextArea(myScript.textInputField, GUILayout.MinHeight(20));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create Swatch Asset"))
        {
            if (myScript.fileName != null && myScript.textInputField != null)
            {
                myScript.CreateSwatch();
            }
            else
            {
                Debug.LogWarning("textInputField or fileName is empty.");
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox("Enter hex colors line by line", MessageType.Info);
        EditorGUILayout.EndHorizontal();
    }
}
#endif