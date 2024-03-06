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
    [SerializeField] private TextMeshProUGUI _trackerText;
    
    [SerializeField] public GameObject _greenTick;

    // Start is called before the first frame update
    void Start()
    {
        _achievementTitle.text = _achievementTitleInput;
        _achievementDetails.text = _achievementDetailsInput;
        _greenTick.SetActive(false);
    }

    public void SetTracker(int maxVal)
    {
       _trackerText.text = "0/" + maxVal.ToString();
    }

    public void UpdateTrackerText(int currentVal, int maxVal)
    {
        _trackerText.text = currentVal.ToString() + "/" + maxVal.ToString();
    }

    public void CapTracker(int maxVal)
    {
        _trackerText.text = maxVal.ToString() + "/" + maxVal.ToString();
    }
}
