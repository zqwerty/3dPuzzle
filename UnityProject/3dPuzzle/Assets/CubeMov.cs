﻿using UnityEngine;
using System.Collections;

namespace Leap.Unity {

  /// <summary>
  /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
  /// allows rotation, translation, and scale of the object (RTS).
  /// </summary>
  public class CubeMov : MonoBehaviour {

    public enum RotationMethod {
      None,
      Single,
      Full
    }

    [SerializeField]
    private PinchDetector _pinchDetectorA;
    public PinchDetector PinchDetectorA {
      get {
        return _pinchDetectorA;
      }
      set {
        _pinchDetectorA = value;
      }
    }

    [SerializeField]
    private PinchDetector _pinchDetectorB;
    public PinchDetector PinchDetectorB {
      get {
        return _pinchDetectorB;
      }
      set {
        _pinchDetectorB = value;
      }
    }

    [SerializeField]
    private RotationMethod _oneHandedRotationMethod;

    [SerializeField]
    private RotationMethod _twoHandedRotationMethod;

    [SerializeField]
    private bool _allowScale = true;

    [Header("GUI Options")]
    [SerializeField]
    private KeyCode _toggleGuiState = KeyCode.None;

    [SerializeField]
    private bool _showGUI = true;

    private Transform _anchor;
    public bool isIn = false;
    public Rigidbody rb;
    public Rigidbody other;
    //public BoxCollider bc;

    private float _defaultNearClip;

    void Start() {
//      if (_pinchDetectorA == null || _pinchDetectorB == null) {
//        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
//        enabled = false;
//      }

      GameObject pinchControl = new GameObject("RTS Anchor");
      _anchor = pinchControl.transform;
      _anchor.transform.parent = transform.parent;
      transform.parent = _anchor;

      // rb = GetComponent<Rigidbody>();
      //bc = GetComponent<BoxCollider>();
    }

    void Update() {
      if (Input.GetKeyDown(_toggleGuiState)) {
        _showGUI = !_showGUI;
      }



      bool didUpdate = false;
      if(_pinchDetectorA != null)
        didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
      if(_pinchDetectorB != null)
        didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

      if (didUpdate) {
        transform.SetParent(null, true);
      }

      if (_pinchDetectorA != null && _pinchDetectorA.IsActive && 
          _pinchDetectorB != null &&_pinchDetectorB.IsActive) {
        transformDoubleAnchor();
      } else if (_pinchDetectorA != null && _pinchDetectorA.IsActive) {
        transformLeftAnchor(_pinchDetectorA);
      } else if (_pinchDetectorB != null && _pinchDetectorB.IsActive) {
        transformRightAnchor(_pinchDetectorB);
      } else{
      	// rb.isKinematic = false;
      	// if(rb) rb.isKinematic = isIn;
      }

      if (didUpdate) {
        transform.SetParent(_anchor, true);
      }
    }

    void OnGUI() {
      if (_showGUI) {
        GUILayout.Label("One Handed Settings");
        doRotationMethodGUI(ref _oneHandedRotationMethod);
        GUILayout.Label("Two Handed Settings");
        doRotationMethodGUI(ref _twoHandedRotationMethod);
        _allowScale = GUILayout.Toggle(_allowScale, "Allow Two Handed Scale");
      }
    }

    private void doRotationMethodGUI(ref RotationMethod rotationMethod) {
      GUILayout.BeginHorizontal();

      GUI.color = rotationMethod == RotationMethod.None ? Color.green : Color.white;
      if (GUILayout.Button("No Rotation")) {
        rotationMethod = RotationMethod.None;
      }

      GUI.color = rotationMethod == RotationMethod.Single ? Color.green : Color.white;
      if (GUILayout.Button("Single Axis")) {
        rotationMethod = RotationMethod.Single;
      }

      GUI.color = rotationMethod == RotationMethod.Full ? Color.green : Color.white;
      if (GUILayout.Button("Full Rotation")) {
        rotationMethod = RotationMethod.Full;
      }

      GUI.color = Color.white;

      GUILayout.EndHorizontal();
    }

