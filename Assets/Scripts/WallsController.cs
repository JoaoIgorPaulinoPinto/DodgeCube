using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Spawns
{
    public Transform point;
    public Wall[] availableWalls;

    public Spawns(Transform _point, Wall[] _availableWalls)
    {
        point = _point;
        availableWalls = _availableWalls;
    }
}

[System.Serializable]
public class Wall
{
    public GameObject obj;
    public int levelStart;
    public float wallSpeed;
    public float XmodifyerIndex;
}

[RequireComponent(typeof(NetworkObject))]
public class WallsController : NetworkBehaviour
{
    public float maxXdif;

    public Spawns[] spwns;

    public Transform center;

    public float timewait; 

    public bool wait;

    public float moveSpeedAditionalIndex;
    public float timeWaitReduceIndex;
    public float timeToUpdateDificultLevel;
    public float speedAdded = 0;
    public float timeWaitReduced = 0;
    public int  passedTime= 0;
    private float lastUpdateTime = 0f; // Armazena o último tempo registrado.



    private void Update()
    {
        if (!IsSpawned || !IsOwner)
            return;
        else
        {      
            // Verifica o tempo decorrido desde a última atualização.
            if (Time.time - lastUpdateTime >= timeToUpdateDificultLevel)
            {
                // Incrementa os índices de dificuldade.
                speedAdded += moveSpeedAditionalIndex;
                timeWaitReduced += timeWaitReduceIndex;

                // Incrementa o nível do jogador.
                PlayerSttsManager.instance.level++;

                // Atualiza o tempo da última verificação.
                lastUpdateTime = Time.time;
            }

            // Lógica de spawn.
            if (!wait)
            {
                StartCoroutine(Lauch());
            }
        }
    }
    IEnumerator Lauch()
    {
        wait = true;
        CreateWall();
        yield return new WaitForSeconds(timewait + timeWaitReduced);
        wait = false;
    }



    void CreateWall()
    {
        Spawns selectedSpwn = spwns[Random.Range(0, spwns.Length)];

        Wall[] availableWallsForLevel = System.Array.FindAll(
            selectedSpwn.availableWalls,
            wall => wall.levelStart <= PlayerSttsManager.instance.level
        );

        if (availableWallsForLevel.Length == 0)
        {
            return;
        }

        Wall selectedWallData = availableWallsForLevel[Random.Range(0, availableWallsForLevel.Length)];
        GameObject selectedWall = selectedWallData.obj;

        Vector3 spawnPosition = selectedSpwn.point.position;

        int randomCorner = Random.Range(0, 4);
        switch (randomCorner)
        {
            case 0: // Canto superior direito
                spawnPosition.x += selectedWallData.XmodifyerIndex;
                spawnPosition.z += selectedWallData.XmodifyerIndex;
                break;
            case 1: // Canto superior esquerdo
                spawnPosition.x -= selectedWallData.XmodifyerIndex;
                spawnPosition.z += selectedWallData.XmodifyerIndex;
                break;
            case 2: // Canto inferior direito
                spawnPosition.x += selectedWallData.XmodifyerIndex;
                spawnPosition.z -= selectedWallData.XmodifyerIndex;
                break;
            case 3: // Canto inferior esquerdo
                spawnPosition.x -= selectedWallData.XmodifyerIndex;
                spawnPosition.z -= selectedWallData.XmodifyerIndex;
                break;
        }

        // Instanciar o objeto mantendo toda a hierarquia do prefab
        GameObject createdWall = Instantiate(selectedWall, spawnPosition, selectedWall.transform.rotation);
        createdWall.GetComponent<NetworkObject>().Spawn(true);
        if (createdWall.GetComponent<NetworkObject>().IsSpawned)
        {
            createdWall.transform.SetParent(center);

            createdWall.TryGetComponent<WallMovimentController>(out WallMovimentController wallControler);
            if (wallControler) wallControler.MoveWall(center, selectedWallData.wallSpeed + speedAdded);
        }
     
    }

}
