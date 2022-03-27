using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GA ga;
    public Text GenerateTime_text;
    public Text BestIndividual_text;
    public float distance = 15f;

    public Individual bestIndividual = new Individual();
    public Individual bestCurIndividual = new Individual();
    public int generate = 0;

    // Update is called once per frame
    void Update()
    {
        GenerateTime_text.text = "Generate: " + generate.ToString();

        BestIndividual();

        DrawLine(bestIndividual, Color.green);
        DrawLine(bestCurIndividual, Color.red);

    }

    private void DrawLine(Individual Individual, Color color) {
        for (int i = 0; i < Individual.CityPath.Count; i++) {
            if (i != Individual.CityPath.Count - 1)
                if(color == Color.red) {
                    Debug.DrawLine(Individual.CityPath[i].transform.position, Individual.CityPath[i + 1].transform.position, color);

                }
                //Vẽ ảo
                else {
                    Debug.DrawLine(Individual.CityPath[i].transform.position + Vector3.right*distance, Individual.CityPath[i + 1].transform.position + Vector3.right*distance, color);
                }
            else {
                if(color == Color.red) {
                    Debug.DrawLine(Individual.CityPath[i].transform.position, Individual.CityPath[0].transform.position, color);
                }
                //Vẽ ảo
                else {
                    Debug.DrawLine(Individual.CityPath[i].transform.position + Vector3.right*distance, Individual.CityPath[0].transform.position + Vector3.right*distance, color);
                }
            }

        }
    }

    private void BestIndividual() {
        //get best current individual
        Individual bestCurrentIndividual = new Individual();
        foreach (var x in ga.CurrentPopulation) {
            if (bestCurrentIndividual.fitness > x.fitness) {
                bestCurrentIndividual = x;
            }
        }

        //for testing
        bestCurIndividual = new Individual(bestCurrentIndividual.CityPath, bestCurrentIndividual.fitness);

        //print best current individual
        BestIndividual_text.text = "Best current Individual (fitness: " + bestCurrentIndividual.fitness + "):\n";
        foreach (var x in bestCurrentIndividual.CityPath) {
            BestIndividual_text.text += x.name + "->";
        }

        //get best individual
        if (bestIndividual.fitness > bestCurrentIndividual.fitness) {
            bestIndividual = new Individual(bestCurrentIndividual.CityPath, bestCurrentIndividual.fitness);
        }
        
        //print best individual
        BestIndividual_text.text += "\n\nBest Individual (fitness: " + bestIndividual.fitness + "):\n";
        foreach (var x in bestIndividual.CityPath) {
            BestIndividual_text.text += x.name + "->";
        }
    }

}