    private void transformDoubleAnchor() {
      _anchor.position = (_pinchDetectorA.Position + _pinchDetectorB.Position) / 2.0f;

      switch (_twoHandedRotationMethod) {
        case RotationMethod.None:
          break;
        case RotationMethod.Single:
          Vector3 p = _pinchDetectorA.Position;
          p.y = _anchor.position.y;
          _anchor.LookAt(p);
          break;
        case RotationMethod.Full:
          Quaternion pp = Quaternion.Lerp(_pinchDetectorA.Rotation, _pinchDetectorB.Rotation, 0.5f);
          Vector3 u = pp * Vector3.up;
          _anchor.LookAt(_pinchDetectorA.Position, u);
          break;
      }

      if (_allowScale) {
        _anchor.localScale = Vector3.one * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
      }
    }

    private void transformLeftAnchor(PinchDetector singlePinch) {
	
	Debug.Log("x="+singlePinch.Position.x+"y="+singlePinch.Position.y+"z="+singlePinch.Position.z+"isIn="+isIn);
      _anchor.position =  singlePinch.Position;
	  
      switch (_oneHandedRotationMethod) {
        case RotationMethod.None:
          break;
        case RotationMethod.Single:
          Vector3 p = singlePinch.Rotation * Vector3.right;
          p.y = _anchor.position.y;
          _anchor.LookAt(p);
          break;
        case RotationMethod.Full:
          _anchor.rotation = singlePinch.Rotation;
          break;
      }

      _anchor.localScale = Vector3.one;
    }

    private void transformRightAnchor(PinchDetector singlePinch) {
	
	if(!rb.isKinematic) return;
	// Debug.Log("x="+singlePinch.Position.x+"y="+singlePinch.Position.y+"z="+singlePinch.Position.z+"isIn="+isIn);
      _anchor.position =  singlePinch.Position;
	  
      switch (_oneHandedRotationMethod) {
        case RotationMethod.None:
          break;
        case RotationMethod.Single:
          Vector3 p = singlePinch.Rotation * Vector3.right;
          p.y = _anchor.position.y;
          _anchor.LookAt(p);
          break;
        case RotationMethod.Full:
          _anchor.rotation = singlePinch.Rotation;
          break;
      }

      _anchor.localScale = Vector3.one;
    }

    void OnTriggerEnter(Collider collider) {
        Debug.Log("OnTriggerEnter");
        isIn = true;
        // rb.detectCollisions = true;
        // super.OnTriggerEnter(collider);
        // rb.isKinematic = true;
    }
 
    // 接触结束
    void OnTriggerExit(Collider collider) {
        Debug.Log("OnTriggerExit");
        isIn = false;
        // rb.isKinematic = false;
    }
 
    // 接触持续中
    void OnTriggerStay(Collider collider) {
        // Debug.Log("OnTriggerStay");
        isIn = true;
    }

    // 碰撞开始
    void OnCollisionEnter(Collision collision) {
        // 销毁当前游戏物体
        // Destroy(this.gameObject);
        Debug.Log("OnCollisionEnter"+this.name);
        var tag = collision.collider.tag;
        Debug.Log(tag);
        if(tag!="cube"){
        	// 人手碰撞：选定当前方块
        	rb.isKinematic = true;
        	other.isKinematic = false;
        }
        
    }
 
    // 碰撞结束
    void OnCollisionExit(Collision collision) {
 		Debug.Log("OnCollisionExit"+this.name);
 		//rb.isKinematic = false;
 		// isIn = false;
    }
 
    // 碰撞持续中
    void OnCollisionStay(Collision collision) {
 		// Debug.Log("OnCollisionStay");
 		// rb.isKinematic = true;

    }

  }
}
