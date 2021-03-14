using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject SaveFileList;
    public GameObject LoadFileUIButton;

    public List<AnimationClip> backgroundAnimations;
    public Animator bgAnim;

    //animation of buttons etc plays in parallel via timeline
    private void Start()
    {
        //splash screens

        //play intro sound while fading in
        GetComponent<AudioSource>().Play();
        //suddenly snaps into a random animation
        //Random randy = new Random();
        //bgAnim.Play(backgroundAnimations[0]);

    }

    public void Quit()
    {
        Application.Quit();
    }
    //searches the save directory and creates a UI list of all save files
    public void LoadSaveFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        foreach (FileInfo f in dir.GetFiles("*.dat"))
        {
            GameObject savefile = Instantiate(LoadFileUIButton, SaveFileList.transform);
            string saveName = Path.GetFileNameWithoutExtension(f.Name);
            savefile.GetComponent<SaveFileUI>().saveName = saveName;
            savefile.GetComponentInChildren<Text>().text = saveName;
        }
    }

    //activates when leaving the load screen
    public void PurgeFileList()
    {
        foreach (Transform child in SaveFileList.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
