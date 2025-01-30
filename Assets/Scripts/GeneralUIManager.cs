using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GeneralUIManager : NetworkBehaviour
{
    // Textos para exibir pontos, vidas e nível
    public TextMeshProUGUI txt_points;
    public TextMeshProUGUI txt_lifes;
    public TextMeshProUGUI txt_level;

    // Objetos adicionais cujas escalas podem ser modificadas
    public GameObject pointsObject;  // Objeto para a escala de pontos
    public GameObject lifesObject;   // Objeto para a escala de vidas
    public GameObject levelObject;   // Objeto para a escala de nível

    // Escalas máximas para cada objeto
    public Vector3 pointsScaleMax = new Vector3(1.2f, 1.2f, 1f);  // Exemplo de escala
    public Vector3 lifesScaleMax = new Vector3(1.2f, 1.2f, 1f);   // Exemplo de escala
    public Vector3 levelScaleMax = new Vector3(1.2f, 1.2f, 1f);   // Exemplo de escala

    public float maxSize;

    public float minSize;
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!IsSpawned)
            return; 

        // Verifique se houve alguma mudança nos valores antes de atualizar os textos
        int newPoints = PlayerSttsManager.instance.points;
        int newLifes = PlayerSttsManager.instance.lifes;
        int newLevel = PlayerSttsManager.instance.level;

        // Se houve alguma mudança, aciona a animação nos textos e objetos
        if (txt_points.text != newPoints.ToString())
        {
            txt_points.text = newPoints.ToString();
            StartCoroutine(FlashTextSize(txt_points, pointsObject, pointsScaleMax));
        }

        if (txt_lifes.text != newLifes.ToString())
        {
            txt_lifes.text = newLifes.ToString();
            StartCoroutine(FlashTextSize(txt_lifes, lifesObject, lifesScaleMax));
        }

        if (txt_level.text != newLevel.ToString())
        {
            txt_level.text = newLevel.ToString();
            StartCoroutine(FlashTextSize(txt_level, levelObject, levelScaleMax));
        }
    }

    // Coroutine para fazer o texto "piscar" em tamanho e escala de objetos de maneira suave
    private IEnumerator FlashTextSize(TextMeshProUGUI text, GameObject targetObject, Vector3 maxScale)
    {
        float elapsedTime = 0f;

        Vector3 originalScale = Vector3.one; // Salva a escala original

        float duration = 0.6f; // Duração da animação

        // Usando uma interpolação linear mais suave
        while (elapsedTime < duration)
        {
            // Calcula a metade do tempo
            float lerpFactor;

            // Na primeira metade da duração, aumenta o tamanho
            if (elapsedTime < duration / 2)
            {
                lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / (duration / 2)); // Cresce
            }
            else
            {
                // Na segunda metade da duração, diminui o tamanho
                lerpFactor = Mathf.SmoothStep(1f, 0f, (elapsedTime - (duration / 2)) / (duration / 2)); // Diminui
            }

            // Alterando o tamanho do texto com Lerp
            text.fontSize = Mathf.Lerp(minSize, maxSize, lerpFactor);

            // Alterando a escala do objeto com Lerp
            targetObject.transform.localScale = Vector3.Lerp(originalScale, maxScale, lerpFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaura a escala e o tamanho original após a animação
        text.fontSize = minSize;
        targetObject.transform.localScale = originalScale;

        // Garantir que o texto e o objeto estejam com o tamanho original
        if (text.fontSize != minSize)
        {
            text.fontSize = minSize;
        }
        if (targetObject.transform.localScale != originalScale)
        {
            targetObject.transform.localScale = originalScale;
        }
    }
}
