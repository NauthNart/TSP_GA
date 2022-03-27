using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GA : MonoBehaviour
{
    [Header("INPUT------------------")]
    public List<GameObject> citys;
    public int soLuongCaTheTrongQuanThe;
    public int soLuongCaTheDuocDotBien;
    public float tiLeNhanGenTroi;
    private List<CitysDistance> citysDistance;

    [Header("DOING------------------")]
    public List<Individual> CurrentPopulation;
    public List<Individual> bestCurrentGen;
    public List<Individual> listChildren;

    UI_Manager UI_manager;

    //tính toán fitness
    private float CalculateFitness(Individual indiv){
        float fitness = 0;
        
        //tính toán khoảng cách các city
        for(int i=0; i<indiv.CityPath.Count-1; i++){
            var indiv_a = indiv.CityPath[i];
            var indiv_b = indiv.CityPath[i + 1];
            foreach(var x in citysDistance){
                if (x.city1 == indiv_a && x.city2 == indiv_b ||
                    x.city2 == indiv_a && x.city1 == indiv_b){
                        fitness += x.distance;
                    }
            }
        }

        //thêm khoảng cách từ city đầu và city cuối
        var indiv_last = indiv.CityPath[indiv.CityPath.Count - 1];
        var indiv_first = indiv.CityPath[0];
        foreach (var x in citysDistance) {
            if (x.city1 == indiv_last && x.city2 == indiv_first ||
                x.city2 == indiv_last && x.city1 == indiv_first) {
                fitness += x.distance;
            }
        }

        return fitness;
    }

    //so sánh xem 2 thành phố có phải khác nhau hay không 
    private bool IsDifference(Vector2 city1, Vector2 city2) {
        return (city1.x != city2.x && city1.y != city2.y);
    }

    //tạo ra danh sách các khoảng cách của từng thành phố
    List<CitysDistance> InitCitysDistance(List<GameObject> citys) {
        List<CitysDistance> _listDistance = new List<CitysDistance>();
        for (int i = 0; i < citys.Count - 1; i++) {
            for (int j = i + 1; j < citys.Count; j++) {
                CitysDistance temp = new CitysDistance(citys[i], citys[j], Vector2.Distance(citys[i].transform.position, citys[j].transform.position));
                _listDistance.Add(temp);
            }
        }

        return _listDistance;
    }

    // (1) Khởi tạo quần thể.
    private void CreatingInitialPopulation(){
        CurrentPopulation.Clear();

        for(int i=0; i<soLuongCaTheTrongQuanThe; i++){
            //tạo ra 1 individual
            Individual indiv = new Individual();
            for(int c=0; c<citys.Count; c++){
                // thêm ngẫu nhiên 1 city vào indiv
                int city = Random.Range(0, citys.Count);
                while(indiv.CityPath.Contains(citys[city])){
                    city = Random.Range(0, citys.Count);
                }
                indiv.CityPath.Add(citys[city]);
            }

            //xét trường hợp không được thêm trùng city vào CurrentPosulation!
            bool isContains = false;
            foreach(var x in CurrentPopulation) {
                if(x.IsSameCityPath(indiv)) {
                    isContains = true;
                    break;
                }
            }
            if (!isContains)
                CurrentPopulation.Add(indiv);
            else Debug.Log("TRUNG NHAU!");
        }
    }

    // (2) Tính toán fitness.
    private void CalculateFitnessForPopulation(){
        for(int i=0; i<CurrentPopulation.Count; i++){
            CurrentPopulation[i].fitness = CalculateFitness(CurrentPopulation[i]);
        }
    }

    //(3) lựa chọn các cá thể tốt.
    private void SelectingTheBestGenes() {
        bestCurrentGen.Clear();
        int soLuongCaTHeDuocChon = (int)((tiLeNhanGenTroi / 100f) * CurrentPopulation.Count);
        for(int i=0; i<soLuongCaTHeDuocChon; i++) {
            //lấy ra cá thể trội nhất
            var temp = new Individual();
            foreach (var x in CurrentPopulation) {
                if (x.fitness < temp.fitness && !bestCurrentGen.Contains(x)) {
                    temp = x;
                }
            }
            //thêm cá thể trội vừa lấy vào bestCurrentGen
            bestCurrentGen.Add(temp);
        }
    }

    //tạo ra cá thể con bằng việc trao đổi chéo từ 2 cá thể 
    private Individual GetChildrenOfMotherAndFather(Individual mother, Individual father) {
        //khởi tạo children
        Individual children = new Individual();
        for(int i=0; i<citys.Count; i++) {
            children.CityPath.Add(null);
        }

        //số lượng NST của mẹ
        int numberOf_Mother_NST = Random.Range(1, citys.Count);

        //get mother's NST
        for (int i=0; i<numberOf_Mother_NST; i++) {
            int getMotherNST = Random.Range(0, citys.Count);
            
            //for sure
            while(children.CityPath[getMotherNST] != null) {
                getMotherNST = Random.Range(0, citys.Count);
            }
            children.CityPath[getMotherNST] = mother.CityPath[getMotherNST];
        }

        //get father's NST
        foreach (var x in father.CityPath) {
            if (!children.CityPath.Contains(x)) {
                for (int i = 0; i < children.CityPath.Count; i++) {
                    if (children.CityPath[i] == null) {
                        children.CityPath[i] = x;
                        break;
                    }
                }
            }
        }

        //get fitness for children
        children.fitness = CalculateFitness(children);

        return children;
    }

    // (4) trao đổi chéo.
    private void CrossingOver() {
        listChildren.Clear();
        //                     5
        for(int i=0; i<soLuongCaTheTrongQuanThe; i++) {
            int pos_mother = Random.Range(0, bestCurrentGen.Count);
            int pos_father = Random.Range(0, bestCurrentGen.Count);

            //for sure
            while (pos_father == pos_mother)
                pos_father = Random.Range(0, bestCurrentGen.Count);
                
            //get children
            Individual children = new Individual();
            children = GetChildrenOfMotherAndFather(bestCurrentGen[pos_mother], bestCurrentGen[pos_father]);

            listChildren.Add(children);
        }
    }


    //đột biến cá thể 
    [Range(1, 10)] public int soNSTBiDotBien;
    private void MutivationChildren(Individual children) {
        Individual tempChild = new Individual(children.CityPath, children.fitness);

        for(int i=0; i<soNSTBiDotBien; i++) {
            //chọn 2 vị trí để tráo đổi NST
            int randomNST1 = Random.Range(0, citys.Count);
            int randomNST2 = Random.Range(0, citys.Count);
            //for sure
            while (randomNST1 == randomNST2) {
                randomNST2 = Random.Range(0, citys.Count);
            }

            //tráo đổi
            var temp = children.CityPath[randomNST1];
            children.CityPath[randomNST1] = children.CityPath[randomNST2];
            children.CityPath[randomNST2] = temp;

        }
        children.fitness = CalculateFitness(children);

        //nếu không tốt hơn ban đầu thì không đột biến
        //if(children.fitness > tempChild.fitness) {
        //    children = new Individual(tempChild.CityPath, tempChild.fitness);
        //}
    }

    // (5) Đột biến
    private void Mutivation() {
        for(int i=0; i<soLuongCaTheDuocDotBien; i++) {
            int childrenDuocChon = Random.Range(0, listChildren.Count);

            MutivationChildren(listChildren[childrenDuocChon]);
        }
    }

    private void Main() {
        // (3) lựa chọn các cá thể tốt.
        SelectingTheBestGenes();

        // (4) trao đổi chéo.
        CrossingOver();

        // (5) Đột biến
        Mutivation();

        //currentPopulation là listchidren sau khi đột biến:
        CurrentPopulation = listChildren;

    }

    private void Start(){
        UI_manager = FindObjectOfType<UI_Manager>();

        //if(SceneManager.GetActiveScene().name == "Random_Pos_Scene") {
        for(int i=0; i< citys.Count; i++) {
            citys[i].transform.position = new Vector2(Random.Range(-20f, -1f), Random.Range(-5f, 5f));
        }
        //}

        citysDistance = InitCitysDistance(citys);

        // (1) Khởi tạo quần thể.
        CreatingInitialPopulation();
        // (2) Tính toán fitness.
        CalculateFitnessForPopulation();
    }


    private void Update() {
        if (Input.GetKey("g")) {
            Main();
            UI_manager.generate += 1;
        }

        //xóa kinh nghiệm
        if (Input.GetKeyDown("l")) {
            bestCurrentGen.Clear();
            listChildren.Clear();
            UI_manager.bestCurIndividual = new Individual();
            UI_manager.bestIndividual = new Individual();
            UI_manager.generate = 0;
            
            CreatingInitialPopulation();
            CalculateFitnessForPopulation();
        }

        //reload scene
        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}