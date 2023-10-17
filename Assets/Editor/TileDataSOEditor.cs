#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileDataSO))]
public class TileDataSOEditor : Editor
{
    TileDataSO tileDataSO;
    private void OnEnable()
    {
        tileDataSO = target as TileDataSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (tileDataSO.tileSprite == null)
            return;
        Texture2D texture = AssetPreview.GetAssetPreview(tileDataSO.tileSprite);

        GUILayout.Label("", GUILayout.Height(150), GUILayout.Width(150));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
#endif