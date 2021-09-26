using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{ 
	public Rigidbody rb;
	public AudioClip starSound;
	private AudioSource playerAudio;
	public float speed = 15;
	
	public bool isTraveling;
	public bool hasTrailstar;
	
	public ParticleSystem fireworkParticle;
	
	private Vector3 travelDirection;
	private Vector3 nextCollisionPosition;
	
	public int minSwipeRecognition = 500;
	private Vector2 swipePosLastFrame;
	private Vector2 swipePosCurrentFrame;
	private Vector2 currentSwipe;
	
	
	//This changes the color when the ball moves
	private Color solveColor;
	
	private void Start() 
	{
		solveColor = Random.ColorHSV(0.5f, 1);
		GetComponent<MeshRenderer>().material.color = solveColor;
		playerAudio=GetComponent<AudioSource>();
		fireworkParticle=GetComponent<ParticleSystem>();
	}
	
	private void FixedUpdate()
	{
		//Only move ball if it's travelling
		if(isTraveling)
		{
		rb.velocity = speed * travelDirection;
		}
		//Create a small sphere underneath our ball to notify us when it hit something
		Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
		
		int i = 0;
		while(i < hitColliders.Length)
		{
			GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
			if(ground && !ground.isColored)
			{
				ground.ChangeColor(solveColor);
			}
			i++;
		}
		
		//The ball should stop moving but change direction when it reaches a wall
		if(nextCollisionPosition != Vector3.zero)
		{
			if(Vector3.Distance(transform.position, nextCollisionPosition) < 1)
			 {	
			   isTraveling = false;
			   travelDirection = Vector3.zero;
			   nextCollisionPosition = Vector3.zero;
			 }
		}
		
		if (isTraveling)
			return;
		
		/* Swipe code begins */
		if (Input.GetMouseButton(0))
		{
		    //get the x and y coordinate of where our finger or mouse is
			swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			
			//gets the currentSwipe of users
			if (swipePosLastFrame != Vector2.zero)
			{
				currentSwipe = swipePosCurrentFrame - swipePosLastFrame;
				
				/* currentSwipe.sqrMagnitude create a square root value from our currentSwipe 
				 and compare it with minSwipeRecognition */
				  if (currentSwipe.sqrMagnitude < minSwipeRecognition)
				  {
					return;
				  }
				  
				  //get the direction of the currentSwipe i.e Left,Right,Up,Down
				  currentSwipe.Normalize();
				  
				  if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				  {
					 //Up/Down 
					 SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
				  }
				  if(currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				  {
					  //GO Left/Right
					  SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
				  }
			}
			
			swipePosLastFrame = swipePosCurrentFrame;
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			swipePosLastFrame = Vector2.zero;
			currentSwipe = Vector2.zero;
		}
		/* Swipe code ends here */
		
	}
  
          private void SetDestination(Vector3 direction)
		  {
			  travelDirection = direction;
			  //RaycastHit checks which object it will collide with
			  RaycastHit hit;
			  if (Physics.Raycast(transform.position, direction, out hit, 100f))
			  {
				  nextCollisionPosition = hit.point;
			  }
			  
			  isTraveling = true;
		  }
		  
		  //Trailstar display firework when in contact with ball
		  private void OnTriggerEnter(Collider other) 
		    {
			  if(other.gameObject.CompareTag("Trailstar"))
			  {
				  fireworkParticle.Play();
				  hasTrailstar = true;
				  Destroy(other.gameObject);
				  playerAudio.PlayOneShot(starSound, 1.0f);
			  }
		  }
		  
}
