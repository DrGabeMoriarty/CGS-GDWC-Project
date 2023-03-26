using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_music : MonoBehaviour
{
    [SerializeField] private float timeLeft, originalTime,colorTime;
    [SerializeField] private GameObject objToSpawn;
    [SerializeField] private GameObject[] piano;
    [SerializeField] private Sprite hitkey,resetkey;

    private bool resume = true;
    public AudioSource song;

    public bool Mike = false;

    private int a;
    private int b;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (resume)
        {
            float[] y_pos = { 2.29f, -1.01f, -4.23f };
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {

                a = Random.Range(0, 3);
                b = Random.Range(0, 3);
                while(a  == b)
                {
                    b = Random.Range(0, 3);
                }

                Instantiate(objToSpawn, new Vector3(transform.position.x, y_pos[a],
                    transform.position.z), Quaternion.identity);
                if(Mike)
                    Instantiate(objToSpawn, new Vector3(transform.position.x, y_pos[b],
                        transform.position.z), Quaternion.identity);
                timeLeft = originalTime;
                StartCoroutine(changeColor());

            }
        }
    }

    private IEnumerator changeColor()
    {
        piano[a].GetComponent<SpriteRenderer>().sprite = hitkey;
        yield return new WaitForSeconds(colorTime);
        piano[a].GetComponent<SpriteRenderer>().sprite = resetkey;
    }

    public void Pause()
    {
        song.Pause();
        resume = false;
    }
    public void Resume()
    {
        song.UnPause();
        resume = true;
    }

}
