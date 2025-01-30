using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class WallMovimentController : MonoBehaviour
{
    public float wallspeed;
    public bool IsMoving;
    public Transform center;
    private Vector3 dir;

    Rigidbody body;
    private void Start()
    {
       
        body = GetComponent<Rigidbody>();   
    }

    // Conjunto para armazenar objetos já detectados
    private HashSet<Transform> detectedObjects = new HashSet<Transform>();

    public void MoveWall(Transform center, float _wallspeed)
    {
        this.wallspeed = _wallspeed;
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

            // Aplicar velocidade ao Rigidbody na direção calculada
            body.linearVelocity = dir * wallspeed;

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
        RaycastHit hit;
        // Verificar direção principal do movimento (X ou Z)
        if (Mathf.Abs(transform.eulerAngles.y) < 90 || Mathf.Abs(transform.eulerAngles.y - 360) < 90)
        {
            // Raios para a direita e esquerda (eixo X)
            Vector3 rayOriginRight = new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z);
            Vector3 rayOriginLeft = new Vector3(transform.position.x + .5f, transform.position.y, transform.position.z);

            if (IsMoving)
            {
                // Raio para a direita
                if (Physics.Raycast(rayOriginRight, Vector3.right, out hit, 10, 1 << LayerMask.NameToLayer("Player")))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        print("COLIDIIU"); return;
                    }
                }

                // Raio para a esquerda
                if (Physics.Raycast(rayOriginLeft, Vector3.left, out hit, 10, 1 << LayerMask.NameToLayer("Player")))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        print("COLIDIIU"); return;
                    }
                }
            }
        }
        else
        {
            // Raios para frente e para trás (eixo Z)
            Vector3 rayOriginForward = new Vector3(transform.position.x, transform.position.y, transform.position.z - .5f);
            Vector3 rayOriginBack = new Vector3(transform.position.x, transform.position.y, transform.position.z + .5f);

            if (IsMoving)
            {
                // Raio para frente
                if (Physics.Raycast(rayOriginForward, Vector3.forward, out hit, 10, 1 << LayerMask.NameToLayer("Player")))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        print("COLIDIIU"); return;
                    }
                }

                // Raio para trás
                if (Physics.Raycast(rayOriginBack, Vector3.back, out hit, 10, 1 << LayerMask.NameToLayer("Player")))
                {
                    if (VerifyPlayerDetected(hit.transform))
                    {
                        print("COLIDIIU"); return;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Verificar direção principal do movimento (X ou Z)
        if (Mathf.Abs(transform.eulerAngles.y) < 90 || Mathf.Abs(transform.eulerAngles.y - 360) < 90)
        {
            // Gizmos para a direção X
            Vector3 rayOriginRight = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 rayOriginLeft = new Vector3(transform.position.x , transform.position.y, transform.position.z);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayOriginRight, Vector3.right * 10); // Raio para a direita
            Gizmos.DrawRay(rayOriginLeft, Vector3.left * 10);   // Raio para a esquerda
        }
        else
        {   
            // Gizmos para a direção Z
            Vector3 rayOriginForward = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 rayOriginBack = new Vector3(transform.position.x, transform.position.y, transform.position.z );

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(rayOriginForward, Vector3.forward * 10); // Raio para frente
            Gizmos.DrawRay(rayOriginBack, Vector3.back * 10);      // Raio para trás
        }
    }
}
