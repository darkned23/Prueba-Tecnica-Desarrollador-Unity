using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("Componente de imagen que muestra la imagen del tutorial")]
    public Image tutorialImage;
    [Tooltip("Componente de texto que muestra el mensaje del tutorial")]
    public TMP_Text tutorialText;
    public TMP_Text continueButtonText;

    [Header("Tutorial Content")]
    [Tooltip("Array de imágenes para cada paso del tutorial")]
    public Sprite[] tutorialSprites;

    [Tooltip("Array de mensajes para cada paso del tutorial")]
    [TextArea(3, 10)]
    public string[] tutorialMessages;

    [SerializeField] private GameObject _panelMain;

    private int currentStep = 0;

    void Start()
    {
        tutorialImage.preserveAspect = true;

        if (tutorialSprites.Length > 0 && tutorialMessages.Length > 0)
        {
            currentStep = 0;
            UpdateTutorialUI();
        }
        else
        {
            Debug.LogWarning("No se han asignado imágenes o mensajes de tutorial.");
        }
    }

    public void NextStep()
    {
        int maxSteps = Mathf.Min(tutorialSprites.Length, tutorialMessages.Length);
        if (currentStep < maxSteps - 1)
        {
            currentStep++;
            UpdateTutorialUI();
        }
        else
        {
            if (continueButtonText.text != "Close")
            {
                continueButtonText.text = "Close";
            }
            else
            {
                CloseTutorial();
            }
        }
    }

    public void CloseTutorial()
    {
        currentStep = 0;
        UpdateTutorialUI();

        _panelMain.SetActive(true);
        continueButtonText.text = "Continue";

        gameObject.SetActive(false);
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            UpdateTutorialUI();
        }
    }

    private void UpdateTutorialUI()
    {
        if (currentStep < tutorialSprites.Length)
        {
            tutorialImage.sprite = tutorialSprites[currentStep];
        }

        if (currentStep < tutorialMessages.Length)
        {
            tutorialText.text = tutorialMessages[currentStep];
        }

        int maxSteps = Mathf.Min(tutorialSprites.Length, tutorialMessages.Length);
        if (currentStep < maxSteps - 1)
        {
            continueButtonText.text = "Continue";
        }
        else
        {
            continueButtonText.text = "Close";
        }
    }
}