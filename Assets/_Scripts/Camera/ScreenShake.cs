using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {
    static public ScreenShake Instance;
    
	[SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0f,1f,-1f,-1f), new Keyframe(1f,0f,-1f,-1f));
    [SerializeField] private bool relativeToCam = false;
    [SerializeField] private Vector3 strength = Vector2.one;
    [SerializeField] private float multiplier = 1f; //default multiplier
    [SerializeField] private float time = 0.2f;
	[Range(0,7)][SerializeField] private int roundDecimals = 7;

    private Transform m_Transform;
    private Coroutine m_ShakeCoroutine;
    private Vector3 lastMovement = Vector3.zero; //the amound of random shake done last frame


    void Awake(){
        Instance = this;
		m_Transform = this.transform;

	}
	void OnValidate(){
		if(time < 0f){time = 0f;}
	}
    
	public void Shake(){ //call default  
		Shake(multiplier);
	}
	public void Shake(float multi){ //call default 
		if(m_ShakeCoroutine != null){
			StopCoroutine(m_ShakeCoroutine);
		}
		m_ShakeCoroutine = StartCoroutine(DoShake(multi));
	}
	IEnumerator DoShake (float multi) {
		float timer = 0f;
		while(timer < time){
			m_Transform.localPosition -= lastMovement; //move back
			Vector3 myStrength = new Vector3(curve.Evaluate(timer / time) * multi * strength.x,
									curve.Evaluate(timer / time) * multi * strength.y,
									curve.Evaluate(timer / time) * multi * strength.z);
			if(relativeToCam){
				lastMovement = m_Transform.localRotation * new Vector3(Random.Range(-myStrength.x,myStrength.x), 
															Random.Range(-myStrength.y,myStrength.y), 
															Random.Range(-myStrength.z,myStrength.z));//relative to camera's rotation
			}else{
				lastMovement = new Vector3(Random.Range(-myStrength.x,myStrength.x), 
											Random.Range(-myStrength.y,myStrength.y), 
											Random.Range(-myStrength.z,myStrength.z));
			}
			if(roundDecimals != 7){
				lastMovement.x = (float)System.Math.Round(lastMovement.x, roundDecimals);
				lastMovement.y = (float)System.Math.Round(lastMovement.y, roundDecimals);
				lastMovement.z = (float)System.Math.Round(lastMovement.z, roundDecimals);
			}
			
			m_Transform.localPosition += lastMovement;
			
			timer += Time.unscaledDeltaTime; //count up using unscaled time.
			yield return null;
		}
		m_Transform.localPosition -= lastMovement; //move back
		lastMovement = Vector3.zero;
	}
}