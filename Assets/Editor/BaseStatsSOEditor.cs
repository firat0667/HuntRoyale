using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseStatsSO))]
public class BaseStatsSOEditor : Editor
{
    SerializedProperty attackType;

    SerializedProperty maxHP;

    SerializedProperty attackDamage;
    SerializedProperty attackRate;
    SerializedProperty detectionRange;
    SerializedProperty attackAngle;

    SerializedProperty attackStartRange;
    SerializedProperty attackHitRange;

    SerializedProperty projectileSpeed;
    SerializedProperty projectilePierce;
    SerializedProperty projectilePrefab;


    SerializedProperty moveSpeed;
    SerializedProperty moveAttackSpeedMult;

    SerializedProperty rotationSpeed;

    private void OnEnable()
    {
        attackType = serializedObject.FindProperty("attackType");

        maxHP = serializedObject.FindProperty("maxHP");

        attackDamage = serializedObject.FindProperty("attackDamage");
        attackRate = serializedObject.FindProperty("attackRate");
        detectionRange = serializedObject.FindProperty("detectionRange");
        attackAngle = serializedObject.FindProperty("attackAngle");


        attackStartRange = serializedObject.FindProperty("attackStartRange");
        attackHitRange = serializedObject.FindProperty("attackHitRange");

        projectileSpeed = serializedObject.FindProperty("projectileSpeed");
        projectilePierce = serializedObject.FindProperty("projectilePierce");
        projectilePrefab = serializedObject.FindProperty("projectilePrefab");

        moveSpeed = serializedObject.FindProperty("moveSpeed");
        moveAttackSpeedMult = serializedObject.FindProperty("moveAttackSpeedMult");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawAttackType();
        DrawHealth();
        DrawCombatCommon();
        DrawAttackSpecific();
        DrawMovement();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawAttackType()
    {
        EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(attackType);
        EditorGUILayout.Space(4);
    }

    void DrawHealth()
    {
        EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(maxHP);
        EditorGUILayout.Space(6);
    }

    void DrawCombatCommon()
    {
        EditorGUILayout.LabelField("Combat (Common)", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(attackDamage);
        EditorGUILayout.PropertyField(attackRate);
        EditorGUILayout.PropertyField(detectionRange);
        EditorGUILayout.PropertyField(attackAngle);

        EditorGUILayout.Space(6);
    }

    void DrawAttackSpecific()
    {
        AttackType type = (AttackType)attackType.enumValueIndex;

        if (type == AttackType.Melee)
        {
            EditorGUILayout.LabelField("Melee Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(attackStartRange);
            EditorGUILayout.PropertyField(attackHitRange);
        }
        else if (type == AttackType.Ranged)
        {
            EditorGUILayout.LabelField("Ranged Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(projectileSpeed);
            EditorGUILayout.PropertyField(projectilePierce);
            EditorGUILayout.PropertyField(projectilePrefab);
        }

        EditorGUILayout.Space(6);
    }

    void DrawMovement()
    {
        EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(moveSpeed);
        EditorGUILayout.PropertyField(moveAttackSpeedMult);

        EditorGUILayout.Space(6);

        EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(rotationSpeed);
    }
}
