using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    [SerializeField] private List<GameObject> items;
    [SerializeField] private AudioClip sound;

    private bool canWork = false;
    private Collider2D player;

    private void Update()
    {
        if (canWork)
            Work();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            canWork = true;

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            canWork = false;
    }

    private void Work()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SoundManager.instance.PlaySound(sound);
            Instantiate(items[UnityEngine.Random.Range(0, items.Count)], transform.position, Quaternion.identity);
            Destroy(gameObject.GetComponentInParent<Transform>().gameObject);
        }
    }
}
