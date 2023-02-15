using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class SensorMovement : MonoBehaviour
{
    private int colCount = 0;
    private float disableTimer;

    private void OnEnable()
    {
        colCount = 0;
    }

    public bool State() => disableTimer > 0 ? false : colCount > 0;

    public void Disable(float duration) => disableTimer = duration;

    void OnTriggerEnter2D(Collider2D collision) => colCount++;

    void OnTriggerExit2D(Collider2D collision) => colCount--;

    void Update() => disableTimer -= Time.deltaTime;


    //private int m_ColCount = 0;

    //private float m_DisableTimer;

    //private void OnEnable()
    //{
    //    m_ColCount = 0;
    //}

    //public bool State()
    //{ 
    //    if (m_DisableTimer > 0)
    //        return false;
    //    return m_ColCount > 0;
    //}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    m_ColCount++; 
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    m_ColCount--;
    //}

    //void Update()
    //{
    //    m_DisableTimer -= Time.deltaTime;
    //}

    //public void Disable(float duration)
    //{
    //    m_DisableTimer = duration;
    //}
}

