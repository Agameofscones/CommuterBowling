using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuCycle : MonoBehaviour
{
    public GameObject[] pages;
    public int pagePos = 1;

    private void Start() {
        for(int i = 0; i < pages.Length; i++) {
            pages[i].SetActive(false);
        }
        cyclePage(0);
    }
    public void cyclePage(int P) {
        pagePos = P;
        if(pagePos < 0) {
            pagePos = pages.Length;//loop us back to the top!
        }
        if(pagePos > pages.Length) {
            pagePos = 0;//loop us back to the bottom!
        }
        for(int i = 0; i < pages.Length; i++) {
            if(i == pagePos) {
                pages[i].SetActive(true);
            } else {
                pages[i].SetActive(false);
            }
        }
    }
}
