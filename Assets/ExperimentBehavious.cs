using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Timeline;

public class ExperimentBehavious : MonoBehaviour
{
    private ArduinoCom p_arduinoCom;
    private float timer = 0f;

    [Tooltip("LOG FILE")]
    [SerializeField]
    // string path = "";


    public enum Experiment
    {
        REACTION_TIME = 1,
        RIGHT_LEFT = 2,
        HIDE_AND_SEEK = 3,
    }

    [SerializeField]
    private Experiment p_experiment = Experiment.REACTION_TIME;


    [SerializeField]
    // private bool m_next = false;

    [Tooltip("Experiment 1 : REACTION TIME")]
    private float startTime = -1f;
    public bool start = false;
    public bool sigSent = false;
    bool hit = false;

    void Start()
    {
        p_arduinoCom = GetComponent<ArduinoCom>();
    }

    void Update()
    {
        float dTime = Time.deltaTime;

        /* PHASE 1 : Reaction time
         * This phase will consist of testing the reaction time of the user.
         * When the test starts, at a random time (<5sec), a signal is given.
         * The user must press Space as quickly as possible.
         * If they press before the signal, the reaction time is negative.
         */
        if (p_experiment == Experiment.REACTION_TIME)
        {
            if (start)
            {
                p_arduinoCom.SendSig(0);
                float randTime = Random.Range(1f, 5f);
                startTime = Time.time + randTime;
                start = false;
                hit = false;
                Debug.Log("Pr�pare-toi...");
            }
      
            timer += dTime;

            if (Input.GetKeyDown(KeyCode.Space) && !hit)
            {
                float reactionTime = Time.time - startTime;
                Debug.Log("Temps de r�action : " + reactionTime.ToString("F3") + " secondes");
                p_arduinoCom.SendSig(0);
                hit = true;
            }

            if (Time.time >= startTime && !sigSent)
            {
                Debug.Log("MAINTENANT ! Appuie sur Espace !");
                sigSent = true;
            }
        }

        /* PHASE 2 : Left/Right
         * This test will determine if the changement of state is distinguishable.
         * we will use 2 point left a rest, we will then activate one or the other point and see if the subject
         * determine the one who changed.
         */
        else if (p_experiment == Experiment.RIGHT_LEFT)
        {

        }/* PHASE 3 : Hide And Seek
         * This test will determine if the subject can distinguish which point (among 7) is active on the 2D surface.
         */
        else if (p_experiment == Experiment.HIDE_AND_SEEK)
        {

        }
    }
}
