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
    public static int achvCurrentCount = 0;
    public static int achvMaxCount = 1;
    
    [SerializeField] public GameObject _greenTick;

    // Start is called before the first frame update
    void Start()
    {
        _achievementTitle.text = _achievementTitleInput;
        _achievementDetails.text = _achievementDetailsInput;
        _greenTick.SetActive(false);
        achvCurrentCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTracker(int currentVal, int maxVal)
    {
        achvCurrentCount = currentVal;
        achvMaxCount = maxVal;
       _trackerText.text = achvCurrentCount.ToString() + "/" + achvMaxCount.ToString();
    }

    public void UpdateTrackerText(int currentVal)
    {
        _trackerText.text = currentVal.ToString() + "/" + achvMaxCount.ToString();
    }

    public void CapTracker()
    {
        _trackerText.text = achvMaxCount.ToString() + "/" + achvMaxCount.ToString();
    }
}
