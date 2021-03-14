using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileUI : MonoBehaviour {
    public string saveName;
    public void LoadGame()
    {
        //GameData.data.Load(saveName);
        //TODO: check the save file info against the online database to validate
        SceneManager.LoadScene("Room Prototype");
    }
}
