using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIEndGame;
    public GameObject UIToHide;
    public TextMeshProUGUI UITextScore;
    public TextMeshProUGUI UITextSalary;

    private bool atOnce;
    void Start()
    {
        atOnce = true;
        UIEndGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PLayerCar.DataHolder.Death && atOnce)
        {
            atOnce = false;
            UIToHide.SetActive(false);
            UIEndGame.SetActive(true);
            UITextScore.text = PLayerCar.score.ToString();
            UITextSalary.text = BalanceHolder.DataHolder.Salary.ToString();
            Database.ExecuteQueryWithAnswer("UPDATE User SET balance_free = " + BalanceHolder.DataHolder.Balance + ", High_score = "+ PLayerCar.HighScore +" WHERE user_id = " + UserHolder.DataHolder.Name + ";");
        }
        if (PLayerCar.DataHolder.Death && Input.GetKey(KeyCode.Return))
        {
            OnClickReturn();
        }
    }
    public void OnClickReturn()
    {
        UIEndGame.SetActive(false);
        SceneManager.LoadScene("HubScene");
    }
}
