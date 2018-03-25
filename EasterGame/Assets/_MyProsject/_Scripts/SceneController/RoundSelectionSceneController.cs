using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundSelectionSceneController : MonoBehaviour {

    private DataController dataController;
    private List<QuizQuestionData> questionPool;
    public GameObject transPanel;




    private void Start()
    {
        transPanel.SetActive(false);

        dataController = FindObjectOfType<DataController>();

    }


    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("01_MainMenu");

    }


    public void LoadLevel(string loadLevel)
    {
        transPanel.SetActive(true);

        if (!dataController.GetIsQuestionsIsLoaded())
        {
            //Debug.Log("Getting QUIZ DATA");

            dataController.QuizData();

        }
        StartCoroutine(GoToNextScene(loadLevel));

        /*

        if (questionPool.Count > 0)
        {
            SceneManager.LoadScene(loadLevel);

        }
        else
        {
            questionPool = dataController.GetQuizQuestions();

        }
        //PlayerPrefs.SetString("selectedLevel", loadLevel);
        */

    }


    // This check if questions is loaded
    IEnumerator GoToNextScene(string loadLevel)
    {
        int i = 0;

        // This is to reload getisQuestionsIsKLoaded 
        while (!dataController.GetIsQuestionsIsLoaded() && i < 500)
        {
            //Debug.Log("HELLLLOOO");

            dataController.GetIsQuestionsIsLoaded();
            i++;
            yield return null;
        }

        yield return new WaitForSeconds(2f);


        if (dataController.GetIsQuestionsIsLoaded())
        {
            SceneManager.LoadScene(loadLevel);
           // Debug.Log("Q IS LOADED");


        }
        else
        {
            transPanel.SetActive(false);

           // Debug.Log("SOMETHING WHENT WRONG TRY AGAIN OR CHECK INTERNETT CONNECTION");

        }
    }

}
