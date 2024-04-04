using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LLMUnity;
using TMPro;
using System;
using Random = System.Random;

public class GameManager : MonoBehaviour
{   
    // Text interaction
    public List<string> pathologyList = new List<string>() { "de sida", "intolerante al gluten", "con esquizofrenia", "embarazado", "con alergia al marisco" };
    public LLM llm;
    public TMP_InputField playerText;
    public TMP_Text AIText;
    public GameObject AIBaloon;

    // Avatar animation 
    public GameObject avatar; 
    private Animator avatarAnimator;
    private Vector3 initial_pos_avatar = new Vector3(-7f, 0f, -4.755545f);
    private Vector3 finish_pos_avatar = new Vector3(0f, 0f, -4.755545f);
    public float current_speed = 4.7f;
    private bool postion_reached = false;

    // Start is called before the first frame update
    void Start()
    {
        // Prepare text interaction 
        setLLMPrompt();
        playerText.onSubmit.AddListener(onInputFieldSubmit);
        playerText.Select();
        playerText.interactable = false;
        AIBaloon.SetActive(false);

        // Prepare avatar animation 
        avatar.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        avatar.transform.position = initial_pos_avatar;
        avatarAnimator = avatar.GetComponent<Animator>();
        //current_speed = 0f;
        avatarAnimator.SetFloat("Speed", current_speed);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Math.Abs(avatar.transform.position.x - finish_pos_avatar.x) > 0.05)
        {
            current_speed = 4.666f;
            avatarAnimator.SetFloat("Speed", current_speed);
            avatar.transform.position += new Vector3(current_speed * Time.fixedDeltaTime, 0f, 0f);
            avatar.transform.eulerAngles += new Vector3(0f, 57.333f*Time.fixedDeltaTime, 0f);
        }
        else
        {
            if (!postion_reached)
            {
                onAvatarPlaced();
                postion_reached = true;
            }
            current_speed = 0.0f;
            avatarAnimator.SetFloat("Speed", current_speed);
        }

    }

    void setLLMPrompt()
    {
        Random rand;
        rand = new System.Random();
        int rand_n = rand.Next(0, pathologyList.Count);
        string prompt = "Una conversación entre un paciente " + pathologyList[rand_n]+ " (aún no sabe que lo es) y el médico. Las respuestas deben ser de menos de 100 carácteres.";
        llm.SetPrompt(prompt, false);
    }
    public void onAvatarPlaced()
    {
        playerText.interactable = true;
    }

    void onInputFieldSubmit(string message)
    {
        playerText.interactable = false;
        AIBaloon.SetActive(true);
        AIText.text = "...";
        _ = llm.Chat(message, SetAIText, AIReplyComplete);
    }

    public void SetAIText(string text)
    {
        AIText.text = text;
    }

    public void AIReplyComplete()
    {
        playerText.interactable = true;
        playerText.Select();
        playerText.text = "";
    }


}
