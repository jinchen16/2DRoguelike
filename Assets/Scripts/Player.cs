using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

	public float restartLevelDelay = 1f;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public int wallDamage = 1;

	private Animator animator;
	private int food;

	// Use this for initialization
	protected override void Start (){
		animator = GetComponent<Animator> ();

		food = GameManager.instance.playerFoodPoints;

		base.Start ();
	}

	private void OnDisable(){
		GameManager.instance.playerFoodPoints = food;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void AttemptMove <T> (int xDir, int yDir){
		food--;
		base.AttemptMove <T> (xDir, yDir);
		RaycastHit2D hit;
		if (Move (xDir, yDir, out hit)) {
		}
		CheckIfGameOver ();
		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove <T> (T component){
		Wall hitWall = component as Wall;
		hitWall.DamageWall (wallDamage);
		animator.SetTrigger ("playerChop");
	}

	private void Restart(){
		SceneManager.LoadScene (SceneManager.sceneLoaded);
	}

	private void Losefood(int loss){
		animator.SetTrigger ("playerHit");
		food -= loss;
		CheckIfGameOver ();
	}

	private void CheckIfGameOver(){
		if (food <= 0) {
			GameManager.instance.GameOver ();
		}
	}

	private void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (col.tag == "Food") {
			food += pointsPerFood;
			col.gameObject.SetActive (false);
		} else if (col.tag == "Soda") {
			food += pointsPerSoda;
			col.gameObject.SetActive (false);
		}
	}
}
