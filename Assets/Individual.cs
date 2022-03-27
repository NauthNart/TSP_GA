using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Individual{
    public List<GameObject> CityPath;
    public float fitness;

    public Individual(){
        this.CityPath = new List<GameObject>();
        this.fitness = 9999;
    }    

    public Individual(List<GameObject> path, float fit){
        this.CityPath = path;
        this.fitness = fit;
    }

    public void SetFitness(float fit){
        this.fitness = fit;
    }

    public void AddCity(GameObject city){
        CityPath.Add(city);
    }

    public bool IsSameCityPath(Individual x) {
        int a = 0;
        for (int i = 0; i < this.CityPath.Count; i++) {
            if (this.CityPath[i] == x.CityPath[i]) {
                a++;
            }
        }

        if (a == this.CityPath.Count) {
            return true;
        }
        else {
            return false;
        }
    }
}