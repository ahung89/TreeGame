using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {

    public Camera playerCamera;
    public InteractionDetector interactionDetector;
    public PickupHolder pickupHolder;
    public float lookSpeedMultiplier;

    public UnityEvent lookRotationEvent;

    public GameObject startUI;
    public PauseUI pauseUI;

    Movement movement;
    float cameraRotation = 0;
    bool gameStarted = false;

    bool gamePaused = false;
    enum PauseOption
    {
        Restart = 0,
        Resume = 1,
        Quit = 2
    }
    private PauseOption currentPauseOption = PauseOption.Resume;

	void Start () {
        movement = GetComponent<Movement>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraRotation = playerCamera.transform.rotation.eulerAngles.x;
        Debug.Assert(pauseUI != null, "Player must have a reference to the Pause UI component for Pause functionality to work.");
    }
	
	void Update ()
    {
        if (gamePaused) // Game is currently in the pause screen
        {
            // Hitting Esc resumes the game
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Resume();
            }
            // Hitting enter executes the current pause option
            else if (Input.GetKeyUp(KeyCode.Return))
            {
                // Debug.Log("Return");
                switch (currentPauseOption)
                {
                    case PauseOption.Restart:
                        Restart();
                        break;
                    case PauseOption.Resume:
                        Resume();
                        break;
                    case PauseOption.Quit:
                        Quit();
                        break;
                }
            }
            // Hitting the arrow keys (including WASD) scrolls selection through Pause Menu options
            else
            {
                bool selectionSwitched = false;
                if (Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
                {
                    currentPauseOption = (PauseOption)(((int)currentPauseOption - 1 + 3) % 3);
                    // Debug.Log("Current Pause Option: " + currentPauseOption);
                    selectionSwitched = true;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow))
                {
                    currentPauseOption = (PauseOption)(((int)currentPauseOption + 1) % 3);
                    // Debug.Log("Current Pause Option: " + currentPauseOption);
                    selectionSwitched = true;
                }

                if (selectionSwitched)
                {
                    switch (currentPauseOption)
                    {
                        case PauseOption.Restart:
                            pauseUI.SelectRestart();
                            break;
                        case PauseOption.Resume:
                            pauseUI.SelectResume();
                            break;
                        case PauseOption.Quit:
                            pauseUI.SelectQuit();
                            break;
                    }
                }
            }
        }
        else if (gameStarted) // Primary game loop state is active
        {
            if (Input.GetKeyUp(KeyCode.Escape) && !SequenceTracker.Instance.flutePlayed) // Basically, player should not be able to pause the cinematic
            {
                Pause();
            }
            else
            {
                float forward = Input.GetAxisRaw("Vertical");
                float strafe = Input.GetAxisRaw("Horizontal");

                float lookX = Input.GetAxis("Mouse X");
                float lookY = Input.GetAxis("Mouse Y");

                bool interact = Input.GetMouseButtonDown(0);
                bool pickup = Input.GetMouseButtonDown(0);

                cameraRotation = Mathf.Clamp(cameraRotation - lookY, -89, 89);
                playerCamera.transform.localRotation = Quaternion.AngleAxis(cameraRotation, Vector3.right);

                Vector3 moveVec = Vector3.forward * forward + Vector3.right * strafe;
                movement.Move(moveVec.normalized);

                bool tryPickup = true;

                if (interact)
                {
                    tryPickup = interactionDetector.PerformInteractions();
                }

                if (pickup && tryPickup)
                {
                    // Debug.Log("Now Try PickUp");
                    pickupHolder.TryPickup();
                }

                if (lookY > 0 || lookX > 0)
                {
                    lookRotationEvent.Invoke();
                }

                movement.AddToYaw(lookX);
            }
        }
        else if (Input.GetMouseButtonDown(0)) // Game has not yet started and player left-clicks
        {
            startUI.SetActive(false);
            gameStarted = true;
        }
	}

    private void Pause()
    {
        // Debug.Log("Pause Game");
        gamePaused = true;
        pauseUI.gameObject.SetActive(true);
        currentPauseOption = PauseOption.Resume;
        // Debug.Log("Current Pause Option: " + currentPauseOption);
        // Time.timeScale = 0f; // A number of scripts and effects rely on time, and we will just let them play out
        foreach (Rigidbody rb in FindObjectsOfType<Rigidbody>())
        {
            rb.Sleep();
        }
    }

    private void Resume()
    {
        // Debug.Log("Resume Game");
        gamePaused = false;
        pauseUI.gameObject.SetActive(false);
        currentPauseOption = PauseOption.Resume;
        // Time.timeScale = 1f; // A number of scripts and effects rely on time, and we will just let them play out
        foreach (Rigidbody rb in FindObjectsOfType<Rigidbody>())
        {
            rb.WakeUp();
        }
    }

    private void Restart()
    {
        // Debug.Log("Restart Game");
        // MusicManager.Instance = null;
        MusicManager.Instance.ResetMusicLayers();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Quit()
    {
        // Debug.Log("Quit Game");
        Application.Quit();
    }
}
