using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public float startingScore = 1000;
    public float defaultScoreSpeed = -1;
    public float noRoadScoreSpeed = -10000;

    int numOfRoadsTouching;

    float score;

    bool counting;

    HUDScript hud;

    Camera[] cameras;

    // Start is called before the first frame update
    void Start()
    {
        counting = false;
        numOfRoadsTouching = 0;
        score = startingScore;

        hud = FindObjectOfType<HUDScript>();

        cameras = FindObjectsOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            if (numOfRoadsTouching > 0) score += defaultScoreSpeed * Time.deltaTime;
            else score += noRoadScoreSpeed * Time.deltaTime;
            Debug.Log("score: " + score);
            Debug.Log("roads: " + numOfRoadsTouching);
        }

        if (score <= 0) StopCounting();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Road")
        {
            numOfRoadsTouching++;
        }

        if (other.tag == "Finish")
        {
            StopCounting();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Road")
        {
            numOfRoadsTouching--;
        }
    }

    public void StartCounting()
    {
        counting = true;
    }

    public void StopCounting()
    {
        counting = false;
        hud.ShowScore(score);
        StartCoroutine(QuitGame());
    }

    public float getScore()
    {
        return score;
    }

    public void addScore(float valueToAdd)
    {
        score += valueToAdd;
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1f);
        switchCameras();
        yield return new WaitForSeconds(4f);
        Application.Quit();
    }

    public void switchCameras()
    {
        foreach(Camera camera in cameras)
        {
            if (camera.tag == "MainCamera") camera.enabled = false;
        }
    }
}
