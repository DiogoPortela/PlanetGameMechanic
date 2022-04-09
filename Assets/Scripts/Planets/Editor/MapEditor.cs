using UnityEngine;
using UnityEditor;
using pt.dportela.PlanetGame.PlanetGeneration;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    static bool shapeSettingFoldout = true;
    static bool colorSettingFoldout = true;

    private Map map;
    
    private Editor shapeEditor;
    private Editor colorEditor;

    private void OnEnable()
    {
        map = (Map)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawSettingsEditor(map.shapeSettings, null, ref shapeSettingFoldout, ref shapeEditor);
        DrawSettingsEditor(map.colorSettings, map.OnUpdateColorSettings, ref colorSettingFoldout, ref colorEditor);

        GUILayout.Space(50);
        if (GUILayout.Button("Generate Planet"))
        {
            map.InitializeMap(true);
            map.GenerateMap();
            map.UpdateMesh();
        }
        GUILayout.Space(25);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }
}
