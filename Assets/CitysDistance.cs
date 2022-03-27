using UnityEngine;

[System.Serializable]
public class CitysDistance{
    public string name;
    public GameObject city1; 
    public GameObject city2; 
    public float distance;//khoảng cách

    public CitysDistance(GameObject a, GameObject b, float dis){
        this.name = a.name + "--" + b.name;
        this.city1 = a;
        this.city2 = b;
        this.distance = dis;
    }
}