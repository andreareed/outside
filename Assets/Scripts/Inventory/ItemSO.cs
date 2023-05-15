using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
  // This SO uses a customer editor to display the fields in a more organized way.
  // Always update the editor script when making changes

  public enum _ItemType { Generic, Consumable, Weapon }
  public enum _WeaponType { Melee, Ranged }

  // UI Display
  [SerializeField] _ItemType itemType;
  [SerializeField] Sprite icon;
  [SerializeField] string itemName;
  [SerializeField] string description;
  [Space]

  // Stacking
  [SerializeField] bool isStackable = false;
  [SerializeField] int maxStack = 1;

  // Consumable
  [SerializeField] float health = 10f;
  [SerializeField] float food = 10f;
  [SerializeField] float water = 10f;

  // Weapon
  [SerializeField] _WeaponType weaponType;
  [SerializeField] float damage = 10f;
  [SerializeField] bool isRanged = false;
  [SerializeField] float range = 10f;
  [SerializeField] float fireRate = 1f;
  [SerializeField] bool hasAmmo = false;
  [SerializeField] float reloadTime = 1f;
  [SerializeField] int maxAmmo = 10;

  // Required References
  [SerializeField] GameObject model;
  [SerializeField] Vector3 modelPosition;
  [SerializeField] Vector3 modelRotation;

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

  public _WeaponType WeaponType => weaponType;
  public float Damage => damage;
  public float Range => range;
  public float FireRate => fireRate;
  public float ReloadTime => reloadTime;
  public int MaxAmmo => maxAmmo;
  public bool HasAmmo => hasAmmo;
  public bool IsRanged => isRanged;

  public GameObject Model => model;
  public Vector3 ModelPosition => modelPosition;
  public Vector3 ModelRotation => modelRotation;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemSO))]
public class ItemSOEditor : Editor
{

  public override void OnInspectorGUI()
  {
    EditorGUILayout.Space();
    GUILayout.Label("Inventory Item", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 18 }, GUILayout.ExpandWidth(true));
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    EditorGUILayout.Space();

    GUILayout.Label("UI Display", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 14 }, GUILayout.ExpandWidth(true));
    EditorGUILayout.Space();
    EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));

    if (serializedObject.FindProperty("icon").objectReferenceValue != null)
    {
      EditorGUILayout.Space();
      GUILayout.Label("Icon Preview:", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = 12 });
      EditorGUILayout.Space();
      Texture2D texture = AssetPreview.GetAssetPreview(serializedObject.FindProperty("icon").objectReferenceValue);

      GUILayout.Label("", GUILayout.Height(60), GUILayout.Width(60));
      GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    EditorGUILayout.Space();

    GUILayout.Label("Stacking", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 14 }, GUILayout.ExpandWidth(true));
    EditorGUILayout.Space();
    EditorGUILayout.PropertyField(serializedObject.FindProperty("isStackable"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("maxStack"));
    EditorGUILayout.Space();
    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    EditorGUILayout.Space();

    GUILayout.Label("Item Effects", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 14 }, GUILayout.ExpandWidth(true));
    EditorGUILayout.Space();
    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
    EditorGUILayout.Space();

    int selectedItemType = serializedObject.FindProperty("itemType").intValue;

    switch (selectedItemType)
    {
      case 1:
        DisplayConsumableInfo();
        break;

      case 2:
        DisplayWeaponInfo();
        break;

    }
    serializedObject.ApplyModifiedProperties();
  }

  void DisplayConsumableInfo()
  {

    EditorGUILayout.PropertyField(serializedObject.FindProperty("health"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("food"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("water"));
  }

  void DisplayWeaponInfo()
  {
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
    EditorGUILayout.PropertyField(serializedObject.FindProperty("model"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("modelPosition"));
    EditorGUILayout.PropertyField(serializedObject.FindProperty("modelRotation"));
  }
}

#endif