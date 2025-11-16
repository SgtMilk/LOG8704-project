using UnityEditor;
using UnityEngine;

public class RevertURPMaterials
{
    [MenuItem("Tools/Revert URP Materials to Standard")]
    static void ConvertMaterials()
    {
        var materialGUIDs = AssetDatabase.FindAssets("t:Material");

        foreach (var guid in materialGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null) continue;

            // If it's a URP material, convert it
            if (mat.shader.name.Contains("Universal Render Pipeline"))
            {
                Undo.RecordObject(mat, "Revert URP Material");
                mat.shader = Shader.Find("Standard");

                // Try to map basic properties
                if (mat.HasProperty("_BaseMap"))
                    mat.SetTexture("_MainTex", mat.GetTexture("_BaseMap"));

                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_Color", mat.GetColor("_BaseColor"));
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Finished converting URP materials.");
    }
}