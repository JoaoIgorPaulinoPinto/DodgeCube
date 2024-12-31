using System.Collections;
using UnityEngine;

[System.Serializable]
public class Spawns
{
    public Transform point;
    public GameObject[] availableWalls;

    public Spawns(Transform _point, GameObject[] _availableWalls)
    {
        point = _point;
        availableWalls = _availableWalls;
    }
}

public class WallsController : MonoBehaviour
{
    public float maxXdif;

    public int dificultLevel = 1;

    public Spawns[] spwns;

    public Transform center;

    public float timewait;
    public bool wait;

    private void Update()
    {
        if (!wait)
        {
            StartCoroutine(Lauch());
        }
        else
        {
            return;
        }
    }

    IEnumerator Lauch()
    {
        wait = true;
        CreateWall();
        yield return new WaitForSeconds(timewait);
        wait = false;
    }

    void CreateWall()
    {
        Spawns selectedSpwn = spwns[Random.Range(0, spwns.Length)];
        GameObject selectedWall = selectedSpwn.availableWalls[Random.Range(0, selectedSpwn.availableWalls.Length)];

        // Determina um canto aleatório em relação ao centro
        Vector3 spawnPosition = selectedSpwn.point.position;

        // Escolhe aleatoriamente o canto
        int randomCorner = Random.Range(0, 4);
        switch (randomCorner)
        {
            case 0: // Canto superior direito
                spawnPosition.x += maxXdif;
                spawnPosition.z += maxXdif;
                break;
            case 1: // Canto superior esquerdo
                spawnPosition.x -= maxXdif;
                spawnPosition.z += maxXdif;
                break;
            case 2: // Canto inferior direito
                spawnPosition.x += maxXdif;
                spawnPosition.z -= maxXdif;
                break;
            case 3: // Canto inferior esquerdo
                spawnPosition.x -= maxXdif;
                spawnPosition.z -= maxXdif;
                break;
        }

        // Instancia a parede na nova posição ajustada
        GameObject createdWall = Instantiate(selectedWall, spawnPosition, selectedWall.transform.rotation);

        // Movê-la em direção ao centro
        MoveWall(createdWall);
    }

    void MoveWall(GameObject wall)
    {
        wall.GetComponent<WallMovimentController>().MoveWall(center);
    }
}
