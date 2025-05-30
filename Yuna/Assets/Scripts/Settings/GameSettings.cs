using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public int qualityLevel = 2; // Por defeito: High (podes mudar o número conforme o índice do teu projeto)

    // Caminho onde o ficheiro JSON será guardado
    public static string FilePath => Application.persistentDataPath + "/gamesettings.json";

    void Awake()
    {
        // Tenta carregar as definições do ficheiro
        GameSettings loadedSettings = GameSettings.LoadFromFile();

        if (loadedSettings != null)
        {
            // Se existirem definições, aplica o nível de qualidade guardado
            QualitySettings.SetQualityLevel(loadedSettings.qualityLevel, true);
            Debug.Log("Definições carregadas: Quality Level " + loadedSettings.qualityLevel);
        }
        else
        {
            // Caso contrário, começa com HIGH (index 2 por exemplo)
            QualitySettings.SetQualityLevel(2, true);
            Debug.Log("Nenhum ficheiro encontrado. A usar qualidade HIGH por defeito.");
        }
    }

    // Guarda os dados atuais num ficheiro JSON
    public void SaveToFile()
    {
        string json = JsonUtility.ToJson(this, true);
        System.IO.File.WriteAllText(FilePath, json);
        Debug.Log("Definições guardadas em: " + FilePath);
    }

    // Carrega os dados do ficheiro JSON, ou retorna null se não existir
    public static GameSettings LoadFromFile()
    {
        if (System.IO.File.Exists(FilePath))
        {
            string json = System.IO.File.ReadAllText(FilePath);
            return JsonUtility.FromJson<GameSettings>(json);
        }
        return null;
    }
}
