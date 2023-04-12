using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Eflatun.SceneReference;
using HackNSlash.Scripts.GameManagement;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float _time;
    private Transform _player;
    private String _elevatorScene1 = "Part4-2-1";
    private String _elevatorScene2 = "Part4-2-2";
    private float upPosition;
    private float downPosition;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == _elevatorScene1)
        {
            upPosition = 10;
            downPosition = 0;
        }
        else if (SceneManager.GetActiveScene().name == _elevatorScene2)
        {
            upPosition = 0;
            downPosition = -10;
        }
        
        if (GameManager.Instance.ElevatorState.isDown)
        {
            transform.position = new Vector3(transform.position.x, upPosition, transform.position.z);
            ElevatorGoDown();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, downPosition, transform.position.z);
            ElevatorGoUp();
        }
    }

    private void ElevatorActivate()
    {
        if (GameManager.Instance.ElevatorState.isDown)
        {
            ElevatorGoUp();
        }
        else
        {
            ElevatorGoDown();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject.transform;
            StartCoroutine(PlayerEnterElevator());
        }
    }

    private IEnumerator PlayerEnterElevator()
    {
        _player.SetParent(transform);
        _player.GetComponent<PlayerMovement>().SuspendMovement();
        _player.GetComponent<PlayerMovement>().SuspendRotation();
        //_player.GetComponent<Rigidbody>().DOMove(new Vector3(
        //    transform.position.x, _player.transform.position.y, transform.position.z), 1) 
        //    .OnKill(() => Debug.Log("movimento finalizado"));
        Vector3.Lerp(_player.transform.position,
            new Vector3(transform.position.x, _player.transform.position.y, transform.position.z), 1);
        ElevatorActivate();
        yield return new WaitForSeconds(2);
        ChangeScene();
    }

    private void ElevatorGoUp()
    {
        transform.DOMove(new Vector3(0, upPosition, 0), _time);
        GameManager.Instance.ElevatorState.isDown = false;
    }

    private void ElevatorGoDown()
    {
        transform.DOMove(new Vector3(0, downPosition, 0), _time);
        GameManager.Instance.ElevatorState.isDown = true;
    }

    private void ChangeScene()
    {
        if (SceneManager.GetActiveScene().name == _elevatorScene1)
        {
            SceneManager.LoadScene(_elevatorScene2);
        }
        else
        {
            SceneManager.LoadScene(_elevatorScene1);
        }
    }
}