using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraOperator : MonoBehaviour {
	
	//public float zoomMaxY = 0;
	//public float zoomMinY = 0;
	public float zoomSpeed = 0.05f;
	public float zoomTime = 0.25f;
	public float rotateSpeed;
	
	public GameObject target;
	
	public float zoomDestination = 0;
	
	// Update is called once per frame
	void Update () 
	{
		ZoomCamera();
		
		//Vector3 rot = transform.rotation.eulerAngles;
		
	    if(Input.GetKey(KeyCode.PageUp))
	    {
	        transform.RotateAround(target.transform.position, Vector3.right, Time.deltaTime * rotateSpeed);
	    }
 
		if(Input.GetKey (KeyCode.PageDown))
		{
			 transform.RotateAround(target.transform.position, Vector3.right, Time.deltaTime * -rotateSpeed);
		}
		if(Input.GetKey (KeyCode.Home))
		{
			 transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * rotateSpeed);
		}
		if(Input.GetKey (KeyCode.End))
		{
			 transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * -rotateSpeed);	
		}
		if(Input.GetKey (KeyCode.Insert))
		{
			 transform.Translate(Vector3.forward * zoomSpeed);	
		
		}
		if(Input.GetKey (KeyCode.Delete))
		{
			 transform.Translate(Vector3.forward *-zoomSpeed);	
		}

	}
	
	private void ZoomCamera()
	{
		float moveY = Input.GetAxis("Mouse ScrollWheel");
		
		transform.position += new Vector3(0, moveY * zoomSpeed, 0);
		
		if(moveY != 0)
		{
			zoomDestination = moveY * zoomSpeed;
		}
		
		transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, moveY * zoomSpeed, 0), zoomTime);
		
		/*if(transform.position.y > zoomMaxY)
			transform.position = new Vector3(transform.position.x, zoomMaxY, transform.position.z);
		if(transform.position.y < zoomMinY)
			transform.position = new Vector3(transform.position.x, zoomMinY, transform.position.z);*/
	}
}
