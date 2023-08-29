using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LevelLoder : MonoBehaviour
{
    public static LevelLoder instance;
    [SerializeField] GameObject levelLoader;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text progressText;
    
     void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        
        DontDestroyOnLoad(gameObject);
        this.gameObject.SetActive(false);
    }

    public void LoadLevel(int sceneIndex)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsynchronusly(sceneIndex));
        Time.timeScale=1;
    }

    //fun to get Current secne index
    public int GetCuurentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    
    IEnumerator LoadSceneAsynchronusly(int sceneIndex)
    {
        AsyncOperation operation= SceneManager.LoadSceneAsync(sceneIndex);
        levelLoader.SetActive(true);
        //run untill its done loading
        while(!operation.isDone)
        {
            //set the value between 0-1
            float progress=Mathf.Clamp01(operation.progress);
            //update slider and Update text
            slider.value=progress;
            progressText.text=progress*100f + "%";
            //wait for one freame
            yield return null;
        }
        //when loading is done deactive the gameobject
        levelLoader.SetActive(false);
        Time.timeScale=1;

    }
}
