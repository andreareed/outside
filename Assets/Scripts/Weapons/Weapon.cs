using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum _AttackType { heavySwing }

public class Weapon : MonoBehaviour
{
  [SerializeField] private _AttackType attackType;
  [SerializeField] float damage;
  [SerializeField] bool isRanged;
  [SerializeField] float range;
  [SerializeField] float fireRate;
  [SerializeField] bool hasAmmo;
  [SerializeField] float reloadTime;
  [SerializeField] int maxAmmo;
  [SerializeField] DamageModifier[] damageModifiers;

  private float timer = 0f;

  // Public setters & getters
  public _AttackType AttackType => attackType;
  public float Damage { get => damage; set => damage = value; }
  public float Range { get => range; set => range = value; }
  public float FireRate { get => fireRate; set => fireRate = value; }
  public float ReloadTime { get => reloadTime; set => reloadTime = value; }
  public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
  public bool HasAmmo { get => hasAmmo; set => hasAmmo = value; }
  public bool IsRanged { get => isRanged; set => isRanged = value; }
  public DamageModifier[] DamageModifiers { get => damageModifiers; set => damageModifiers = value; }

  private void Update()
  {
    if (timer > 0f)
    {
      timer -= Time.deltaTime;

      if (timer <= 0f)
      {
        timer = 0f;

      }
    }
  }

  public void Attack(Animator animator)
  {
    if (timer > 0f)
    {
      return;
    }
    animator.SetTrigger(attackType.ToString());
    timer = fireRate;
  }

  public float GetDamageModifier(string itemTag)
  {
    DamageModifier modifier = Array.Find(damageModifiers, mod => mod.tag == itemTag);
    return modifier != null ? modifier.modifier : 0f;
  }


  private void OnTriggerEnter(Collider other)
  {
    bool isTerrainTree = other.gameObject.layer == LayerMask.NameToLayer("Terrain");
    float modifier = GetDamageModifier(isTerrainTree ? "Tree" : other.gameObject.tag);

    other.gameObject.SendMessage("ApplyDamage", damage * modifier);
  }
}




#if UNITY_EDITOR
[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{

  public override void OnInspectorGUI()
  {
    EditorGUILayout.Space();
    GUILayout.Label("Weapon Stats", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 18 }, GUILayout.ExpandWidth(true));
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    EditorGUILayout.Space();

    EditorGUILayout.PropertyField(serializedObject.FindProperty("attackType"));
    EditorGUILayout.Space();

    EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("isRanged"));

    if (serializedObject.FindProperty("isRanged").boolValue)
    {
      EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
    }

    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireRate"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("hasAmmo"));

    if (serializedObject.FindProperty("hasAmmo").boolValue)
    {
      EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadTime"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
    }

    EditorGUILayout.PropertyField(serializedObject.FindProperty("damageModifiers"));

    serializedObject.ApplyModifiedProperties();
  }
}

#endif