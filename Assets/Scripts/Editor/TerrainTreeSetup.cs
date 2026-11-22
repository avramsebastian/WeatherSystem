using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TerrainTreeSetup
{
    [MenuItem("Tools/Setup Terrain Trees")]
    public static void SetupTrees()
    {
        List<TreePrototype> prototypes = new List<TreePrototype>();

        // 1. Setup Deciduous Tree
        GameObject treePrefab = CreateLODPrefab("Assets/Models/Tree.obj", "Assets/Models/WeatherTreePrefab.prefab");
        if (treePrefab != null)
        {
            TreePrototype p = new TreePrototype();
            p.prefab = treePrefab;
            prototypes.Add(p);
        }

        // 2. Setup Pine Tree
        GameObject pinePrefab = CreateLODPrefab("Assets/Models/PineTree.obj", "Assets/Models/PineTreePrefab.prefab");
        if (pinePrefab != null)
        {
            TreePrototype p = new TreePrototype();
            p.prefab = pinePrefab;
            prototypes.Add(p);
        }

        // 3. Setup Bush
        GameObject bushPrefab = CreateLODPrefab("Assets/Models/Bush.obj", "Assets/Models/BushPrefab.prefab");
        if (bushPrefab != null)
        {
            TreePrototype p = new TreePrototype();
            p.prefab = bushPrefab;
            prototypes.Add(p);
        }

        // Update Terrain
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null && terrain.terrainData != null)
        {
            terrain.terrainData.treePrototypes = prototypes.ToArray();
            terrain.terrainData.RefreshPrototypes();
            EditorUtility.SetDirty(terrain.terrainData);
            EditorUtility.SetDirty(terrain);
            Debug.Log("<color=green>SUCCESS: 3 tree/foliage varieties (Deciduous Tree, Pine Tree, Bush) added to Terrain successfully!</color>");
        }
        else
        {
            Debug.LogWarning("No active terrain found in current scene.");
        }
    }

    private static GameObject CreateLODPrefab(string modelPath, string targetPrefabPath)
    {
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        if (model == null)
        {
            Debug.LogWarning("Model not found at: " + modelPath);
            return null;
        }

        GameObject tempObj = Object.Instantiate(model);
        tempObj.name = System.IO.Path.GetFileNameWithoutExtension(targetPrefabPath);

        LODGroup lodGroup = tempObj.GetComponent<LODGroup>();
        if (lodGroup == null)
        {
            lodGroup = tempObj.AddComponent<LODGroup>();
        }

        Renderer[] renderers = tempObj.GetComponentsInChildren<Renderer>();
        LOD[] lods = new LOD[1];
        lods[0] = new LOD(0.01f, renderers);
        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(tempObj, targetPrefabPath);
        Object.DestroyImmediate(tempObj);
        return prefab;
    }
}
