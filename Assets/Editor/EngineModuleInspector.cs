using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseModule))]
public class EngineModuleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EngineModule engineModule = (EngineModule) target;
        if(GUILayout.Button("Break Module"))
        {
            engineModule.EditorBreak();
        }
    }
}
