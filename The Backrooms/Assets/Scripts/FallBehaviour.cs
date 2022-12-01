using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBehaviour : MonoBehaviour
{
    public Animator fallAnim;
    public GameObject player;

    public GameManager gameManager;

    public Transform spawnPoint;

    public float waitTime;

    // OnTriggerEnter est appelé quand le Collider other entre dans le déclencheur
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(flipFlop());
        }
    }

    private IEnumerator flipFlop()
    {
        fallAnim.enabled = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Camera>().GetComponent<MouseLook>().enabled = false;
        yield return new WaitForSeconds(waitTime);
        fallAnim.enabled = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponentInChildren<Camera>().GetComponent<MouseLook>().enabled = true;
        TeleportToBackrooms();
    }

    private void TeleportToBackrooms()
    {
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }
}
