
    var lookSensitivity : float= 5;
	var yRotation: float;
	var xRotation: float;
	var currentYRotation: float;
	var currentXRotation: float;
	var yRotationV: float ;
	var xRotationV : float;
	var lookSmoothDamp = 0.1f;
	

	
	// Update is called once per frame
	function Update () {
	 if (networkView.isMine){
	   yRotation +=Input.GetAxis("Mouse X") * lookSensitivity;
	   xRotation -=Input.GetAxis("Mouse Y") * lookSensitivity;
	   
		xRotation = Mathf.Clamp(xRotation,-90,90);
		
		
	   currentXRotation = Mathf.SmoothDamp(currentXRotation,xRotation,xRotationV,lookSmoothDamp);
	   currentYRotation = Mathf.SmoothDamp(currentYRotation,yRotation,yRotationV,lookSmoothDamp);
	   transform.rotation = Quaternion.Euler(currentXRotation,currentYRotation,0);	
	}} 