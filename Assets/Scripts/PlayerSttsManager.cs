using System.Collections;
using UnityEngine;

public class PlayerSttsManager : MonoBehaviour
{
    public Color initialColor;
    public Color hitColor = Color.red; // Cor para exibir durante o tremor

    public int lifes;
    public int protections;
    public float shakeIntensity = 2f; // Intensidade do tremor
    public float shakeDuration = 0.5f; // Dura��o do tremor
    public float playerSpeed;

    public Transform cameraTransform; // Refer�ncia � c�mera

    private Quaternion originalCameraRotation; // Salva a rota��o original da c�mera

    private void Start()
    {
        initialColor = Camera.main.backgroundColor;
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Define a c�mera principal como padr�o
        }

        // Armazena a rota��o original da c�mera
        originalCameraRotation = cameraTransform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            StartCoroutine(ShakeCamera());
            Destroy(other.gameObject);
        }
    }

    private IEnumerator ShakeCamera()
    {
        float elapsedTime = 0f;
        Camera mainCamera = cameraTransform.GetComponent<Camera>();

        while (elapsedTime < shakeDuration)
        {
            // Alterna entre a cor inicial e a cor de impacto
            mainCamera.backgroundColor = (elapsedTime % 0.1f < 0.05f) ? hitColor : initialColor;

            // Adiciona uma rota��o aleat�ria baseada na intensidade
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            float z = Random.Range(-shakeIntensity, shakeIntensity);

            cameraTransform.rotation = originalCameraRotation * Quaternion.Euler(x, y, z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaura a cor inicial e a rota��o original
        mainCamera.backgroundColor = initialColor;
        cameraTransform.rotation = originalCameraRotation;
    }
}
