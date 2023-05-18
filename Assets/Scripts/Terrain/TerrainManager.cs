using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
  Terrain terrain;
  GameObject player;
  Dictionary<string, TerrainTree> treesByPosition = new Dictionary<string, TerrainTree>();
  List<TreeInstance> trees;

  void Start()
  {
    terrain = Terrain.activeTerrain;
    trees = new List<TreeInstance>(terrain.terrainData.treeInstances);
    player = GameObject.Find("Player");

    for (int i = 0; i < terrain.terrainData.treeInstanceCount; i++)
    {
      TreeInstance tree = terrain.terrainData.GetTreeInstance(i);
      treesByPosition.TryAdd(tree.position.ToString(), new TerrainTree(i, tree.position, 100f));
    }
  }

  public void Harvest(int index)
  {
    TreeInstance tree = terrain.terrainData.GetTreeInstance(index);
    trees.Remove(tree);
    terrain.terrainData.treeInstances = trees.ToArray();
  }

  public void ApplyDamage(float damage)
  {
    Vector3 position = GetClosestTree(player.transform.position);
    treesByPosition.TryGetValue(position.ToString(), out TerrainTree tree);
    if (tree != null)
    {
      tree.ApplyDamage(damage);
      if (treesByPosition[position.ToString()].Health <= 0)
      {
        Harvest(tree.Index);
      }
    }
  }

  private Vector3 GetClosestTree(Vector3 playerPosition)
  {
    Vector3 currentTreePosition;
    float minDistance = Mathf.Infinity;
    TreeInstance closestTree = trees[0];

    for (int i = 0; i < trees.Count; i++)
    {
      currentTreePosition = trees[i].position;
      float currentDistance = Vector3.Distance(playerPosition, currentTreePosition);
      if (currentDistance < minDistance)
      {
        minDistance = currentDistance;
        closestTree = trees[i];
      }
    }

    return closestTree.position;
  }
}
