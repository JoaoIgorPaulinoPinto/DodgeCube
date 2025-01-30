using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSttsManager : NetworkBehaviour
{

    public GeneralUIManager generalUIManager;

    public static PlayerSttsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    public int points;
    public int lifes;
    public int level;


    public Color initialColor;
    public Color hitColor = Color.red; // Cor para exibir durante o tremor

    public int protections;
    public float shakeIntensity = 2f; // Intensidade do tremor
    public float shakeDuration = 0.5f; // Duração do tremor
    public float playerSpeed;

    public Transform cameraTransform; // Referência à câmera

    private Quaternion originalCameraRotation; // Salva a rotação original da câmera

    private void Start()
    {
        initialColor = Camera.main.backgroundColor;
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Define a câmera principal como padrão
        }

        // Armazena a rotação original da câmera
        originalCameraRotation = cameraTransform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (lifes >= 1)
            {
                StartCoroutine(ShakeCamera(null, true));
                lifes--;
                UpdateUI();
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject, 0.5f);
                Destroy(other);
                    
              }

          
        }
    }
    public void UpdateUI()
    {
        generalUIManager.UpdateUI();
    }

    public void AddPoint()
    {
        points++;
        UpdateUI(); // Atualiza a interface do usuário para refletir os pontos
        StartCoroutine(FlashGreen()); // Inicia a corrotina para o flash verde
    }

    private IEnumerator FlashGreen()
    {
        float flashDuration = 0.33f; // Duração total do flash
        float elapsedTime = 0f;
        Camera mainCamera = cameraTransform.GetComponent<Camera>();

        StartCoroutine(ShakeCamera(0.1f, false));
        while (elapsedTime < flashDuration)
        {
            // Calcula o fator de interpolação (vai de 0 a 1 e volta a 0)
            float lerpFactor = Mathf.PingPong(elapsedTime / (flashDuration / 2), 1);

            // Interpola entre a cor inicial e a cor verde
            mainCamera.backgroundColor = Color.Lerp(initialColor, Color.green, lerpFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaura a cor inicial
        mainCamera.backgroundColor = initialColor;
    }

    private IEnumerator ShakeCamera(float? intensity, bool whtFlash)
    {
        float elapsedTime = 0f;
        float currentIntansity;
        if(intensity!= null)
        {
            currentIntansity = intensity.Value;
        }
        else
        {
            currentIntansity = shakeIntensity;
        }
        Camera mainCamera = cameraTransform.GetComponent<Camera>();

        while (elapsedTime < shakeDuration)
        {
            if (whtFlash)
            {
                mainCamera.backgroundColor = (elapsedTime % 0.1f < 0.05f) ? hitColor : initialColor;

            }
            // Alterna entre a cor inicial e a cor de impacto

            // Adiciona uma rotação aleatória baseada na intensidade
            float x = Random.Range(-currentIntansity, currentIntansity);
            float y = Random.Range(-currentIntansity, currentIntansity);
            float z = Random.Range(-currentIntansity, currentIntansity);

            cameraTransform.rotation = originalCameraRotation * Quaternion.Euler(x, y, z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaura a cor inicial e a rotação original
        mainCamera.backgroundColor = initialColor;
        cameraTransform.rotation = originalCameraRotation;
    }
}
