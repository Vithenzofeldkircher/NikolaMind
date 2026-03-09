using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static Dictionary<string, string> currentLanguage;

    void Awake()
    {
        SetPortuguese();
    }

    public static void SetPortuguese()
    {
        currentLanguage = LanguageData.PT;
    }

    public static void SetEnglish()
    {
        currentLanguage = LanguageData.EN;
    }

    public static string GetText(string key)
    {
        if (currentLanguage.ContainsKey(key))
            return currentLanguage[key];

        return key;
    }
}