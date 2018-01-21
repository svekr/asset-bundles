using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public class BuildTools {

    [MenuItem("Tools/Build Asset Bundles Standalone")]
    public static void BuildAssetBundlesStandalone() {
        BuildAssetBundlesMethod(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tools/Build Asset Bundles Android")]
    public static void BuildAssetBundlesAndroid() {
        BuildAssetBundlesMethod(BuildTarget.Android);
    }

    [MenuItem("Tools/Build Asset Bundles iOS")]
    public static void BuildAssetBundlesIOS() {
        BuildAssetBundlesMethod(BuildTarget.iOS);
    }

    [MenuItem("Tools/Make Build Standalone")]
    public static void MakeBuildWindows() {
        string fileName = MakeBuild(BuildTarget.StandaloneWindows, "exe");
        RunBuild(fileName);
    }

    [MenuItem("Tools/Make Build Android")]
    public static void MakeBuildAndroid() {
        MakeBuild(BuildTarget.Android, "apk");
    }

    [MenuItem("Tools/Make Build iOS")]
    public static void MakeBuildIOS() {
        MakeBuild(BuildTarget.iOS, "ipa");
    }

    private static void BuildAssetBundlesMethod(BuildTarget buildTarget) {
        string directory = Path.Combine(Application.streamingAssetsPath, "assetBundles");
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, buildTarget);
    }

    private static string MakeBuild(BuildTarget buildTarget, string extension) {
        string fileName = null;
        if (!BuildPipeline.isBuildingPlayer) {
            string directory = Path.Combine(Application.persistentDataPath, "/builds");
            fileName = EditorUtility.SaveFilePanel("Select build name", directory, "build", extension);
            if (fileName != null && fileName.Length > 0) {
                BuildPlayerOptions options = new BuildPlayerOptions();
                options.scenes = new[] {"Assets/Scenes/Scene.unity"};
                options.locationPathName = fileName;
                options.target = buildTarget;
                options.options = BuildOptions.None;
                BuildPipeline.BuildPlayer(options);
            }
        }
        return fileName;
    }

    private static void RunBuild(string fileName) {
        if (fileName != null && fileName.Length > 0) {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.Start();
        }
    }
}
