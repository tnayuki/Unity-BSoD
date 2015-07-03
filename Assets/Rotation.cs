using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

	public Transform target;
	public  float angleSpeed = 30f;
	
	//回転の中心をとるために使う変数
	private Vector3 targetPos;
	
	
	void Start () {
		transform.LookAt(target);
		
//		//自分をZ軸を中心に0～360でランダムに回転させる
//		transform.Rotate(new Vector3(0, 0, Random.Range(0,360)),Space.World);	
	}
	
	void Update () {
		
		//	Sampleを中心に自分を現在の上方向に、毎秒angle分だけ回転する。
		transform.RotateAround(Vector2.zero, Vector3.up, angleSpeed * Time.deltaTime);
	}
}

