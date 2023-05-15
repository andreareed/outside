using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
  [SerializeField] float resourceHealth = 100f;
  [SerializeField] GameObject harvestedModel;
  [SerializeField] float respawnDelayMinutes = 15f;

  private void Harvest()
  {
    Destroy(gameObject);
  }

  private void UpdateHealth(float damage)
  {
    resourceHealth -= damage;
    if (resourceHealth <= 0)
    {
      Harvest();
    }
  }


}
