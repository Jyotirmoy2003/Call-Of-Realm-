using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] TMP_Text suggestionKey;
    [SerializeField] GameObject suggestionPanel;

    [SerializeField] TMP_Text logText;

    [SerializeField] TMP_Text questName,questInfo,questRemaining;

    [SerializeField] Image playerStateImage;

    [SerializeField] GameObject ExitPanel,restartPanel;
   

    
    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        suggestionPanel.SetActive(false);
        ExitPanel.SetActive(false);
        restartPanel.SetActive(false);
        questInfo.text="";
        questName.text="";
        questRemaining.text="";
        
       
    }

    public void ShowLog(string val,float stopLogTimmer)
    {
        logText.text+=val;
        Invoke("StopLog",stopLogTimmer);
    }
    public void ShowQuest(string QuestName,string objective)
    {
        questName.text=QuestName;
        questInfo.text=objective;
    }

    public void QuestCuurentAmout(string data)
    {    
        questRemaining.text=data;
        Debug.Log("Debugging:"+data);
        
    }

    void StopLog()
    {
        logText.text="";
    }

    public void SetPlayerState(Color c)
    {
        playerStateImage.color=c;
    }
    #region  Panel

    public void ShowExitPanel()
    {
        ExitPanel.SetActive(true);
        Time.timeScale=0;
    }
        public void OnClickExit()
        {
            LevelLoder.instance.LoadLevel(0);
            Time.timeScale=1;
        }

        public void OnClickNo()
        {
            ExitPanel.SetActive(false);
            Time.timeScale=1;
            Cursor.lockState=CursorLockMode.Locked;
        }

        public void ShowRestartPanel()
        {
            restartPanel.SetActive(true);
            Time.timeScale=0;
        }

        public void OnCLickRestart()
        {
            LevelLoder.instance.LoadLevel(1);
            Time.timeScale=1;
        }



    #endregion



    //set key suggestion in UI
    public void SetKeySuggest(string Key)
    {
        if(Key=="empty")
        {
            suggestionPanel.SetActive(false);
        }else{
            suggestionPanel.SetActive(true);
            suggestionKey.text=Key;
        }
    }




}
