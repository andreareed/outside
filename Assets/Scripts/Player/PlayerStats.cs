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
  [SerializeField] float staminaDrain = 10f;
  public float stamina;
  [Space]
  [Header("Hunger Settings")]
  [SerializeField] float maxFood = 100f;
  [SerializeField] float foodDrain = .1f;
  private float food;
  [Space]
  [Header("Thirst Settings")]
  [SerializeField] float maxWater = 100f;
  [SerializeField] float waterDrain = .1f;
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
    DrainStats();
    UpdateStatsUI();
  }

  private void DrainStats()
  {
    if (food > 0)
    {
      UpdateFood(-foodDrain * Time.deltaTime);
    }
    else
    {
      UpdateHealth(-foodDrain * Time.deltaTime);
    }

    if (water > 0)
    {
      UpdateWater(-waterDrain * Time.deltaTime);
    }
    else
    {
      UpdateHealth(-waterDrain * Time.deltaTime);
    }
  }

  private void UpdateStatsUI()
  {
    healthBar.bar.fillAmount = health / maxHealth;
    staminaBar.bar.fillAmount = stamina / maxStamina;
    hungerBar.bar.fillAmount = food / maxFood;
    thirstBar.bar.fillAmount = water / maxWater;
    weightBar.bar.fillAmount = weight / maxWeight;
  }

  private void NormalizeStats()
  {
    health = Mathf.Clamp(health, 0, maxHealth);
    stamina = Mathf.Clamp(stamina, 0, maxStamina);
    food = Mathf.Clamp(food, 0, maxFood);
    water = Mathf.Clamp(water, 0, maxWater);
    // Don't clamp weight, can go over max
    if (weight < 0)
    {
      weight = 0;
    }
  }

  public float GetStaminaDrain()
  {
    return staminaDrain;
  }
  public float GetFoodDrain()
  {
    return foodDrain;
  }
  public float GetWaterDrain()
  {
    return waterDrain;
  }

  public void UpdateHealth(float amount)
  {
    health += amount;
  }
  public void UpdateStamina(float amount)
  {
    stamina += amount;
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
