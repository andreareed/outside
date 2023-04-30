using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
  public enum ItemType { Generic, Consumable, Weapon }

  [Header("General")]
  private ItemType itemType;
  private Sprite icon;
  private string itemName;
  private string description;
  [Space]
  [Header("Item Stacking")]
  private bool isStackable = false;
  private int maxStack = 1;


  // Public getters
  public ItemType Type => itemType;
  public Sprite Icon => icon;
  public string ItemName => itemName;
  public string Description => description;
  public bool IsStackable => isStackable;
  public int MaxStack => maxStack;
}