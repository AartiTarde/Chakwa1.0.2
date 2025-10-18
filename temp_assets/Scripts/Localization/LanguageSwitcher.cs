/*using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class LanguageSwitcher : MonoBehaviour
{
    // References to the buttons
    public Button englishButton;
    public Button marathiButton;

    // Language locales
    private Locale englishLocale;
    private Locale marathiLocale;

    // Reference to button texts (optional)
    public TextMeshProUGUI englishButtonText;
    public TextMeshProUGUI marathiButtonText;

    // Localized strings for button text (optional)
    public LocalizedString localizedEnglishText;
    public LocalizedString localizedMarathiText;

    // Start is called before the first frame update
    void Start()
    {
        // Get the available locales from the Localization Settings
        englishLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
        marathiLocale = LocalizationSettings.AvailableLocales.GetLocale("mr");

        var availableLocales = LocalizationSettings.AvailableLocales.Locales;
        foreach (var locale in availableLocales)
        {
            Debug.Log("Available Locale: " + locale.Identifier);
        }

        // Set default language (optional)
        LocalizationSettings.SelectedLocale = englishLocale;

        // Set button listeners to change language
        englishButton.onClick.AddListener(SetEnglish);
        marathiButton.onClick.AddListener(SetMarathi);

        // Update button text based on current language (this is optional)
        UpdateButtonTexts();
    }

    // Set English Language
    public void SetEnglish()
    {
        LocalizationSettings.SelectedLocale = englishLocale;
        Debug.Log("Language changed to English");
        UpdateButtonTexts();
    }

    // Set Marathi Language
    public void SetMarathi()
    {
        LocalizationSettings.SelectedLocale = marathiLocale;
        Debug.Log("Language changed to Marathi");
        UpdateButtonTexts();
    }

    // Update the button texts based on the selected language
    private void UpdateButtonTexts()
    {
        if (englishButtonText != null)
        {
            englishButtonText.text = localizedEnglishText.GetLocalizedString();
        }
        if (marathiButtonText != null)
        {
            marathiButtonText.text = localizedMarathiText.GetLocalizedString();
        }
    }
}
*/