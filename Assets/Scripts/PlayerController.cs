using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class PlayerController : MonoBehaviour {

//    [SerializeField] private CurveControlledBob HeadBob = new CurveControlledBob();

    public float speed;
//    private Camera Camera;
//    private Vector3 OriginalCameraPosition;
    private CharacterController charaCon;		// キャラクターコンポーネント用の変数
    private Vector3 move = Vector3.zero;
    private const float gravity = 9.8f;
    private float rotateSpeed = 90f;

    // Use this for initialization
    void Start() {
        charaCon = GetComponent<CharacterController>();
//        Camera = Camera.main;
//        OriginalCameraPosition = Camera.transform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        playerMove_1rdParson();
    }

    // ■■■１人称視点の移動■■■
    private void playerMove_1rdParson() {
        // ▼▼▼移動量の取得▼▼▼
        float y = move.y;
        // 上下のキー入力を取得し、移動量に代入.
        move = new Vector3(0f, 0f, Input.GetAxis("Vertical") * speed);
        move = transform.TransformDirection(move);
        //重力を与える
        move.y += y;
        move.y -= gravity * Time.deltaTime;


        // ▼▼▼プレイヤーの向き変更▼▼▼
        Vector3 playerDir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        playerDir = transform.TransformDirection(playerDir);
        if (playerDir.magnitude > 0.1f) {
            Quaternion q = Quaternion.LookRotation(playerDir);          // 向きたい方角をQuaternionn型に直す .
            // 向きを q に向けてじわ～っと変化させる.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);

        }

        // ▼▼▼移動処理▼▼▼
        charaCon.Move(move * Time.deltaTime);   // プレイヤー移動.
    }
/*
    //エネミーに衝突したら終了
    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "Enemy") {
            Application.LoadLevel("GameOver");
        }
    }
*/

    /* 歩行時の揺れ　未実装

        private void FixedUpdate() {

            UpdateCameraPosition(speed);

        }

        private void UpdateCameraPosition(float speed) {
            Vector3 newCameraPosition;
            if (charaCon.velocity.magnitude > 0) {
                Camera.transform.localPosition = new Vector3(0f, 0.1f, 0f);

                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = Camera.transform.localPosition.y;
            } else {

                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = OriginalCameraPosition.y;
            }
            Camera.transform.localPosition = newCameraPosition;
        }

    */
}