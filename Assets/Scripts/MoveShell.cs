using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShell : MonoBehaviour
{
    public int Strong = 100;//Сила удара по снаряду
    public Vector3 Direction;//Напрвление полёта снаряда

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Strong * Direction);
    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Direction);
    }
}
