using UnityEngine;
using System.Collections;
using System;
public class CubeCorrect03 : MonoBehaviour {
 
    public GameObject Cube;
    public GameObject Cube03;
    public Vector3 oriPos;
    public Vector3 oriRota;

    void Start() {
        Cube = GameObject.Find("Cube");
        Cube03 = GameObject.Find("Cube03");
    }
    void OnMouseUp(){
        print(Cube03);
        int flag = 0;
        for (int i = 0; i < 361; i += 90){
            if (Math.Abs(Cube03.transform.localEulerAngles.x - i) < 15){
                oriRota.x = i;
                flag ++;
                break;
            }
        }
        for (int i = 0; i < 361; i += 90){
            if (Math.Abs(Cube03.transform.localEulerAngles.y - i) < 15){
                oriRota.y = i;
                flag ++;
                break;
            }
        }
        for (int i = 0; i < 361; i += 90){
            if (Math.Abs(Cube03.transform.localEulerAngles.z - i) < 15){
                oriRota.z = i;
                flag ++;
                break;
            }
        }
        oriPos = Cube03.transform.localPosition;
        if (Math.Abs(oriPos.x - 0.25f) < 0.12){
            oriPos.x = 0.25f;
            flag ++;
        }
        if (Math.Abs(oriPos.x + 0.25f) < 0.12){
            oriPos.x = -0.25f;
            flag ++;
        }
        if (Math.Abs(oriPos.y - 0.25f) < 0.12){
            oriPos.y = 0.25f;
            flag ++;
        }
        if (Math.Abs(oriPos.y + 0.25f) < 0.12){
            oriPos.y = -0.25f;
            flag ++;
        }
        if (Math.Abs(oriPos.z - 0.25f) < 0.12){
            oriPos.z = 0.25f;
            flag ++;
        }
        if (Math.Abs(oriPos.z + 0.25f) < 0.12){
            oriPos.z = -0.25f;
            flag ++;
        }
        if (flag == 6){
            Cube03.transform.localEulerAngles = oriRota;
            Cube03.transform.localPosition = oriPos;
        }
    }
}