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


    public Spawns[] spwns;

    public Transform center;

    public float timewait; // Tempo de espera entre as gerações de paredes
    public bool wait;

    // Modificadores de dificuldade
    public float wallSpeedIncrease = 0.5f;  // Aumento de velocidade das paredes
    public float timeWaitDecrease = 0.2f;   // Diminuição do tempo entre o surgimento das paredes
    public float difficultyIncreaseInterval = 30f; // Intervalo de tempo (em segundos) para aumentar a dificuldade

    private float difficultyTimer = 0f;  // Timer para controlar o aumento de dificuldade

    private void Update()
    {
        // Atualiza o timer de dificuldade
        difficultyTimer += Time.deltaTime;

        // Verifica se chegou o momento de aumentar a dificuldade
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;  // Reseta o timer após o aumento de dificuldade
        }

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
        // Modifica a velocidade das paredes com base no nível de dificuldade
        WallMovimentController wallController = wall.GetComponent<WallMovimentController>();
        wallController.wallspeed += PlayerSttsManager.instance.level * wallSpeedIncrease;  // Aumenta a velocidade das paredes

        wallController.MoveWall(center);
    }

    // Método para aumentar a dificuldade
    void IncreaseDifficulty()
    {
        // Aumenta o nível de dificuldade
        PlayerSttsManager.instance.level++;

        // Diminui o tempo de espera entre as paredes
        timewait -= timeWaitDecrease;
        if (timewait < 1f) timewait = 1; // Impede que o tempo de espera fique muito baixo
        
        PlayerSttsManager.instance.UpdateUI();

    }
}
