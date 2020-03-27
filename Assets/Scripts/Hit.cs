using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public GameObject Text;//Текст "Есть пробитие"
    public GameObject Pinetartion;
    [Tooltip("Значение пробития снаряда")] public float PinetrationValue;
    public AudioSource audio;
    private float ContactAngle;//Угол контакта с бронёй
    private Transform Armor;//Лист брони,с который столкнулся снаряд
    private Manager manager;
    private CameraMove moveCamera;
    private bool StartCalculate;
    private void Start()
    {
        moveCamera = Camera.main.GetComponent<CameraMove>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Armor"))
        {
           if (!StartCalculate)
           {
                print(other.transform.tag);
                StartCalculate = true;
                Armor = other.transform;
                CalculateHit();
           }
       }
    }
    void CalculateHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1))
        {
            ContactAngle = Vector3.Angle(hit.normal, transform.forward);
            Debug.Log(ContactAngle);
            if (IsPenetration())
            {
                StartCoroutine(TimerDestroy());
            }
            else
            {
                print("Броня не пробита");
                Destroy(gameObject);
            }
        }
    }
    bool IsPenetration()//Упрощённая реализация системы пробития.Не имеет ничего общего с рельной
    {
        if (Armor != null)
        {
            if (ContactAngle < 90 && PinetrationValue < Armor.GetComponent<Armor>().ArmorScore * 3)
            {
                return false;
            }         
            else if (ContactAngle > 90 && PinetrationValue < Armor.GetComponent<Armor>().ArmorScore * 2)
            {
                return false;
            }
            else if (ContactAngle == 90 && PinetrationValue > Armor.GetComponent<Armor>().ArmorScore)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    IEnumerator TimerDestroy()
    {
        audio.Play();
        Text.SetActive(true);//Включается текст "есть пробитие"
        manager = Armor.parent.parent.GetComponent<Manager>();
        yield return new WaitForSeconds(0.3f);
        moveCamera.CalculateCurva();//Камера начинает двигаться вокруг танка
        Time.timeScale = 0.3f;
        manager.OnEffectBreaking();
        Transform System = Instantiate(Pinetartion,transform.position,Quaternion.identity).transform;
        Camera.main.GetComponent<DrawSplinter>().penetrationSystem = System.GetComponent<PenetrationSystem>();
        System.GetComponent<PenetrationSystem>().manager = manager;
        Destroy(gameObject);
    }
}
