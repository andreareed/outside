using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
  public enum _ItemType { Generic, Consumable, Weapon }

  [Header("General")]
  [SerializeField] _ItemType itemType;
  [SerializeField] Sprite icon;
  [SerializeField] string itemName;
  [SerializeField] string description;
  [Space]
  [Header("Item Stacking")]
  [SerializeField] bool isStackable = false;
  [SerializeField] int maxStack = 1;

  [Header("Consumable")]
  [SerializeField] float health = 10f;
  [SerializeField] float food = 10f;
  [SerializeField] float water = 10f;

  [Header("Weapon")]
  [SerializeField] float damage = 10f;
  [SerializeField] float range = 10f;
  [SerializeField] float fireRate = 1f;
  [SerializeField] float reloadTime = 1f;
  [SerializeField] int maxAmmo = 10;

  // Public getters
  public _ItemType ItemType => itemType;
  public Sprite Icon => icon;
  public string ItemName => itemName;
  public string Description => description;
  public bool IsStackable => isStackable;
  public int MaxStack => maxStack;

  public float Health => health;
  public float Food => food;
  public float Water => water;

  public float Damage => damage;
  public float Range => range;
  public float FireRate => fireRate;
  public float ReloadTime => reloadTime;
  public int MaxAmmo => maxAmmo;
}