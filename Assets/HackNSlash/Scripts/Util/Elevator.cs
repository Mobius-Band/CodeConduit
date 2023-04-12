using System;
using System.Collections;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Util
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private Transform _player;
        private String _elevatorScene1 = "Part4-2-1";
        private String _elevatorScene2 = "Part4-2-2";
        private float _upPosition;
        private float _downPosition;
        private bool _canUseElevator;
        private static bool IsDown
        {
            get => GameManager.Instance.ElevatorState.isDown;
            set => GameManager.Instance.ElevatorState.isDown = value;
        }

        private static bool StartOnElevator
        {
            get => GameManager.Instance.ElevatorState.startOnElevator;
            set => GameManager.Instance.ElevatorState.startOnElevator = value;
        }

        private void Start()
        {
            StartOnElevator = false;
            
            if (SceneManager.GetActiveScene().name == _elevatorScene1)
            {
                _upPosition = 10;
                _downPosition = 0;
                // only for this build
                transform.position = new Vector3(transform.position.x, _downPosition, transform.position.z);
            }
            else if (SceneManager.GetActiveScene().name == _elevatorScene2)
            {
                _upPosition = 0;
                _downPosition = -10;
                // only for this build
                transform.position = new Vector3(transform.position.x, _upPosition, transform.position.z);
            }

            if (!StartOnElevator)
            {
                _canUseElevator = true;
                _player.GetComponent<Rigidbody>().isKinematic = false;
                return;
            }
        
            // starting on the elevator:
            
            if (IsDown)
            {
                transform.position = new Vector3(transform.position.x, _upPosition, transform.position.z);
                StartCoroutine(ElevatorGoDown());
            }
            else
            {
                transform.position = new Vector3(transform.position.x, _downPosition, transform.position.z);
                StartCoroutine(ElevatorGoUp());
            }

            //_canUseElevator = false;
            //_player.SetParent(transform);
            //_player.GetComponent<Rigidbody>().isKinematic = true;
            //_player.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        }

        private void ElevatorActivate()
        {
            if (IsDown)
            {
                StartCoroutine(ElevatorGoUp());
            }
            else
            {
                StartCoroutine(ElevatorGoDown());
            }
        }

        private IEnumerator PlayerEnterElevator()
        {
            _player.SetParent(transform);
            _player.GetComponent<PlayerMovement>().SuspendMovement();
            _player.GetComponent<PlayerMovement>().SuspendRotation();
            
            // sometimes this works, but then the elevator's DOMove doesn't
            
            //_player.GetComponent<Rigidbody>().DOMove(new Vector3(
            //    transform.position.x, _player.transform.position.y, transform.position.z), 1);
        
            ElevatorActivate();
        
            yield return new WaitForSeconds(_time);
        
            if (IsDown) { IsDown = false; } 
            else { IsDown = true; }
            
            ChangeScene();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //StartOnElevator = true;
                
                if (_canUseElevator)
                {
                    StartCoroutine(PlayerEnterElevator());
                }
            }
        }

        private IEnumerator ElevatorGoUp()
        {
            _player.GetComponent<Rigidbody>().isKinematic = true;
            transform.DOMove(new Vector3(0, _upPosition, 0), _time);

            yield return new WaitForSeconds(_time + 0.5f);
            
            _player.GetComponent<Rigidbody>().isKinematic = false;
        }

        private IEnumerator ElevatorGoDown()
        {
            _player.GetComponent<Rigidbody>().isKinematic = true;
            transform.DOMove(new Vector3(0, _downPosition, 0), _time);
            
            yield return new WaitForSeconds(_time + 0.5f);
            
            _player.GetComponent<Rigidbody>().isKinematic = false;
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
}