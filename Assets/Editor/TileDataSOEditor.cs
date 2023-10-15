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
    //Here is the meat of the script
    public override void OnInspectorGUI()
    {
        //Draw whatever we already have in SO definition
        base.OnInspectorGUI();
        //Guard clause
        if (tileDataSO.tileSprite == null)
            return;

        //Convert the weaponSprite (see SO script) to Texture
        Texture2D texture = AssetPreview.GetAssetPreview(tileDataSO.tileSprite);
        //This allows us to place the image JUST UNDER our default inspector
        GUILayout.Label("", GUILayout.Height(150), GUILayout.Width(150));
        //Draws the texture where we have defined our Label (empty space)
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
#endif