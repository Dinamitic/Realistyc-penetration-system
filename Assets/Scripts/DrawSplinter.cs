using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSplinter : MonoBehaviour
{
    public bool LineRenderMode;
    public Material lineMat;
    public PenetrationSystem penetrationSystem;
    public float[] distance = new float[0];
    private int[] pont = new int[0];
    Vector3 current;
    public void Draw(Vector3 begin, Vector3 end)
    {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(Color.red);
        GL.Vertex3(begin.x, begin.y, begin.z );
        GL.Vertex3(end.x, end.y, end.z);
        GL.End();
    }
    private bool s = true;
    private void OnPostRender()
    {
        if (LineRenderMode && penetrationSystem != null)
        {
            if (s)
            {
                s = false;
                System.Array.Resize(ref pont, penetrationSystem.Points.Count);
                System.Array.Resize(ref distance,penetrationSystem.Points.Count) ;
                for (int i = 0;i < pont.Length;i++)
                {
                    pont[i] = 1;
                    distance[i] = 0.001f;
                }
                current = penetrationSystem.Points[0][0];
            }
            for (int i = 0;i < penetrationSystem.Points.Count;i++)
            {
                for (int x = 0;x < pont[i];x++)
                {
                    current = Vector3.MoveTowards(penetrationSystem.Points[i][x], penetrationSystem.Points[i][x + 1], distance[i]);
                    distance[i] += 0.5f * Time.deltaTime;
                    if (current == penetrationSystem.Points[i][x + 1] && pont[i] < penetrationSystem.Points[i].Count - 1)
                    {
                        pont[i]++;
                        distance[i] = 0.01f;
                    }  
                    if (x == pont[i] - 1)
                    {
                        Draw(penetrationSystem.Points[i][x], current);
                    }
                    else
                    {
                        Draw(penetrationSystem.Points[i][x], penetrationSystem.Points[i][x + 1]);
                    }
                }
            }
        }
    }
    void Update()
    {
        
    }
}