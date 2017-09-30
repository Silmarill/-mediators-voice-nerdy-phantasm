﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
// TODO Сделать и потестировать другой вариант джойстика - круг, разделенный на 4 части
public class TouchControls : MonoBehaviour {

    private PlayerController _p;
    private LevelLoader levelExit;
    private PauseMenu pausMenu;
    private float xAxis;
    private float yAxis;
    private bool isMoving;
    public float axisShift;
    public float xLadderAxis;
    // Use this for initialization
    void Start () {
        #if UNITY_STANDALONE || UNITY_WEBPLAYER
        gameObject.SetActive(false);
        return;
        #endif 
        _p = FindObjectOfType <PlayerController>();
        levelExit = FindObjectOfType <LevelLoader>();
        pausMenu  = FindObjectOfType <PauseMenu>();
    }

    private void Update() {
        ///<summary>
        /// Горизонтальная и вертикальная ось джойстика
        ///</summary>
        xAxis = CrossPlatformInputManager.GetAxis("Horizontal");
        yAxis = CrossPlatformInputManager.GetAxis("Vertical");
        AxisCheck();
    }


    ///<summary>
    /// Движение и проверка на лестницы. На лестнице нужно увести джойстик по горизонтали
    /// в более дальнее положение
    ///</summary>
    private void AxisCheck() {
        if (xAxis == 0) {
            _p.Move(0);
        }
        if (yAxis == 0) {
            _p.Climb(0);
        }
        if (xAxis == 0 && yAxis == 0) {
            isMoving = false;
        }
        else {
            isMoving = true;
        }

        if (!isMoving) return;

        if (yAxis > 0.3f) {
            if (levelExit.isPlayerInZone) {
                levelExit.LoadLevel();
            }
        }

        if (_p.isOnLadder) {
            if (yAxis > 0) {
                _p.Climb(1);
                _p.Move(0);
                if (xAxis > xLadderAxis) {
                    _p.Move(1);
                    return;
                }
                else if (xAxis < -xLadderAxis) {
                    _p.Move(-1);
                    return;
                }
            }
            if (yAxis < -axisShift) {
                _p.Climb(-1);
                if (xAxis > xLadderAxis) {
                    _p.Move(1);
                    return;
                }
                else if (xAxis < -xLadderAxis) {
                    _p.Move(-1);
                    return;
                }
            }
            return;
        }
        if (xAxis > axisShift) {
            _p.Move(1);
        }
        if (xAxis < -axisShift) {
            _p.Move(-1);
        }
    }

    public void UpArrow() {
     _p.Climb(1);
    }

    public void DownArrow() {
     _p.Climb(-1);
    }
    public void ResetClimb() {
     _p.Climb(0);
    }

    // Update is called once per frame
    public void LeftArrow () {
        _p.Move(-1);
    }

    public void RightArrow () {
        _p.Move(1);
    }

    public void UnpressedArrow () {
        _p.Move(0);
    }


    public void Sword () {
        _p.Sword();
    }

    public void ResetSword () {
        _p.SwordReset();
    }


    public void Fire () {
        _p.Fire();
    }

    public void Jump () {
        _p.Jump();
        if (levelExit.isPlayerInZone) {
            levelExit.LoadLevel();
        }
    }

    public void Pause() {
        pausMenu.TogglePause();
        Messenger.Broadcast("PauseStatus", true);
    }

}
