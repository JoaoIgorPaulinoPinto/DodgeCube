using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovimentController : MonoBehaviour
{
    public float wallspeed;
    public bool IsMoving;
    public Transform center;
    private Vector3 dir;

    // Conjunto para armazenar objetos já detectados
    private HashSet<Transform> detectedObjects = new HashSet<Transform>();

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
        RayDetection();
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

    bool VerifyPlayerDetected(Transform target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Verificar se o objeto já foi detectado
            if (!detectedObjects.Contains(target))
            {
                detectedObjects.Add(target); // Adicionar ao conjunto
                PlayerSttsManager.instance.AddPoint();
                return true;
            }
        }
        return false;
    }

    void RayDetection()
    {
        if (transform.localScale.x > transform.localScale.y)
        {
            RaycastHit hit;
            Vector3 rayorigin = new Vector3((transform.position.x + transform.localScale.x / 2), transform.position.y, transform.position.z);
            Vector3 rayorigin2 = new Vector3((transform.position.x - transform.localScale.x / 2), transform.position.y, transform.position.z);

            if (IsMoving)
            {
                // Raio para a direita
                if (Physics.Raycast(rayorigin, Vector3.right, out hit, 10))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        return;
                    }
                }

                // Raio para a esquerda
                if (Physics.Raycast(rayorigin2, Vector3.left, out hit, 10))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        return;
                    }
                }
            }
        }
        else
        {
            RaycastHit hit;
            Vector3 rayorigin = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z / 2);
            Vector3 rayorigin2 = new Vector3(transform.position.x, transform.position.y, transform.position.z - transform.localScale.z / 2);

            if (IsMoving)
            {
                // Raio para frente (forward)
                if (Physics.Raycast(rayorigin, Vector3.forward, out hit, 10))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        return;
                    }
                }

                // Raio para trás (back)
                if (Physics.Raycast(rayorigin2, Vector3.back, out hit, 10))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        return;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (transform.localScale.x > transform.localScale.y)
        {
            Vector3 rayorigin = new Vector3((transform.position.x + transform.localScale.x / 2), transform.position.y, transform.position.z);
            Vector3 rayorigin2 = new Vector3((transform.position.x - transform.localScale.x / 2), transform.position.y, transform.position.z);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayorigin, Vector3.right * 10); // Raio para a direita
            Gizmos.DrawRay(rayorigin2, Vector3.left * 10); // Raio para a esquerda
        }
        else
        {
            Vector3 rayorigin = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z / 2);
            Vector3 rayorigin2 = new Vector3(transform.position.x, transform.position.y, transform.position.z - transform.localScale.z / 2);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(rayorigin, Vector3.forward * 10); // Raio para frente (forward)
            Gizmos.DrawRay(rayorigin2, Vector3.back * 10);   // Raio para trás (back)
        }
    }
}
