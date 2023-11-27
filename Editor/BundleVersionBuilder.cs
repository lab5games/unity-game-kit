using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Lab5Games.Editor
{
    public class BundleVersionBuilder : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            string bundleVer = PlayerSettings.bundleVersion;
            string[] arrVer = bundleVer.Split('.');

            if(arrVer.Length != 3)
            {
                Debug.Log("Creating a new bundle version 0.0.1");
                arrVer = new string[] { "0", "0", "1"};
            }

            bool verCheck = true;
            verCheck &= int.TryParse(arrVer[0], out int major);
            verCheck &= int.TryParse(arrVer[1], out int minor);
            verCheck &= int.TryParse(arrVer[2], out int build);

            if (!verCheck)
                throw new System.Exception($"Invalid bundle version {arrVer[0]}.{arrVer[1]}.{arrVer[2]}");

            string strVer = string.Format("{0}.{1}.{2}", major, minor, build);
            PlayerSettings.bundleVersion = strVer;
            
            AssetDatabase.SaveAssets();
            Debug.LogWarning($"Building application with version {strVer}");
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            string bundleVer = PlayerSettings.bundleVersion;
            string[] arrVer = bundleVer.Split('.');

            if(arrVer.Length != 3)
            {
                Debug.LogError($"Invalid bundle version {bundleVer}");
                return;
            }

            bool verCheck = true;
            verCheck &= int.TryParse(arrVer[0], out int major);
            verCheck &= int.TryParse(arrVer[1], out int minor);
            verCheck &= int.TryParse(arrVer[2], out int build);

            if (!verCheck)
            {
                Debug.LogError($"Invalid bundle version {arrVer[0]}.{arrVer[1]}.{arrVer[2]}");
                return;
            }

            string newVer = string.Format("{0}.{1}.{2}", major, minor, build + 1);
            PlayerSettings.bundleVersion = newVer;
            PlayerSettings.Android.bundleVersionCode = build + 1;
            PlayerSettings.iOS.buildNumber = (build + 1).ToString();

            AssetDatabase.SaveAssets();
        }
    }
}
