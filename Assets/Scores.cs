using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Scores : MonoBehaviour {
    string line = "";
    public Text puan;
    string puanlar;
    int i = 0;
    // Use this for initialization
    void Start () {
        using (StreamReader sr = new StreamReader("HighScore.txt"))
        {
            while ((line = sr.ReadLine()) != null)
            {
                if (i> 9)
                    break;
                i++;

                puanlar += line;
            }
            puan.text = puanlar;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
