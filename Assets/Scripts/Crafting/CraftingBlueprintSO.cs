using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom serializable class
[System.Serializable]
public class CraftingResource
{
  public ItemSO resource;
  public int amount = 1;
}

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Game/Crafting/Blueprint")]
public class CraftingBlueprintSO : ScriptableObject
{
  public string blueprintName;
  public CraftingResource[] requirements;
  public ItemSO result;
  public int resultAmount = 1;
  public float craftTime = 1f;
}

