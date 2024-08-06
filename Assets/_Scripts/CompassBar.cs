using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CompassBar : PlayerJoinedNotifier
{
    [SerializeField] float m_squashDivider = 3;

    [SerializeField] private RectTransform compassTransform;
    [SerializeField] private RectTransform objectiveMarkerTransform;
    [SerializeField] private RectTransform northMarkerTransform;
    [SerializeField] private RectTransform southMarkerTransform;
    [SerializeField] private RectTransform eastMarkerTransform;
    [SerializeField] private RectTransform westMarkerTransform;

    [SerializeField] private RectTransform northEastMarkerTransform;
    [SerializeField] private RectTransform northWestMarkerTransform;
    [SerializeField] private RectTransform southEastMarkerTransform;
    [SerializeField] private RectTransform southWestMarkerTransform;

    [SerializeField] private Transform cameraObjectTransform;
    public static Transform objectiveObjectTransform;

    Camera m_cam;

    [SerializeField] private GameObject objectiveMarkerImage;
    //[Header("Debug")]

    [Header("Players")]
    [SerializeField] GameObject player;
    [SerializeField] private RectTransform player1;
    [SerializeField] private RectTransform player2;
    [SerializeField] private RectTransform player3;
    [SerializeField] private RectTransform player4;

    Transform player1Transform;
    Transform player2Transform;
    Transform player3Transform;
    Transform player4Transform;

    public override void Awake()
    {
        base.Awake();
        m_cam = cameraObjectTransform.GetComponent<Camera>();
    }

    void Update()
    {
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 1000);
        SetMarkerPosition(southMarkerTransform, Vector3.back * 1000);
        SetMarkerPosition(eastMarkerTransform, Vector3.right * 1000);
        SetMarkerPosition(westMarkerTransform, Vector3.left * 1000);

        //SetMarkerPosition(northEastMarkerTransform, (Vector3.forward + Vector3.right) / 2 * 1000);
        //SetMarkerPosition(northWestMarkerTransform, (Vector3.forward + Vector3.left) / 2 * 1000);
        //SetMarkerPosition(southWestMarkerTransform, (Vector3.back - Vector3.right) / 2 * 1000);
        //SetMarkerPosition(southEastMarkerTransform, (Vector3.back - Vector3.left) / 2 * 1000);

        if (objectiveObjectTransform == null)
        {
            objectiveMarkerImage.SetActive(false);
        }
        if(objectiveObjectTransform != null)
        {
            objectiveMarkerImage.SetActive(true);
            SetMarkerPosition(objectiveMarkerTransform, objectiveObjectTransform.position);
        }

        if (player1.gameObject.activeSelf && player1Transform) SetMarkerPosition(player1, player1Transform.position);
        if (player2.gameObject.activeSelf && player2Transform) SetMarkerPosition(player2, player2Transform.position);
        if (player3.gameObject.activeSelf && player3Transform) SetMarkerPosition(player3, player3Transform.position);
        if (player4.gameObject.activeSelf && player4Transform) SetMarkerPosition(player4, player4Transform.position);
    }

    public override void OnPlayerUpdated()
    {
        TimerManager.RunAfterTime(() =>
        {
            List<PlayerData> players = GameManager.Instance.players;
            for (int i = 0; i < players.Count; i++)
            {
                players[i].player.GetComponentInChildren<CompassBar>().UpdatePlayerTransforms();
            }
        }, 0.1f);
    }
    public void UpdatePlayerTransforms()
    {
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        player3.gameObject.SetActive(false);
        player4.gameObject.SetActive(false);

        List<PlayerData> players = GameManager.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].player == player) continue;

            int playerNum = players[i].player.GetComponent<PlayerInput>().user.index + 1;
            Transform targetTransform = players[i].player.GetComponent<PlayerMovement>().rb.transform;
            if (playerNum == 1)
            {
                player1.gameObject.SetActive(true);
                player1Transform = targetTransform;
            }
            else if (playerNum == 2)
            {
                player2.gameObject.SetActive(true);
                player2Transform = targetTransform;
            }
            else if (playerNum == 3)
            {
                player3.gameObject.SetActive(true);
                player3Transform = targetTransform;
            }
            else if (playerNum == 4)
            {
                player4.gameObject.SetActive(true);
                player4Transform = targetTransform;
            }
        }
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPos)
    {
            Vector3 directionToTarget = worldPos - cameraObjectTransform.position;
            float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
            float compassPosX = Mathf.Clamp((2 * angle / m_cam.fieldOfView) / m_squashDivider, -1, 1);
            markerTransform.anchoredPosition = new Vector2(compassTransform.rect.width / 2 * compassPosX, 0);
    }
}
