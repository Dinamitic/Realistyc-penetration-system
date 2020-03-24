using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrationSystem : MonoBehaviour
{
    [Header("Свойства снаряда")]
    [Tooltip("Радиус взрывной волны")] public float RaduisBlast;
    [Tooltip("Радиус разлёта осколков")] public float RadiusExpansion;
    [Tooltip("Количество осколков в снаряде")] public int CountSplinter;
    [Tooltip("Значение пробития осколка")] public float PinetrationSplinter;
     public int CurrentIteration { get; private set; }
    [Tooltip("Направление центра полёта осколков")] public Vector3 DirectionSplenter;

    public Transform konus; //Ссылка на обьект,с CircleCollider2D.Нужен для установки радиуса разброса осколков

    [Tooltip("Количество возможного количества рикошета осколков")] public int Iteration = 4;
    [HideInInspector] public Manager manager;
    [HideInInspector] public List<Vector3> Direction = new List<Vector3>();//Массив направлений для осколков
    [HideInInspector] public List<List<Vector3>> Points = new List<List<Vector3>>();//Массив точек у рейкаста
    public bool DebugMode;

    void Start()
    {
        CurrentIteration = 1;
        GetComponent<SphereCollider>().radius = RaduisBlast;
        SetRandomDirection(ref Direction, CountSplinter);
        Penetration();
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2.2f);
        manager.OffEffectBreaking();
    }
    /// <summary>
    /// Устанавливает случайные направления для осколков.
    /// Принимает направление и кол-во осколков
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="Length"></param>
    public void SetRandomDirection(ref List<Vector3> dir, int Length)
    {
        float radius = konus.GetComponent<CircleCollider2D>().radius;
        for (int i = 0; i < Length; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 heading = new Vector3(
                konus.transform.position.x + (radius * Mathf.Cos(angle)),
                konus.transform.position.y + (radius * Mathf.Sin(angle)),
                konus.transform.position.z) - transform.position;
            Vector3 direction = heading / Vector3.Distance(transform.position, heading);
            direction.z = transform.InverseTransformPoint(konus.transform.position).z;
            dir.Add(direction);
        }
    }
    
    private void OnDrawGizmos()//Рисует линии в Scene
    {
        if (DebugMode)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                for (int x = 0; x < Points[i].Count - 1; x++)
                {
                    switch (x)
                    {
                        case 0:
                            Debug.DrawLine(Points[i][x], Points[i][x + 1], Color.blue);
                            break;
                        case 1:
                            Debug.DrawLine(Points[i][x], Points[i][x + 1], Color.green);
                            break;
                        case 2:
                            Debug.DrawLine(Points[i][x], Points[i][x + 1], Color.red);
                            break;
                        case 3:
                            Debug.DrawLine(Points[i][x], Points[i][x + 1], Color.black);
                            break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Возвращает отраженное направление
    /// </summary>
    /// <param name="StartPos"></param>
    /// <param name="Target"></param>
    /// <param name="Normal"></param>
    /// <returns></returns>
    Vector3 GetReflectDirection(Vector3 StartPos, Vector3 Target, Vector3 Normal)
    {
        Vector3 heading = Target - StartPos;
        Vector3 direction = heading / Vector3.Distance(StartPos, heading);
        return Vector3.Reflect(direction, Normal);
    }
    /// <summary>
    /// Возвращает направление
    /// </summary>
    /// <param name="StartPos"></param>
    /// <param name="Target"></param>
    /// <returns></returns>
    Vector3 GetDirection(Vector3 StartPos, Vector3 Target)
    {
        Vector3 heading = Target - StartPos;
        Vector3 direction = heading / Vector3.Distance(StartPos, heading);
        return direction;
    }
    /// <summary>
    /// Отвечает за всю логику рейкаста
    /// </summary>
    void Penetration()
    {
        RaycastHit hit = default;
        Vector3 normal;
        for (int i = 0; i < Direction.Count; i++)
        {
            if (Physics.Raycast(transform.position, Direction[i], out hit, RadiusExpansion, ~(1 << 8)))
            {
                Points.Add(new List<Vector3>());
                Points[Points.Count - 1].Add(transform.position);
                Points[Points.Count - 1].Add(hit.point);
            start:
                if (hit.collider.gameObject.CompareTag("Armor") && Random.Range(0f,1f) < 0.7f)
                {
                    normal = hit.normal;
                    if (Physics.Raycast(Points[Points.Count - 1][CurrentIteration], GetReflectDirection(Points[Points.Count - 1][CurrentIteration - 1], Points[Points.Count - 1][CurrentIteration], normal), out hit, RadiusExpansion, ~(1 << 8)) && CurrentIteration < 4)
                    {
                        CurrentIteration++;
                        Points[Points.Count - 1].Add(hit.point);
                        goto start;
                    }
                }
                if (Physics.Raycast(Points[Points.Count - 1][CurrentIteration], GetDirection(Points[Points.Count - 1][CurrentIteration - 1], Points[Points.Count - 1][CurrentIteration]), out hit, RadiusExpansion, ~(1 << 8)) && CurrentIteration < 4)
                {
                    if (hit.collider.gameObject.CompareTag("Module"))
                    {
                        if (hit.collider.gameObject.GetComponent<HealthModule>().ArmorValue < PinetrationSplinter)
                        {
                            hit.collider.gameObject.GetComponent<HealthModule>().Health -= 30;
                            if (Random.Range(0f,1f) < 0.7f)
                            {
                                if (Physics.Raycast(hit.point, GetDirection(Points[Points.Count - 1][CurrentIteration - 1], Points[Points.Count - 1][CurrentIteration]), out hit, RadiusExpansion, ~(1 << 8))) { }
                                Points[Points.Count - 1].Add(hit.point);
                                CurrentIteration++;
                                goto start;
                            }
                            else
                            {
                                Points[Points.Count - 1].Add(hit.point);
                            }
                        }
                    }
                }
            }
            CurrentIteration = 1;
        }
    }
}