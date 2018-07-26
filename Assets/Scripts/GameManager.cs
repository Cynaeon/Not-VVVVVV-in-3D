using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public int currentLevel;

    public static float gameTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() { Load(); }
    private void OnDisable() { Save(); }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");

        SaveData data = new SaveData();
        data.currentLevel = currentLevel;
        data.gameTime = gameTime;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            currentLevel = data.currentLevel;
            gameTime = data.gameTime;
        }
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            gameTime += Time.deltaTime;
    }
}

[System.Serializable]
class SaveData
{
    public int currentLevel;
    public float gameTime;
}