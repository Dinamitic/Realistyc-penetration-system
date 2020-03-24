using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 Cube1, Cube2, Cube3;
    public Vector3 SizeCube;
    public bool DebugMode;
    public float Speed;

    public Vector3 TankPosition;
    public Vector3 Distance;//Дистанция от танка,до точки 
    public GameObject Text;//Текст "Есть пробитие"

    private bool EndCalculate;
    private List<Vector3> Curva = new List<Vector3>();//Точки кривой Безье

    private int Target;
    void SetPositionCube()
    {
        Cube1 = new Vector3(TankPosition.x - Distance.x, TankPosition.y + Distance.y, TankPosition.z);
        Cube3 = new Vector3(TankPosition.x + Distance.x, TankPosition.y + Distance.y, TankPosition.z);
        Cube2 = new Vector3(TankPosition.x, TankPosition.y + Distance.y, TankPosition.z - Distance.z);
    }
    void Start()
    {
        SetPositionCube();
        transform.position = Cube1;
        transform.LookAt(TankPosition);
    }
    public void CalculateCurva()
    {
        Curva.Clear();
        for (float i = 0;i <= 1f;i += 0.07f)
        {
            Curva.Add( Mathf.Pow(1 - i,2) * Cube1 + 2 * i * (1 - i) * Cube2 + Mathf.Pow(i , 2) * Cube3 );
        }
        EndCalculate = true;
    }
    private void OnDrawGizmosSelected()
    {
        if (DebugMode)
        {
            Gizmos.DrawCube(Cube1, SizeCube);
            Gizmos.DrawCube(Cube2, SizeCube);
            Gizmos.DrawCube(Cube3, SizeCube);
            for (int i = 0;i < Curva.Count - 1;i++)
            {
                Debug.DrawLine(Curva[i],Curva[i + 1]);
            }
        }
    }
    void Update()
    {
        if (EndCalculate)
        {
            Move();
        }
    }
    float GetDistance(Vector3 Player,Vector3 Target)
    {
        return Vector3.Distance(Player,Target);
    }
    void Move()
    {
        if (GetDistance(gameObject.transform.position,Curva[Target]) < 0.1f && Target < Curva.Count - 1)
        {
            Target++;
        }
        if (Target == Curva.Count - 2)
        {
            //Text.SetActive(true);
        }
        transform.LookAt(TankPosition);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,Curva[Target],Speed);
    }
}
