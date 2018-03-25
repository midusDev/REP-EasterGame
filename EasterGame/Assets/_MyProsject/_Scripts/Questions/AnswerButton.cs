using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

    public Text answerText;
    public Button thisButton;
    private QuizAnswerData answerData;
    private QuizGameController gameController;

    private Color originalColor;

    // Use this for initialization
    void Start()
    {


        gameController = FindObjectOfType<QuizGameController>();
        originalColor = thisButton.GetComponent<Image>().color;
    }

    public void Setup(QuizAnswerData data)
    {

        answerData = data;
        answerText.text = answerData.answer;

    }

    public void HandleClick()
    {

        if (answerData.isCorrect)
        {
            thisButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            thisButton.GetComponent<Image>().color = Color.red;

        }
        gameController.AnswerButtonClicked(answerData.isCorrect);
    }

    private void OnDisable()
    {
        thisButton.GetComponent<Image>().color = originalColor;

    }

}
