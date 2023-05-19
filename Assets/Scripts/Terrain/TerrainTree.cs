using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainTree
{
  private TreeInstance tree;
  private int index;
  private Vector3 position;
  private float health = 100f;
  private bool isChopped = false;

  public TreeInstance Tree { get => tree; set => tree = value; }
  public int Index { get => index; set => index = value; }
  public Vector3 Position { get => position; set => position = value; }
  public float Health { get => health; set => health = value; }
  public bool IsChopped { get => isChopped; set => isChopped = value; }

  public TerrainTree(TreeInstance treeInstance)
  {
    tree = treeInstance;
    index = tree.prototypeIndex;
    position = tree.position;
  }

  public void ApplyDamage(float damage)
  {
    health -= damage;
    Debug.Log("damage health: " + health);
    if (health <= 0)
    {
      isChopped = true;
    }
  }
}
