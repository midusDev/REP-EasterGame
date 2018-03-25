using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneController : MonoBehaviour {

    public Text HighScoreText;
    public GameObject infoPanel;
    public Text infoMessageFromFirebaseText;

    private DataController dataController;
    
    private void Start()
    {
        infoPanel.SetActive(false);
        dataController = FindObjectOfType<DataController>();
        dataController.InfoMessageFromFirebase("info");
        HighScoreText.text = "Topp poengsum: " + dataController.GetHighestPlayerScore().ToString();
    }


    public void InfoBtnWasPressed()
    {
        infoMessageFromFirebaseText.text = dataController.GetInfoMessage();
        infoPanel.SetActive(true);
    }


    public void OkBtnWasPressed()
    {
        infoPanel.SetActive(false);

    }


    // Use this for initialization
    public void LoadLevel(string loadLevel)
    {

   
        PlayerPrefs.SetString("selectedLevel", loadLevel);
        SceneManager.LoadScene(loadLevel);

    }

}
