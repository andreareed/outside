using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
  [Header("Slot Data")]
  private ItemSO item;
  private int stackSize;
  [Space]
  [Header("Required References")]
  [SerializeField] Image icon;
  [SerializeField] Text stackText;
  private bool isEmpty;

  // Public getters
  public bool IsEmpty => isEmpty;
  public ItemSO Item => item;
  public int StackSize => stackSize;

  private void Start()
  {
    UpdateSlot();
  }

  public void UpdateSlot()
  {
    if (item == null)
    {
      isEmpty = true;
      icon.gameObject.SetActive(false);
      stackText.gameObject.SetActive(false);
    }
    else
    {
      isEmpty = false;
      icon.gameObject.SetActive(true);
      stackText.gameObject.SetActive(true);
      icon.sprite = item.Icon;
    }
  }

  public void AddItemToSlot(ItemSO data_, int stackSize_)
  {
    item = data_;
    stackSize = stackSize_;
    stackText.text = $"{stackSize}";
    UpdateSlot();
  }
}