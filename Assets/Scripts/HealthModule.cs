using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModule : MonoBehaviour
{
    [Tooltip("Значение очков жизни у модуля")] public int Health;
    [Tooltip("Значение очков брони у модуля")] public int ArmorValue;
    [Tooltip("Цвет модуля,при минимальном количестве жизней у обьекта")]public Color32 MinHealthColor;

    private int HealthMax;
    private void Start()
    {
        HealthMax = Health;  
    }
    float Normalize(float number,float MaxNumber)
    {
        return number / MaxNumber;
    }
    private void Update()
    {
        if (Health < 0)
        {
            GetComponent<MeshRenderer>().material.color = MinHealthColor;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = new Color32(145, System.Convert.ToByte(255  * Normalize(Health, HealthMax)), 0, 0);
        }
    }
}
