using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System;

public class QuizGameController : MonoBehaviour {

    public GameObject testAdPanel;

    private int showAdEvery = 5;
    private float roundTime;

    // UI
    public Text questionDisplayText;
    public Text timeRemainingDisplayText;
    public Text scoreDisplayText;
    public Text currentScoreText;
    public Text highScoreDisplay;
    public Text questionCountDisplay;

    public GameObject correctAnsweredPanel;
    public GameObject correctAnsweredButton;
    public Text correctAnsweredText;

    public Text questionsAnsweredCorretText;

    public Transform answerButtonParent;
    public SimpleObjectPool answerButtonObjectPool;


    public GameObject roundOverPanelDisplay;

    // Classes
    private DataController dataController;
    private List<QuizQuestionData> questionPool;


    private int questionIndex;
    private int questionsAnsweredCorrect;
    private int questionsAnsweredTotall;
    private bool isRoundActive;
    private int playerScore;


    private List<GameObject> answerButtonGameObjects = new List<GameObject>();


    public AdController adController;


    // Use this for initialization
    void Start () {

        adController.RequestInterstitial();



        testAdPanel.SetActive(false);
        roundOverPanelDisplay.SetActive(false);
        dataController = FindObjectOfType<DataController>();
        questionPool = dataController.GetQuizQuestions();
        //dataController.QuizData();

        //timeRemaining = 20;
        isRoundActive = true;

        RandomaizeQuestions();
        questionIndex = 0;
        playerScore = 0;
        ShowQuestion();
        //RandomaizeQuestions();

    }

    void Update()
    {

        if (isRoundActive && testAdPanel.activeSelf == false)
        {
            roundTime -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (roundTime <= 0f)
            {
                adController.ShowInterstitial();
                EndRound();
            }
        }

    }


    // Slett den her etterpå
    public void CloseTestADPanel()
    {
        testAdPanel.SetActive(false);

    }



    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = Mathf.Round(roundTime).ToString();

    }


    private void ShowQuestion()
    {
        isRoundActive = true;
        roundTime = 10;

        RemoveAnswerButtons();
        QuizQuestionData quizQuestionData = questionPool[questionIndex];
        questionDisplayText.text = quizQuestionData.questionText;
        questionsAnsweredTotall = questionIndex + 1;
        questionCountDisplay.text = questionsAnsweredTotall.ToString() + " / " + questionPool.Count;

        for (int i = 0; i < quizQuestionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObject.transform.SetParent(answerButtonParent, false);
            answerButtonGameObject.GetComponent<Button>().interactable = true;
            answerButtonGameObjects.Add(answerButtonGameObject);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(quizQuestionData.answers[i]);
        }

    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        foreach (var button in answerButtonGameObjects)
        {
            button.GetComponent<Button>().interactable = false;
        }
        StartCoroutine(ExecuteAfterTime(isCorrect, 1f));

    }


    public void ReturnToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    private void RandomaizeQuestions()
    {
        for (int i = 0; i < questionPool.Count; i++)
        {
            QuizQuestionData temp = questionPool[i];
            int randomIndex = Random.Range(i, questionPool.Count);
            questionPool[i] = questionPool[randomIndex];
            questionPool[randomIndex] = temp;
        }

    }



    private void RemoveAnswerButtons()
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }


    public void EndRound()
    {
        //adController.ShowInterstitial();
        isRoundActive = false;

        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = "Topp poengsum: " + dataController.GetHighestPlayerScore().ToString();
        currentScoreText.text = "Poengsum: " + playerScore;
        questionsAnsweredCorretText.text = "Du klarte " + questionsAnsweredCorrect.ToString() + " av " + questionsAnsweredTotall.ToString();

        //questionPanelDisplay.SetActive(false);
        roundOverPanelDisplay.SetActive(true);

    }


    private void ShowCorrectAnsweredPanel(bool isCorrect, bool isActive)
    {
        correctAnsweredPanel.SetActive(isActive);

        if (isCorrect)
        {
            correctAnsweredButton.GetComponent<Image>().color = Color.green;
            correctAnsweredText.text = "RIKTIG!";
        }else
        {
            correctAnsweredButton.GetComponent<Image>().color = Color.red;
            correctAnsweredText.text = "FEIL!";
        }
    }

    IEnumerator ExecuteAfterTime(bool isCorrect, float time)
    {
        //int timesQuestionsIsShowd;

        isRoundActive = false;

        ShowCorrectAnsweredPanel(isCorrect, true);
        if (isCorrect)
        {
            int temp = (int)roundTime;
            questionsAnsweredCorrect++;
            playerScore += temp;
            scoreDisplayText.text = playerScore.ToString();
        }

        //yield return new WaitForSeconds(0.2f);


        if (questionPool.Count > questionIndex + 1)
        {
            if (questionsAnsweredTotall % showAdEvery == 0)
            {
                // Show ad here
                yield return new WaitForSeconds(0.5f);

                adController.ShowInterstitial();
                yield return new WaitForSeconds(1f);

                //yield return new WaitForSeconds(time);
            }
        }


        yield return new WaitForSeconds(time);

        if (questionPool.Count > questionIndex + 1)
        {
            questionIndex++;
            /*
            if (questionsAnsweredTotall % showAdEvery == 0)
            {
                // Show ad here
                adController.ShowInterstitial();
                //yield return new WaitForSeconds(time);

            }

            */
            ShowQuestion();
        }
        else
        {
            adController.ShowInterstitial();
            EndRound();
        }

        ShowCorrectAnsweredPanel(isCorrect, false);

    }
}
