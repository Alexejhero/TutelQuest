using UnityEditor;

namespace SchizoQuest.Editor
{
    public static class SaveAllAssets
    {
        [MenuItem("SCHIZO/Save All Assets")]
        public static void Save()
        {
            AssetDatabase.SaveAssets();
        }
    }
}
