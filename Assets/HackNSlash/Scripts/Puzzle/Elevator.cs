using System;
using System.Collections;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

namespace HackNSlash.Scripts.Puzzle
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float time;
        [SerializeField] private float finalPlayerRotation = -90;
        private String _elevatorScene1 = "Part4-2-1-2";
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
                player.GetComponent<Rigidbody>().isKinematic = false;
                return;
            }
        
            // starting on the elevator:
            if (player)
            {
                StartCoroutine(PlayerStartOnElevator());
            }
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
                player.GetComponent<Rigidbody>().DOMove(new Vector3(
                    transform.position.x, player.transform.position.y, transform.position.z), time/2));
            
            sequence.Append(
                player.transform.DORotate(new Vector3(
                    player.rotation.x, finalPlayerRotation, player.rotation.z), time/2));
            
            sequence.Append(transform.DOMove(new Vector3(transform.position.x, direction, transform.position.z), time));
            
            DOTween.Play(sequence);
        }

        private IEnumerator PlayerEnterElevator()
        {
            player.SetParent(transform);
            player.GetComponent<PlayerMovement>().SuspendMovement();
            player.GetComponent<PlayerMovement>().SuspendRotation();
            player.GetComponent<Rigidbody>().isKinematic = true;

            ElevatorActivate();

            StartOnElevator = true;
            
            yield return new WaitForSeconds(time * 2);
        
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
            player.SetParent(transform);
            player.GetComponent<PlayerMovement>().SuspendMovement();
            player.GetComponent<PlayerMovement>().SuspendRotation();
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

            ElevatorActivate();
            
            yield return new WaitForSeconds(time * 2);
            
            player.GetComponent<PlayerMovement>().RegainMovement();
            player.GetComponent<PlayerMovement>().RegainRotation();
            player.GetComponent<Rigidbody>().isKinematic = false;
            
            yield return new WaitForSeconds(time);

            StartOnElevator = false;
            _canUseElevator = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_canUseElevator)
                {
                    if (other.GetComponent<PlayerPickupSphere>().IsHoldingSphere)
                    {
                        return;
                    }
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