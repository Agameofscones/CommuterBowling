using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirArrowManagerClown : MonoBehaviour
{
    [SerializeField]
    private GameObject[] dirClown;
    [SerializeField]
    private playerClown player;//reference to the player, duh

    public bool arrowsOut = false;

    private void Start() {
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerClown>();
        }
    }
    public void toggleChildren(bool B) {
        for (int i = 0; i < dirClown.Length; i++) {
            Debug.Log("Toggling children in arrow manager: " + B);
            dirClown[i].SetActive(B);
        }
    }
    public void toggleBallMode() {
        for (int i = 0; i < dirClown.Length; i++) {
            dirClown[i].GetComponent<dirArrowClown>().ballMode = !dirClown[i].GetComponent<dirArrowClown>().ballMode;
            dirClown[i].GetComponent<dirArrowClown>().updateSprite();
        }
    }

}
