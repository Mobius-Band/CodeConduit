using System;
using System.Collections;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sequence = DG.Tweening.Sequence;

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
            DOTween.Init();
            
            if (SceneManager.GetActiveScene().name == _elevatorScene1)
            {
                _upPosition = 10;
                _downPosition = 0;
            }
            else if (SceneManager.GetActiveScene().name == _elevatorScene2)
            {
                _upPosition = 0;
                _downPosition = -10;
            }

            if (!StartOnElevator)
            {
                _canUseElevator = true;
                _player.GetComponent<Rigidbody>().isKinematic = false;
                return;
            }
        
            // starting on the elevator:
            StartCoroutine(PlayerStartOnElevator());
        }

        private void ElevatorActivate()
        {
            _canUseElevator = false;
            
            float direction;

            if (StartOnElevator)
            {
                if (IsDown) { direction = _downPosition; }
                else { direction = _upPosition; }
            }
            else
            {
                if (IsDown) { direction = _upPosition; }
                else { direction = _downPosition; }
            }
            

            // using a dotween sequence to make the player enter the elevator and then go up
            
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(
                _player.GetComponent<Rigidbody>().DOMove(new Vector3(
                    transform.position.x, _player.transform.position.y, transform.position.z), _time/2));
            
            sequence.Append(
                _player.transform.DORotate(new Vector3(
                    _player.rotation.x, 0, _player.rotation.z), _time/2));
            
            sequence.Append(transform.DOMove(new Vector3(0, direction, 0), _time));
            
            DOTween.Play(sequence);
        }

        private IEnumerator PlayerEnterElevator()
        {
            _player.SetParent(transform);
            _player.GetComponent<PlayerMovement>().SuspendMovement();
            _player.GetComponent<PlayerMovement>().SuspendRotation();
            _player.GetComponent<Rigidbody>().isKinematic = true;

            ElevatorActivate();

            StartOnElevator = true;
            
            yield return new WaitForSeconds(_time * 2);
        
            if (IsDown) { IsDown = false; } 
            else { IsDown = true; }
            
            ChangeScene();
        }

        private IEnumerator PlayerStartOnElevator()
        {
            if (IsDown)
            {
                transform.position = new Vector3(transform.position.x,_upPosition, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x,_downPosition, transform.position.z);
            }
            
            _canUseElevator = false;
            _player.SetParent(transform);
            _player.GetComponent<PlayerMovement>().SuspendMovement();
            _player.GetComponent<PlayerMovement>().SuspendRotation();
            _player.GetComponent<Rigidbody>().isKinematic = true;
            _player.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

            ElevatorActivate();
            
            yield return new WaitForSeconds(_time * 2);
            
            _player.GetComponent<PlayerMovement>().RegainMovement();
            _player.GetComponent<PlayerMovement>().RegainRotation();
            _player.GetComponent<Rigidbody>().isKinematic = false;
            
            yield return new WaitForSeconds(_time);

            StartOnElevator = false;
            _canUseElevator = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_canUseElevator)
                {
                    StartCoroutine(PlayerEnterElevator());
                }
            }
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