using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform[] Armor = new Transform[0];
    public Transform[] Module = new Transform[0];
    public Material ArmorBaseMat;
    public Material ModuleBaseMat;
    public Material ArmorEffectMat;
    public Material ModuleEffectMat;
    /// <summary>
    /// Включает эффект пробития у танка
    /// </summary>
    public void OnEffectBreaking()
    {
        for (int i = 0;i < Armor.Length;i++)
        {
            Armor[i].GetComponent<MeshRenderer>().material = ArmorEffectMat;
        }
        for (int i = 0;i < Module.Length;i++)
        {
            Module[i].GetComponent<MeshRenderer>().material = ModuleEffectMat;
        }
    }
    /// <summary>
    /// Выключает эффект пробития у танка
    /// </summary>
    public void OffEffectBreaking()
    {
        for (int i = 0; i < Armor.Length; i++)
        {
            Armor[i].GetComponent<MeshRenderer>().material = ArmorBaseMat;
        }
        for (int i = 0;i < Module.Length;i++)
        {
            Module[i].GetComponent<MeshRenderer>().material = ModuleBaseMat;
        }
    }
}
