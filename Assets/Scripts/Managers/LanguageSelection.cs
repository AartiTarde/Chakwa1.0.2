using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageSelection : MonoBehaviour
{
    public Toggle englishToggle;
    public Toggle marathiToggle;
    public TMP_Text selectedLanguageText;
    private const string LanguageKey = "SelectedLanguage";

    void Start()
    {
        
        string savedLanguage = PlayerPrefs.GetString(LanguageKey, "English");

        if (savedLanguage == "English")
        {
            englishToggle.isOn = true;
            marathiToggle.isOn = false;
        }
        else if (savedLanguage == "Marathi")
        {
            englishToggle.isOn = false;
            marathiToggle.isOn = true;
        }

       
        SetLanguage(savedLanguage);
        englishToggle.onValueChanged.AddListener((isSelected) => OnToggleSelected(isSelected, "English"));
        marathiToggle.onValueChanged.AddListener((isSelected) => OnToggleSelected(isSelected, "Marathi"));
    }

    void OnToggleSelected(bool isSelected, string language)
    {
        if (isSelected)
        {
            
            if (language == "English")
            {
                marathiToggle.isOn = false;
            }
            else if (language == "Marathi")
            {
                englishToggle.isOn = false;
            }

           
            PlayerPrefs.SetString(LanguageKey, language);
            PlayerPrefs.Save();

           
            SetLanguage(language);
        }
    }

    void SetLanguage(string language)
    {
        if (selectedLanguageText != null)
        {
            selectedLanguageText.text = "Selected Language: " + language;
        }
        Debug.Log("Language set to: " + language);

       
    }
}
