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
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Transform player;
        [SerializeField] private float time;
        [SerializeField] private float finalPlayerRotation = -90;
        [SerializeField] private float upPosition;
        [SerializeField] private float downPosition;
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
                if (IsDown) { direction = downPosition; }
                else { direction = upPosition; }
            }
            else
            {
                if (IsDown) { direction = upPosition; }
                else { direction = downPosition; }
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
                transform.position = new Vector3(transform.position.x,upPosition, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x,downPosition, transform.position.z);
            }
            
            _canUseElevator = false;
            player.SetParent(transform);
            player.GetComponent<PlayerMovement>().SuspendMovement();
            player.GetComponent<PlayerMovement>().SuspendRotation();
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

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
                    if (other.GetComponent<PlayerPickupSphere>().isHoldingSphere)
                    {
                        return;
                    }
                    StartCoroutine(PlayerEnterElevator());
                }
            }
        }

        private void ChangeScene()
        {
            if (SceneManager.GetActiveScene().name == gameManager.playerElevatorSceneUp)
            {
                print("a");
                SceneManager.LoadScene(gameManager.playerElevatorSceneDown);
            }
            else if (SceneManager.GetActiveScene().name == gameManager.playerElevatorSceneDown)
            {
                SceneManager.LoadScene(gameManager.playerElevatorSceneUp);
            }
        }
    }
}