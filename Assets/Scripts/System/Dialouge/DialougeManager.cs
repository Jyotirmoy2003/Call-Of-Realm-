using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager instance;
    [SerializeField] int winCutsceneIndex=2;
    private Queue<string> sentences;
    public TMP_Text NPCname;
    public TMP_Text dialougeForNpc;
    public GameObject dialogPanel;
    public KeyCode triggerKey=KeyCode.Tab;
    public bool triggered=false;
    private bool isGameWin=false;

   
    
    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        sentences=new Queue<string>();
        dialogPanel.SetActive(false);
    }

    //call when player wants to start a conversation
    public void StartDialouge(Dialouge dialouge)
    {
        triggered=true;
        dialogPanel.SetActive(true);
       
        NPCname.text=dialouge.name;
       
        sentences.Clear();

        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    //fun to disply the next sentence
    public void  DisplayNextSentence()
    {
        if(sentences.Count==0)
        {
            EndDialouge();

            return;
        }

        string sentence=sentences.Dequeue();
        StopAllCoroutines();
       StartCoroutine(TypeSentences(sentence));

    }
    
    //type words one by one
    IEnumerator TypeSentences(string sentence)
    {
        dialougeForNpc.text="";
        foreach(char letter in sentence.ToCharArray())
        {
                dialougeForNpc.text+=letter;
                yield return null;
        }
    }

    public void EndDialouge()
    {
        
        dialogPanel.SetActive(false);
        //when game wined then after completeing the dialouge show the cutscene
        if(isGameWin && triggered)
        {
            LevelLoder.instance.LoadLevel(winCutsceneIndex);
        }
        triggered=false;
    }


    public void GameWinEvent(Component sender,object data)
    {
       if(data is bool && (bool)data)
       {
            isGameWin=true;
       }
        
    }
    


}
