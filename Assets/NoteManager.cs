using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public Animation generate;
    public Animation random;
    public Animation reload;

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown("g")) {
            generate.Play();
        }
        if (Input.GetKeyDown("r")) {
            random.Play();

        }
        if (Input.GetKeyDown("l")) {
            reload.Play();

        }
    }
}
