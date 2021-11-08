using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostball : MonoBehaviour
{
    private Renderer rendo;
    public float fadetime = 3f;
    public float fadespeed = 3f;
    private bool fading = false;
    private void Awake() {
        if(rendo == null) {
            rendo = this.GetComponent<Renderer>();
        }
    }
    void Start()
    {
        StartCoroutine(fadeOut(fadetime));//we're basically a glorified particle. But I don't know how to use particles in unity. :)
    }
    private void Update() {
        if (fading) {
            transform.localScale = transform.localScale * 1.0005f;
            Color ObjectColor = rendo.material.color;
            ObjectColor.a -= (fadespeed * Time.deltaTime * fadespeed);
            if (ObjectColor.a <= 0) {
                Destroy(this.gameObject);
            }
            rendo.material.color = ObjectColor;//yes this is stupid!
            
        }
    }

    IEnumerator fadeOut(float T) {
        fading = true;
        yield return new WaitForSeconds(T);
        Destroy(this);
    }
}
