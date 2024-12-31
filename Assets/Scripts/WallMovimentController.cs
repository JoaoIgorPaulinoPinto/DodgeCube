using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovimentController : MonoBehaviour
{
    public float wallspeed;
    public bool IsMoving;
    public Transform center;
    private Vector3 dir;

    public void MoveWall(Transform center)
    {
        this.center = center;
        IsMoving = true;

        // Calcular direção limitada a um eixo (X ou Z)
        Vector3 difference = center.position - transform.position;
        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.z))
        {
            // Mover no eixo X
            dir = new Vector3(difference.x, 0, 0).normalized;
        }
        else
        {
            // Mover no eixo Z
            dir = new Vector3(0, 0, difference.z).normalized;
        }
    }

    private void Update()
    {
        if (IsMoving && center != null)
        {
            // Aplicar velocidade ao Rigidbody na direção calculada
            GetComponent<Rigidbody>().velocity = dir * wallspeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
