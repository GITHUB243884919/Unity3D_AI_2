using UnityEngine;
using System.Collections;

public class IdelRunCtr : MonoBehaviour 
{
    private Animator m_animator = null;
	// Use this for initialization
	void Start () 
    {
        m_animator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_animator == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Press W");
            m_animator.SetBool("isRun", !m_animator.GetBool("isRun"));
        }
	
	}
}
