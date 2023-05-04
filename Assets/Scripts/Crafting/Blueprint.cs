using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blueprint : MonoBehaviour
{
  public Image icon;
  public ItemSO result;
  public int resultAmount;
  public CraftingResource[] requirements;
  public float craftTime;

  public void init(CraftingBlueprintSO blueprint)
  {
    icon.sprite = blueprint.result.Icon;
    result = blueprint.result;
    resultAmount = blueprint.resultAmount;
    requirements = blueprint.requirements;
    craftTime = blueprint.craftTime;
  }
}