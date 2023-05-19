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
      treesByPosition.TryAdd(tree.position.ToString(), new TerrainTree(tree));
    }
  }

  public void Harvest(TreeInstance tree)
  {
    trees.Remove(tree);
    terrain.terrainData.SetTreeInstances(trees.ToArray(), true);
  }

  public void ApplyDamage(float damage)
  {
    Vector3 position = GetClosestTree(player.transform.position);
    treesByPosition.TryGetValue(position.ToString(), out TerrainTree tree);
    if (tree != null)
    {
      tree.ApplyDamage(damage);
      if (tree.Health <= 0)
      {
        Harvest(tree.Tree);
      }
    }
  }

  private Vector3 GetLocalTreePosition(Vector3 treePosition)
  {
    return Vector3.Scale(treePosition, terrain.terrainData.size) + terrain.transform.position;
  }

  private Vector3 GetClosestTree(Vector3 playerPosition)
  {
    Vector3 currentTreePosition;
    float minDistance = Mathf.Infinity;
    TreeInstance closestTree = trees[0];

    for (int i = 0; i < trees.Count; i++)
    {
      currentTreePosition = GetLocalTreePosition(trees[i].position);

      float currentDistance = Vector3.Distance(playerPosition, currentTreePosition);
      if (currentDistance < minDistance)
      {
        minDistance = currentDistance;
        closestTree = trees[i];
      }
    }

    return closestTree.position;
  }

  private void RespawnTrees()
  {
    foreach (KeyValuePair<string, TerrainTree> tree in treesByPosition)
    {
      if (tree.Value.IsChopped)
      {
        trees.Add(tree.Value.Tree);
      }
    }
    terrain.terrainData.SetTreeInstances(trees.ToArray(), true);
  }

  void OnApplicationQuit()
  {
    RespawnTrees();
    Debug.Log("Application ending after " + Time.time + " seconds");
  }
}
