using UnityEngine;
using System.Collections;

public class EnemyAI2 : MonoBehaviour {

    public Transform target; //プレイヤーの位置
    static Vector3 pos;
    NavMeshAgent agent;

    float agentToPatroldistance;
    float agentToTargetdistance;

 //   void Awake() {
 //       agent = GetComponent<NavMeshAgent>();
 //   }

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        DoPatrol();

    }

    // Update is called once per frame
    void Update () {
        //Agentと目的地の距離
        agentToPatroldistance = Vector3.Distance(this.agent.transform.position, pos);
        Debug.Log(agentToPatroldistance +  "目的地：" + pos + "敵位置" + this.agent.transform.position);

        //Agentとプレイヤーの距離
        agentToTargetdistance = Vector3.Distance(this.agent.transform.position, target.transform.position);


            //プレイヤーと目的地の距離が15f以下になると次の目的地をランダム指定
         if (agentToPatroldistance < 7f) {
            DoPatrol();
        }

        //プレイヤーと目的地の距離が15f以下になると次の目的地をランダム指定
        if (agentToTargetdistance < 3f) {
            Debug.Log("プレイヤーを追いかけとるよ" + agentToTargetdistance);
            DoTracking();
        }


    }



    //エージェントが向かう先をランダムに指定するメソッド
    public void DoPatrol() {
        float x = Random.Range(-10.0f, 10.0f);
        float z = Random.Range(-10.0f, 10.0f);
        pos = new Vector3(x, 1.1f, z);
        agent.SetDestination(pos);

    }
    
    //targetに指定したplayerを追いかけるメソッド
    public void DoTracking() {
        pos = target.position;
        agent.SetDestination(pos);

    }

    //エネミーに衝突したら終了
    void OnTriggerEnter(Collider colobj) {
        if (colobj.gameObject.tag == "Player") {
            Application.LoadLevel("GameOver");
        }
    }



    /**
 * 円の範囲に入ったときのハンドラ
    
    void OnTriggerEnter(Collider colobj) {
        //円に入ったのがPlayerのタグがついているオブジェクトなら
        if (colobj.tag == "Player") {
            Debug.Log("範囲に入った！！");
            DoTracking();
        }

    }

    /**
     * 円の範囲外に出た時のハンドラ
    
    void OnTriggerExit(Collider colobj) {
        //出たのがPlayerタグがついているオブジェクトなら
        if (colobj.tag == "Player") {
            Debug.Log("範囲から出た！！");

            DoPatrol();
        }
    }
    */
}
