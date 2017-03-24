
//
//Filename: CameraControl.cs
//
using UnityEngine;

[AddComponentMenu("Camera-Control/CameraControl")]
public class CameraControl : MonoBehaviour
{


	Camera _camera;


	public  float _rate = .5f; //rate of camera movement

	




	// Mouse buttons in the same order as Unity
	public enum MouseButton
	{
		Left = 0,
		Right = 1,
		Middle = 2,
		None = 3
	}
	
	[System.Serializable]
		// Handles left keyAlternate keys (Alt, Ctrl, Shift)
    public class KeyAlternate
	{
		
		public bool w;
		public bool a;
		public bool s;
		public bool d;
		
		public bool checkKeyAlternate ()
		{
		
			return ((!w ^ Input.GetKey (KeyCode.W)) &&
                (!a ^ Input.GetKey (KeyCode.A)) &&
                (!s ^ Input.GetKey (KeyCode.S)) &&
                (!d ^ Input.GetKey (KeyCode.D)));
		}
	}

	public class ArrowAlternate
	{
		public bool upArrow;
		public bool downArrow;
		public bool leftArrow;
		public bool rightArrow;
		
		public bool checkArrows ()
		{
			
			return ((!upArrow ^ Input.GetKey (KeyCode.UpArrow)) &&
                (!downArrow ^ Input.GetKey (KeyCode.DownArrow)) &&
                (!leftArrow ^ Input.GetKey (KeyCode.LeftArrow)) &&
                (!rightArrow ^ Input.GetKey (KeyCode.RightArrow)));
		}
	}

	[System.Serializable]
		// Handles common parameters for translations and rotations
    public class ControlConfiguration
	{

		public bool activate;
		public MouseButton mouseButton;
		public KeyAlternate keyAlternate;
		public ArrowAlternate alternate;
		public float sensitivity;
	
		public bool isActivated ()
		{
		
	
			
			return activate && Input.GetMouseButton ((int)mouseButton) && keyAlternate.checkKeyAlternate () && (alternate.checkArrows ());
		}
	}

	[System.Serializable]
		// Handles scroll parameters
    public class ScrollConfiguration
	{

		public bool activate;
		public KeyAlternate keyAlternate;
		public ArrowAlternate alternate;
		public float sensitivity;

		public bool isActivated ()
		{
			//if (MSettings.VerboseLogging) Debug.Log("Checl");
			//if (MSettings.VerboseLogging) Debug.Log(alternate.checkArrows ());
			//if (MSettings.VerboseLogging) Debug.Log(keyAlternate.checkKeyAlternate ());
			return activate ;
		
		}
	}


	// Vertical translation default configuration
	public ControlConfiguration verticalTranslation = new ControlConfiguration {activate = true, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{},
		alternate = new  ArrowAlternate{}, sensitivity = 1f };
	public ControlConfiguration verticalTranslationUp = new ControlConfiguration {activate = true, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{w=true},
		alternate = new  ArrowAlternate{ } };
	public ControlConfiguration verticalTranslationDown = new ControlConfiguration {activate = true, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{s=true},
		alternate = new  ArrowAlternate{}};
	// Horizontal translation default configuration
	public ControlConfiguration horizontalTranslation = new ControlConfiguration {activate = true, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{},
		alternate = new  ArrowAlternate{}, sensitivity = 1f };
	public ControlConfiguration horizontalTranslationLeft = new ControlConfiguration {activate = true, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{a=true }, 
		alternate = new  ArrowAlternate{} };
	public ControlConfiguration horizontalTranslationRight = new ControlConfiguration {activate = true, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{d=true}, 
		alternate = new  ArrowAlternate{} };
	
	// Depth (forward/backward) translation default configuration use scroll
	public ControlConfiguration depthTranslation = new ControlConfiguration {activate = false, mouseButton = MouseButton.Middle, keyAlternate = new KeyAlternate{ }, 
		alternate = new  ArrowAlternate{}, sensitivity = 1f };
	

	// Scroll default configuration
	public ScrollConfiguration scroll = new ScrollConfiguration { activate = true, sensitivity = 1f };

	// Default unity names for mouse axes
	public string mouseHorizontalAxisName = "Mouse X";
	public string mouseVerticalAxisName = "Mouse Y";
	public string scrollAxisName = "Mouse ScrollWheel";
	
	public float rotateSpeed = 5.0f;
	public float moveSpeed = 1.0f;
/*	public float rotationX = 10.0f;
	public float rotationY = 10.0f;
	*/
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;


	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;


	
	void Update ()
	{
		if (Input.GetMouseButton (1)) {
			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") *2*_rate;
				
				rotationY += Input.GetAxis("Mouse Y") * 4*_rate;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * _rate, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * _rate;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
				
			}
			transform.position += transform.forward * moveSpeed * Input.GetAxis ("Vertical")*_rate;
			transform.position += transform.right * moveSpeed * Input.GetAxis ("Horizontal")*_rate;
		}
		/*if (Input.GetMouseButton (1)) {
			rotationX += Input.GetAxis ("Mouse X") * rotateSpeed*_rate;
			rotationY += Input.GetAxis ("Mouse Y") * rotateSpeed*_rate;
			rotationY = Mathf.Clamp (rotationY, -90, 90);
       		if (MSettings.VerboseLogging) Debug.Log(rotationX + " " + rotationY);
			if (MSettings.VerboseLogging) Debug.Log(transform.rotation.eulerAngles.ToString());
			transform.localRotation = Quaternion.AngleAxis (rotationX, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis (rotationY, Vector3.left);
   
			transform.position += transform.forward * moveSpeed * Input.GetAxis ("Vertical")*_rate;
			transform.position += transform.right * moveSpeed * Input.GetAxis ("Horizontal")*_rate;
   		
		}*/

			if (verticalTranslation.isActivated ()) {
				float translateY = Input.GetAxis (mouseVerticalAxisName) * verticalTranslation.sensitivity * _rate;
				transform.Translate (0, translateY, 0);
			}
			if (verticalTranslationUp.isActivated ()) {
				float translateY = verticalTranslation.sensitivity * _rate;
				transform.Translate (0, translateY, 0);
			}
			if (verticalTranslationDown.isActivated ()) {
				float translateY = -verticalTranslation.sensitivity * _rate;
				transform.Translate (0, translateY, 0);
			}
			if (horizontalTranslation.isActivated ()) {
				float translateX = Input.GetAxis (mouseHorizontalAxisName) * horizontalTranslation.sensitivity * _rate;
				transform.Translate (translateX, 0, 0);
			}
			
			if (horizontalTranslationLeft.isActivated ()) {
				float translateX = -horizontalTranslation.sensitivity;
				transform.Translate (translateX, 0, 0);
			}
			if (horizontalTranslationRight.isActivated ()) {
				float translateX = horizontalTranslation.sensitivity;
				transform.Translate (translateX, 0, 0);
			}
		 


			

		
	}

	

}