using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementBoxDetail : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _achievementTitle;
    [SerializeField] private string _achievementTitleInput;

    [SerializeField] private TextMeshProUGUI _achievementDetails;
    [SerializeField] private string _achievementDetailsInput;

    // Start is called before the first frame update
    void Start()
    {
        _achievementTitle.text = _achievementTitleInput;
        _achievementDetails.text = _achievementDetailsInput;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
