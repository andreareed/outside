using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
  [SerializeField] ItemSO item;
  [SerializeField] int stackSize;

  // Public setters & getters
  public ItemSO Item { get => item; set => item = value; }
  public int StackSize { get => stackSize; set => stackSize = value; }
}
