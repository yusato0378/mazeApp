using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public GameObject target;
    private CharacterController controller;
    private float speed =  1f;
    private float gravity = 20.0f;
    private Vector3 moveDirection;
    private Vector3 targetDirection;
    private bool isEnable;

    private float rotationSmooth = 1f;

    private Vector3 targetPosition;

    private float changeTargetSqrDistance = 0.1f;


    public enum AIType {
        Wander,
        Pursuit
    }

    public AIType aiType;

    // Use this for initialization
    void Start() {
        //これは敵の認識範囲にいるかどうかのフラグ
        isEnable = false;
        //さっきaddConponentしたCharacterControllerを呼び出しています
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
        aiType = AIType.Wander;

        targetPosition = GetRandomPositionOnLevel();

    }


    void Update() { 

        switch(aiType) {
            case AIType.Wander:
                EnemyWander();
                break;
            case AIType.Pursuit:
                EnemyPursuit();
                break;
        }
    }
    void EnemyWander() {
        // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
        //position.y を常に1に更新
        Vector3 enemyPos = transform.position;
        enemyPos.y = 1f;
        transform.position = enemyPos;
        float sqrDistanceToTarget = Vector3.SqrMagnitude((transform.position - targetPosition) * Time.deltaTime);
        if (sqrDistanceToTarget < changeTargetSqrDistance) {
            targetPosition = GetRandomPositionOnLevel();
        }

        // 目標地点の方向を向く
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

        // 前方に進む
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public Vector3 GetRandomPositionOnLevel() {
        //フロアマップサイズ
        float levelSize = 3f;
        return new Vector3(Random.Range(-levelSize, levelSize), 0f, Random.Range(-levelSize, levelSize));
    }

    void EnemyPursuit() {
        //認識範囲にいれば
        if (isEnable == true) {

            //mobの位置と自分の位置の差分をとって距離をはかり、0でなければ
            if (Vector3.Distance(transform.position, target.transform.position) != 0) {
                //プレイヤーの位置をtargetDirectionに入れて
                targetDirection = target.transform.position;
                //敵が浮かない様に縦座標は0に固定します
                targetDirection.y = 1f;
                //敵の向きを自分に向かせます
                //                  transform.LookAt(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection - transform.position), Time.deltaTime * speed);

                //敵のスピードと重力か次の位置を計算して移動させます
                moveDirection += transform.forward * 0.1f;
                moveDirection.y -= gravity * Time.deltaTime;
                controller.Move(moveDirection * Time.deltaTime * 0.5f);
            }
            else {
                isEnable = false;
            }
        }

    }


    /**
     * 円の範囲に入ったときのハンドラ
     */
    void OnTriggerEnter(Collider colobj) {
            //円に入ったのがPlayerのタグがついているオブジェクトなら
            if (colobj.tag == "Player") {
                //isEnableをtrueにして入ったオブジェクトをtarget変数に入れます
                isEnable = true;
                aiType = AIType.Pursuit;
                target = colobj.gameObject;
            }

        }

        /**
         * 円の範囲外に出た時のハンドラ
         */
        void OnTriggerExit(Collider colobj) {
            //出たのがPlayerタグがついているオブジェクトなら
            if (colobj.tag == "Player") {
                //フラグをfalseにする
                isEnable = false;
                aiType = AIType.Wander;

            }
        }

//        void OnControllerColliderHit(ControllerColliderHit hit) {
//            if (hit.tag == "Wall" && controller.velocity.magnitude == 0) {
//                targetPosition = GetRandomPositionOnLevel();
//            }
//        }


}
