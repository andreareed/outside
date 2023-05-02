using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
  [SerializeField] ItemSO item;
  [SerializeField] int stackSize;

  // Public getters
  public ItemSO Item => item;
  public int StackSize => stackSize;
}
