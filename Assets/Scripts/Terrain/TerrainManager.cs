using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
  Terrain terrain;
  Dictionary<string, TerrainTree> treesByPosition = new Dictionary<string, TerrainTree>();
  List<TreeInstance> trees;

  void Start()
  {
    terrain = Terrain.activeTerrain;
    terrainDataSize = Terrain.activeTerrain.terrainData.size;
    trees = new List<TreeInstance>(terrain.terrainData.treeInstances);

    for (int i = 0; i < terrain.terrainData.treeInstanceCount; i++)
    {
      TreeInstance tree = terrain.terrainData.GetTreeInstance(i);
      treesByPosition.TryAdd(tree.position.ToString(), new TerrainTree(i, tree.position, 100f));
    }
  }

}
