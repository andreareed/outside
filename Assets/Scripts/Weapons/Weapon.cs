using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _AttackType { heavySwing }

public class Weapon : MonoBehaviour
{

  [SerializeField] private _AttackType attackType;


  public void Attack(Animator animator)
  {
    animator.SetTrigger(attackType.ToString());
  }

}
