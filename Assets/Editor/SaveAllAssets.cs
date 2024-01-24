using UnityEditor;

namespace SchizoQuest.Editor
{
    public static class SaveAllAssets
    {
        [MenuItem("Tools/Save All Assets", false, -1000)]
        public static void Save()
        {
            AssetDatabase.SaveAssets();
        }
    }
}
