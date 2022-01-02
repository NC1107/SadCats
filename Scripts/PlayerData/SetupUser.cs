using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BreakInfinity;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerData.Database;

namespace PlayerData
{
    public class SetupUser : MonoBehaviour
    {
        [Header("Input Fields")] public TMP_InputField userInputField;
        public TMP_InputField passInputField;

        [Header("TextBoxes")] public TMP_Text errorTextBox;

        [Header("Options")] public bool showDebugPrintouts = true;
        //access-type object-type var-name

        //database accessor
        private Database _db;


        private void Start()
        {
            _db = new Database(FirebaseDatabase.DefaultInstance.RootReference);
            _db.ShowDebug(showDebugPrintouts);
        }

        public void OnButtonClick()
        {
            if (!CheckBoxes()) return;
            if (!_db.LoginUser(userInputField.text, passInputField.text)) return;
            _db.SavePlayer(userInputField.text);
            SceneManager.LoadScene("Game");
        }

        private bool CheckBoxes()
        {
            errorTextBox.text = "";
            if (userInputField.text == null)
            {
                errorTextBox.text += "Invalid Username\n";
            }

            if (passInputField.text == null)
            {
                errorTextBox.text += "Invalid Password\n";
            }

            return errorTextBox.text == "";
        }

      
    }
}