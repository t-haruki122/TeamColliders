using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartButton : MonoBehaviour
{
    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    [SerializeField] private TMP_InputField name;
    [SerializeField] private TextMeshProUGUI displayScore;
    private Text errorText;
    private Transform loading;
    private string[] rankHeader = {"1st.", "2nd.", "3rd.", "4th.", "5th.",
    "6th.", "7th."};

    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Start()
    {
        loading = transform.Find("Loading");
        loading.gameObject.SetActive(false);

        string[] temp = GameManager.GMInstance.getplayerScoreText();

        for (int i = 0, len = GameManager.GMInstance.getPlayerScoreLength(); i < len; ++i)
        {
            if (!string.IsNullOrEmpty(temp[i])) displayScore.text += "\n" + rankHeader[i] + "\t" + temp[i];
            else break;
        }
    }
    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        loading.gameObject.SetActive(true); //Now Loading...を表示

        string playerName = name.text;

        if (string.IsNullOrEmpty(playerName))
        {
            errorText.text = "Please enter your name";
            return;
        }
        /*名前を保存*/
        GameManager.GMInstance.savePlayerData(playerName, 0);
        /*変数を初期化*/
        GameManager.GMInstance.initialize();
        /*シーン遷移*/
        Debug.Log("Start->Main");
        SceneManager.LoadScene("Main");
    }
}
