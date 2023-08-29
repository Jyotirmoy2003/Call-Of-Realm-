using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Experimental.GlobalIllumination;
//using UnityEngine.UIElements;



public class MainMenu : MonoBehaviour
{
   [SerializeField] GameObject menuPanel,settingPanel,controlPanel;
   [Header("Mouse")]
   [SerializeField] Slider mouseSensetivitySlider;
   [SerializeField] TMP_InputField mouseInputField;
   [Header("Sound")]
   [SerializeField] Slider soundSlider;
   [SerializeField] TMP_InputField soundInputField;
   [SerializeField] int GameSceneIndex=1;



   [Header("Graphics Menu")]
    [SerializeField] List<Toggle> graphicsToggles=new List<Toggle>();


    void Start()
    {
        AudioManager.instance.PlaySound("background");
        //sound slider
        float currentSoundValue=PlayerPrefs.GetFloat("soundlvl",100f);
        soundSlider.value=currentSoundValue;
        soundInputField.text=currentSoundValue.ToString();

        //mouse slider
         
        float currentMouseValue=PlayerPrefs.GetFloat("mouse",100f);
        mouseSensetivitySlider.value=currentMouseValue;
        mouseInputField.text=currentMouseValue.ToString();

        controlPanel.SetActive(false);
        settingPanel.SetActive(false);
        menuPanel.SetActive(true);

    }
#region  Mian Menu
    public void OnClickStartGame()
    {
        LevelLoder.instance.LoadLevel(GameSceneIndex);
       // SceneManager.LoadScene("Game");
        Time.timeScale=1;
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickSettings()
    {
        menuPanel.SetActive(false);
        controlPanel.SetActive(false);
        settingPanel.SetActive(true);
        
    }

    public void OnClickControl()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(false);
        controlPanel.SetActive(true);
    }
#endregion

#region Settings

    #region  slider
    public void SetFromSlider(int switch_on)
    {
        switch (switch_on)
        {
            case 1:
                soundInputField.text=soundSlider.value.ToString();
                AudioManager.instance.SetAudioVolumeFactor(soundSlider.value);
                PlayerPrefs.SetFloat("soundlvl",soundSlider.value);
                break;

            case 3: 
                mouseInputField.text=mouseSensetivitySlider.value.ToString();
                PlayerPrefs.SetFloat("mouse",mouseSensetivitySlider.value);
                break;
                
            
            default:
                Debug.Log("Wrong Choice");
                break;
        }

    }
    
    public void SetFromInputField(int switch_on)
    {
        switch (switch_on)
        {
            case 1:
                int temp=int.Parse(soundInputField.text);
                if(temp>100) temp=100;
                else if(temp<0) temp=0;
                soundSlider.value=temp;
                AudioManager.instance.SetAudioVolumeFactor(temp);
                soundInputField.text=temp.ToString();
                
                PlayerPrefs.SetFloat("soundlvl",soundSlider.value);
                break;

            case 3:
                int temp1=int.Parse(mouseInputField.text);
                if(temp1>1000) temp1=1000;
                else if(temp1<0) temp1=0;
                mouseSensetivitySlider.value=temp1;
                mouseInputField.text=temp1.ToString();
                PlayerPrefs.SetFloat("mouse",mouseSensetivitySlider.value);
                break;
            
            default:
                Debug.Log("Wrong Choice");
                break;
        }
    }
    #endregion
    public void OnClickBackSettings()
    {
        settingPanel.SetActive(false);
        controlPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnClickBackControls()
    {
        settingPanel.SetActive(false);
        controlPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

#endregion



}
