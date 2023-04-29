using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
  [Header("Health Settings")]
  [SerializeField] float maxHealth = 100f;
  private float health;
  [Space]
  [Header("Stamina Settings")]
  [SerializeField] float maxStamina = 100f;
  public float stamina;
  [Space]
  [Header("Hunger Settings")]
  [SerializeField] float maxFood = 100f;
  private float food;
  [Space]
  [Header("Thirst Settings")]
  [SerializeField] float maxWater = 100f;
  private float water;
  [Space]
  [Header("Weight Settings")]
  [SerializeField] float maxWeight = 100f;
  private float weight;
  [Space]
  [Header("UI")]
  [SerializeField] StatBar healthBar;
  [SerializeField] StatBar staminaBar;
  [SerializeField] StatBar hungerBar;
  [SerializeField] StatBar thirstBar;
  [SerializeField] StatBar weightBar;


  private void Start()
  {
    health = maxHealth;
    stamina = maxStamina;
    food = maxFood;
    water = maxWater;
    weight = 0;
  }

  private void Update()
  {
    NormalizeStats();
    UpdateStats();
  }

  private void UpdateStats()
  {
    healthBar.bar.fillAmount = health / maxHealth;
    staminaBar.bar.fillAmount = stamina / maxStamina;
    hungerBar.bar.fillAmount = food / maxFood;
    thirstBar.bar.fillAmount = water / maxWater;
    weightBar.bar.fillAmount = weight / maxWeight;
  }

  private void NormalizeStats()
  {
    Mathf.Clamp(health, 0, maxHealth);
    Mathf.Clamp(stamina, 0, maxStamina);
    Mathf.Clamp(food, 0, maxFood);
    Mathf.Clamp(water, 0, maxWater);
    // Don't clamp weight, can go over max
    if (weight < 0)
    {
      weight = 0;
    }
  }

  public void UpdateHealth(float amount)
  {
    health += amount;
  }
  public void UpdateStamina(float amount)
  {
    stamina += amount;
    Debug.Log("Stamina: " + stamina);
  }
  public void UpdateFood(float amount)
  {
    food += amount;
  }
  public void UpdateWater(float amount)
  {
    water += amount;
  }
  public void UpdateWeight(float amount)
  {
    weight += amount;
  }
}
