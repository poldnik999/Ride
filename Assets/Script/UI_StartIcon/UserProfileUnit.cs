using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserProfileUnit : MonoBehaviour
{
    public string userID;
    public string userNickname;
    public string userLevel;

    public TextMeshProUGUI _userNickname;
    public TextMeshProUGUI _userLevel;
    // Start is called before the first frame update
    void Start()
    {
        _userNickname.text = userNickname;
        _userLevel.text = userLevel;
    }
    public void OnClickStart()
    {
        UserHolder.DataHolder.Name = userID;
        SceneManager.LoadScene("HubScene");
    }
}
