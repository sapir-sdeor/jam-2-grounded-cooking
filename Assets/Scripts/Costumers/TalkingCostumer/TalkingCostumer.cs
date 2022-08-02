using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class TalkingCostumer : customers
{

    #region Inspector

   [Header("Talking Costumer Parameters")]
    [SerializeField] private TextMeshProUGUI questionDisplay;

    [SerializeField] private TextMeshProUGUI LeftButtonDisplay;

    [SerializeField] private TextMeshProUGUI RightButtonDisplay;

    [SerializeField] private Canvas DialogueGameObject;

    #endregion

    public struct Dialogue
    {
        public string _question;
        public string _goodAnswer;
        public  string _badAnswer;

        public Dialogue(string question, string goodAnswer, string badAnswer)
        {
            _question = question;
            _goodAnswer = goodAnswer;
            _badAnswer = badAnswer;
        }
    }

    #region Private Properties

    private List<Dialogue> _dialogues;
    
    private Queue<string> _questions;

    private Queue<string> _goodAnswers;

    private Queue<string> _badAnswers;
    
    private bool _isDialogueComplete = false;

    // 0 is left button, 1 is right button.
    private int _correctButton;

    private enum Buttons
    {
        Left,
        Right
    }

    #endregion

    #region Constants

    private const int NO_MORE_QUESTIONS = 0;

    #endregion
    
    #region Mono Behaviour Funtions

    private void Awake()
    {
       // DialogueGameObject.worldCamera = Camera.current;
        SetUpDialogueQueues();  
    }

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        SetUpDialogue();
    }
  
    #endregion
    
    #region Event Functions
    
    /**
     * event Function, called by one of the two dialouge  buttons.
     * buttonPressed: 0 if the function was called by left button, 1 if called by the right. 
     */
    public void DialogueButtonPressed(int buttonPressed)
    {
        bool isAnswerCorrect = false;

        // check if the answer is correct
        switch (buttonPressed)
        {
            case (int) Buttons.Left:

                isAnswerCorrect = _correctButton == (int) Buttons.Left;
                break;

            case (int) Buttons.Right:

                isAnswerCorrect = _correctButton == (int) Buttons.Right;
                break;

        }

        SetNextDialogueStep(isAnswerCorrect);

    }
    
    #endregion

    #region Private Methods
    /**
     * called by start to enable the dialogue
     */
    private void SetUpDialogue()
    {
        DialogueGameObject.gameObject.SetActive(true);
        GetNextQuestion();
    }

    /**
     * this function is called by "DialogueButtonPressed" and sets up the next dialogue based on what the player
     * has answered 
     */
    private void SetNextDialogueStep(bool isAnswerCorrect)
    {
        if (isAnswerCorrect)
        {   
            
            // player answered right so the next question needs to be set up, if there are no questions left
            // the player has succeeded
            if (_questions.Count == NO_MORE_QUESTIONS)
            {   
                Debug.Log("you answered all the questions, costumer is happy!");
                // no more questions, dialogue is complete
                DialogueGameObject.gameObject.SetActive(false);
                _isDialogueComplete = true;
                SetAngerLevel(CurAngerLevel - 1);
            }

            else
            {   
                Debug.Log("good answer, next question!");
                // still have questions
                GetNextQuestion();
            }
        }

        else
        {
            // wrong answer, you are not nice and costumer is angry now
            Debug.Log("wrong answer, you are not nice and costumer is angrier now");

            DialogueGameObject.gameObject.SetActive(false);
            _isDialogueComplete = true;
            SetAngerLevel(CurAngerLevel - 1);
        }


    }

    private void GetNextQuestion()
    {
        StartCoroutine(TypeEffectQuestion());
        _correctButton = Random.Range(0, 1);

        if (_correctButton == (int) Buttons.Left)
        {
            LeftButtonDisplay.text = _goodAnswers.Dequeue();
            RightButtonDisplay.text = _badAnswers.Dequeue();
        }

        else if (_correctButton == (int) Buttons.Right)
        {
            RightButtonDisplay.text = _goodAnswers.Dequeue();
            LeftButtonDisplay.text = _badAnswers.Dequeue();
        }

    }

    private void SetUpDialogueQueues()
    {
        _dialogues = new List<Dialogue>()
        {
            new Dialogue("Fine day for shopping is it?", "Yes it is!", "Yeah, whatever"),
            new Dialogue("Are the flies fresh?", "it's the best!", "more than you breath"),
            new Dialogue("Any Unicorn hair left?", "Let me check!", "I ate them"),
            new Dialogue("You make deliveries?", "Sorry, no", "For you? No chance!"),
            new Dialogue("Any sales i should know?", "I'll check!", "Cheap huh"),
            new Dialogue("Can you help me carry the stuff to my broom?", "i'll call someone", "Lazy huh?"),
            new Dialogue("Where is the broom oil?", "Aisle 3 madame", "don't wanna know"),
            new Dialogue("You guys open on holidays?", "Of course, sir!", "Yeah, don't come")
        };
        
        
        _questions = new Queue<string>();
        _goodAnswers = new Queue<string>();
        _badAnswers = new Queue<string>();

        int first = Random.Range(0, 3);
        int second = Random.Range(4, 7);
        
        _questions.Enqueue(_dialogues[first]._question);
        _questions.Enqueue(_dialogues[second]._question);
        _goodAnswers.Enqueue(_dialogues[first]._goodAnswer);
        _goodAnswers.Enqueue(_dialogues[second]._goodAnswer);
        _badAnswers.Enqueue(_dialogues[first]._badAnswer);
        _badAnswers.Enqueue(_dialogues[second]._badAnswer);
       
    }
    

    protected override void CustomerLeave()
    {
        if (!_isDialogueComplete)
        {
            return;
        }
        _startMove = true;
        _changeMainCustomer = true;
        customerId = 3;
        if (_customerFinish)
           GameManager.PlayerScore += 3;
            
    }

   
    #endregion
    private IEnumerator TypeEffectQuestion()
    {
        questionDisplay.text = "";
        string curQuestion = _questions.Dequeue();

        foreach (char c in curQuestion)
        {
            questionDisplay.text += c;
            yield return new WaitForSeconds(0.05f);
        }

    }

   
}
