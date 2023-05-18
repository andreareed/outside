using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainTree : MonoBehaviour
{
  private int index;
  private Vector3 position;
  private float health;
  private bool isChopped = false;

  public int Index { get => index; set => index = value; }
  public float Health { get => health; set => health = value; }
  public bool IsChopped { get => isChopped; set => isChopped = value; }



  public TerrainTree(int i, Vector3 pos, float hp)
  {
    index = i;
    position = pos;
    health = hp;
  }

  public void UpdateHealth(float damage)
  {
    health -= damage;
    if (health <= 0)
    {
      isChopped = true;
    }
  }
}
