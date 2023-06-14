using Eflatun.SceneReference;
using HackNSlash.ScriptableObjects;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.GamePlayFlowManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioClip menuTrack;
        [SerializeField] private AudioClip[] physicalWorldTracks;
        [SerializeField] private AudioClip[] digitalWorldTracks;
        [SerializeField] private SceneReference[] newTrackScenes;
        [SerializeField] private SceneRefSO sceneRefSo;
        private AccessData _accessData;
        private AudioSource _audioSource;
        private AudioClip _currentClip;
        private bool _sceneVerified;
        private bool IsCurrentClipPlaying => _audioSource.clip == _currentClip;
        private int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            _accessData = GameManager.Instance.AccessData;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!_accessData) _accessData = GameManager.Instance.AccessData;
            if (!_audioSource) _audioSource = GetComponent<AudioSource>();
            
            if (SceneVerification() && CurrentSceneIndex != sceneRefSo.previousSceneIndex)
            {
                ChooseTrack();
            }
        }

        private void ChooseTrack()
        {
            // menu
            if (sceneRefSo.IsOnMainMenu())
            {
                PlayTrack(-1);
                return;
            }
            
            // part 1
            if (!_accessData.canAccessPart2)
            {
                PlayTrack(0);
                return;
            }

            // part 2
            if (_accessData.canAccessPart2 && !_accessData.canAccessPart3)
            {
                PlayTrack(1);
                return;
            }
            
            // part 3
            if (_accessData.canAccessPart3 && !_accessData.canAccessPart4)
            {
                PlayTrack(2);
                return;
            }
            
            // part 4
            if (_accessData.canAccessPart4)
            {
                PlayTrack(3);
            }
        }

        private void PlayTrack(int index)
        {
            if (_audioSource.clip == null && IsCurrentClipPlaying) return;
            
            _audioSource.Stop();
            if (sceneRefSo.IsOnMainMenu()) _audioSource.clip = menuTrack;
            if (sceneRefSo.IsOnPhysicalWorld) _audioSource.clip = physicalWorldTracks[index];
            if (sceneRefSo.IsOnDigitalWorld) _audioSource.clip = digitalWorldTracks[index];
            _audioSource.Play();
            _currentClip = _audioSource.clip;
        }

        private bool SceneVerification()
        {
            for (int i = 0; i < newTrackScenes.Length - 1; i++)
            {
                if (CurrentSceneIndex == newTrackScenes[i].BuildIndex)
                {
                    return true;
                }
            }

            return false;
        }
    }
}