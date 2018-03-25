using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DataController : MonoBehaviour {

    AsyncOperation asyncLoadLevel;


    private PlayerProgress playerProgress;
    public List<QuizQuestionData> quizQuestionData;

    public bool questionsIsLoaded;
    public string messageFromFirebase;

    public AdController adController;


    void Start () {

        DontDestroyOnLoad(gameObject);
        adController.SetUpAdController();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://easterquiz-2135f.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        LoadPlayerProgress();
        StartCoroutine(LoadLevel());
    }


    public List<QuizQuestionData> GetQuizQuestions()
    {

        return quizQuestionData;
    }


    public string GetInfoMessage()
    {
        return messageFromFirebase;
    }


    public void InfoMessageFromFirebase(string keyString)
    {
        FirebaseDatabase.DefaultInstance.GetReference("message").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Something whent wrong");
                questionsIsLoaded = false;

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                messageFromFirebase = snapshot.Child(keyString).Value as string;
                //Debug.Log(temp);
            }

        });
    }


    public void QuizData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("quiz").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Something whent wrong");
                questionsIsLoaded = false;

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string dataAsJson = snapshot.GetRawJsonValue();
                //Debug.Log("DATA FROM JSON: " + dataAsJson);
                QuizData loadedData = JsonUtility.FromJson<QuizData>(dataAsJson);
                //Debug.Log(loadedData.quizData[31].questionText);
                quizQuestionData = loadedData.quizData;
                //allRoundData = loadedData.allRoundData;
                questionsIsLoaded = true;

            }

        });

    }


    public void SetQuestionsIsLoaded(bool isQuestionsLoaded)
    {
        questionsIsLoaded = isQuestionsLoaded;
    }

    public bool GetIsQuestionsIsLoaded()
    {
        return questionsIsLoaded;
    }






    public void SubmitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }


    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }


    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }





    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);
        asyncLoadLevel = SceneManager.LoadSceneAsync("01_MainMenu", LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            //  print("Loading the Scene");
            yield return null;
        }
    }


}
