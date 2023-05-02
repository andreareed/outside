using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
  public enum ItemType { Generic, Consumable, Weapon }

  [Header("General")]
  [SerializeField] ItemType itemType;
  [SerializeField] Sprite icon;
  [SerializeField] string itemName;
  [SerializeField] string description;
  [Space]
  [Header("Item Stacking")]
  [SerializeField] bool isStackable = false;
  [SerializeField] int maxStack = 1;

  [Header("Consumable")]
  [SerializeField] float healthAmount = 10f;
  [SerializeField] float foodAmount = 10f;
  [SerializeField] float waterAmount = 10f;

  [Header("Weapon")]
  [SerializeField] float damage = 10f;
  [SerializeField] float range = 10f;
  [SerializeField] float fireRate = 1f;
  [SerializeField] float reloadTime = 1f;
  [SerializeField] int maxAmmo = 10;






  // Public getters
  public ItemType Type => itemType;
  public Sprite Icon => icon;
  public string ItemName => itemName;
  public string Description => description;
  public bool IsStackable => isStackable;
  public int MaxStack => maxStack;
}