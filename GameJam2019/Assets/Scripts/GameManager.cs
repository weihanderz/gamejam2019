using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Completed
{
	using System.Collections.Generic;
	using UnityEngine.UI;
    public class GameManager : MonoBehaviour
    {

        private BoardManager boardScript;
        // Use this for initialization
        void Start()
        {
            boardScript = GetComponent<BoardManager>();
            boardScript.SetupScene(1);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}