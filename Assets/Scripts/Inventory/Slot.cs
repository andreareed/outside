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
  private Image icon;
  private Text stackText;

  private bool isEmpty;
  public bool IsEmpty => isEmpty;

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
      icon.sprite = item.Icon;
    }
  }

  public void AddItemToSlot(ItemSO data_, int stackSize_)
  {
    item = data_;
    stackSize = stackSize_;

    if (item.IsStackable && stackSize < item.MaxStack)
    {
      stackSize++;
      stackText.text = stackSize.ToString();
    }
  }
}