using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public PlayerDatabase   playerDatabase;

    [Header("Playerdata")]
    public TMP_Text playerName;
    public Transform modelParent;
    public Image image;
    private GameObject currentModel;
    void Start()
    {
        foreach(var dataFetch in playerDatabase.playerDetails)
        {
            print("Player Name : " + dataFetch.playerName);
            print("Model : " + dataFetch.player);
            print("Image : " + dataFetch.playerImage);

            //display players
            playerName.text = dataFetch.playerName;
            image.sprite = dataFetch.playerImage;
            if (currentModel != null)
                Destroy(currentModel);

            currentModel = Instantiate(
                dataFetch.player,
                modelParent.position,
                modelParent.rotation,
                modelParent
            );

        }
    }
}
