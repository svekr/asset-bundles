using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LoadAssetBundle : MonoBehaviour {

    void Start() {
        //LoadBundleFromFile();
        StartCoroutine(LoadBundleWithRequest());
    }

    private IEnumerator LoadBundleWithRequest() {
#if UNITY_EDITOR
        string path = "file:///" + Path.Combine(Application.streamingAssetsPath, "assetBundles/prototyping/blocks");
#elif UNITY_ANDROID
        string path = "jar:file:///" + Application.dataPath + "!/assets/" + "assetBundles/prototyping/blocks";
#else
        string path = "file:///" + Application.dataPath + "/Raw/" + "assetBundles/prototyping/blocks";
#endif
        Debug.Log("Start load bundle: " + path);
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(path);
        yield return request.SendWebRequest();

        Debug.Log("Load complete.");
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        ProcessAssetBundle(bundle);
    }

    private void LoadBundleFromFile() {
        string path = Path.Combine(Application.streamingAssetsPath, "assetBundles/prototyping/blocks");
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        ProcessAssetBundle(bundle);
    }

    private void ProcessAssetBundle(AssetBundle bundle) {
        if (bundle == null) {
            Debug.Log("Asset bundle load error.");
        } else {
            Debug.Log("Asset bundle load complete successfully.");
            ProcessPrefab(bundle, "CubeGray", Vector3.zero);
            ProcessPrefab(bundle, "CubePink", new Vector3(10f, 0f, -10f));
        }
    }

        private void ProcessPrefab(AssetBundle bundle, string prefabName, Vector3 position) {
        GameObject prefab = bundle.LoadAsset<GameObject>(prefabName);
        if (prefab == null) {
            Debug.Log("Prefab load error.");
        } else {
            Debug.Log("Prefab loaded successfully.");
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
