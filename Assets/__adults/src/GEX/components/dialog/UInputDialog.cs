/*using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GEX.utils;
using Holoville.HOTween;

namespace GEX.components.dialog
{
    public class UInputDialog
    {
        private static Func<UDialogResult, string, int> _returnFunction;
        private static String game_object_path;
        private static GameObject mcontainer;
        private static GameObject container;
        private static Text title_output;
        private static Text msg_output;
        private static Text labelInput;
        private static UInputField inputField;
        private static UButton button_01;
        private static UButton button_02;
        private static UButton button_03;
        private static String[] buttons_str;
        private static UDialogTimer timer;
        private static int _tweenReferenceID;

        public enum Language
        {
            ENGLISH,
            SPANISH
        }

        public enum UDialogBoxButtons
        {
            AbortRetryIgnore,
            OK,
            OKCancel,
            RetryCancel,
            YesNo,
            YesNoCancel
        }

        public enum UDialogResult
        {
            Abort,
            Cancel,
            Ignore,
            No,
            None,
            OK,
            Retry,
            Yes
        }

        private static string[] buttons_str_english = new string[]{
            "Abort",
            "Cancel",
            "Ignore",
            "No",
            "OK",
            "Retry",
            "Yes"};

        private static string[] buttons_str_spanish = new string[]{
            "Abortar",
            "Cancelar",
            "Ignorar",
            "No",
            "OK",
            "Reintentar",
            "Si"};

        private const int _abort = 0;
        private const int _cancel = 1;
        private const int _ignore = 2;
        private const int _no = 3;
        private const int _ok = 4;
        private const int _retry = 5;
        private const int _yes = 6;

        public static void initialize(String gameObjectPath, Language language = Language.ENGLISH)
        {
            timer = new UDialogTimer();
            game_object_path = gameObjectPath;
            setLanguage(language);
        }

        public static void setLanguage(Language language)
        {
            if (language == Language.ENGLISH)
                buttons_str = buttons_str_english;
            else if (language == Language.SPANISH)
                buttons_str = buttons_str_spanish;
        }

        private static void _create()
        {
            mcontainer = Resources.Load("GAME_GUI/INPUT_DIALOG") as GameObject;
            mcontainer = XGUI.addGUI(mcontainer);
            mcontainer.name = "INPUT_DIALOG";


            container =     mcontainer.transform.FindChild("container").gameObject;
            title_output =  container.transform.FindChild("title").GetComponent<Text>();
            msg_output =    container.transform.FindChild("msg_rect").FindChild("msg").GetComponent<Text>();
            labelInput =    container.transform.FindChild("input/label").GetComponent<Text>();
            inputField =    container.transform.FindChild("input/USERNAME_INPUT").GetComponent<UInputField>();

            XGuiFX.setAlpha(mcontainer, 0);

        }

        private static void _hide()
        {
            XGuiFX.setAlpha(mcontainer, 0, 0.2f);
            timer.time = 0;
            _tweenReferenceID = HOTween.To(timer, 0.2f, new TweenParms().Prop("time", 2).OnComplete(onHideComplete)).intId;
        }

        private static void onHideComplete()
        {
            GameObject.Destroy(mcontainer);
            mcontainer = null;
        }

        private static void showButtons(int total)
        {
            GameObject group;
            GameObject selected_group = null;

            UButton ubutton;
            int a;

            for (a = 1; a < 4; a++)
            {
                group = container.transform.FindChild("buttons_group_" + a).gameObject;
                if (a == total)
                    selected_group = group;
                else
                    group.SetActive(false);
            }

            selected_group.SetActive(true);

            for (a = 1; a < (total + 1); a++)
            {
                ubutton = selected_group.transform.FindChild("button_0" + a).GetComponent<UButton>();

                if (a == 1) button_01 = ubutton;
                else if (a == 2) button_02 = ubutton;
                else if (a == 3) button_03 = ubutton;

                ubutton.addEventOnClick(onButtonPressed);
            }
        }


        private static void setupButtons(UDialogBoxButtons buttons)
        {
            if (buttons == UDialogBoxButtons.AbortRetryIgnore)
                _setButtons(UDialogResult.Abort, UDialogResult.Retry, UDialogResult.Ignore);
            else if (buttons == UDialogBoxButtons.OK)
                _setButtons(UDialogResult.OK);
            else if (buttons == UDialogBoxButtons.OKCancel)
                _setButtons(UDialogResult.OK, UDialogResult.Cancel);
            else if (buttons == UDialogBoxButtons.RetryCancel)
                _setButtons(UDialogResult.Retry, UDialogResult.Cancel);
            else if (buttons == UDialogBoxButtons.YesNo)
                _setButtons(UDialogResult.Yes, UDialogResult.No);
            else if (buttons == UDialogBoxButtons.YesNoCancel)
                _setButtons(UDialogResult.Yes, UDialogResult.No, UDialogResult.Cancel);
        }

        private static void _setButtons(UDialogResult button01Dialog)
        {
            showButtons(1);
            rename(button_01, button01Dialog);

        }

        private static void _setButtons(UDialogResult button01Dialog, UDialogResult button02Dialog)
        {
            showButtons(2);
            rename(button_01, button01Dialog);
            rename(button_02, button02Dialog);

        }

        private static void _setButtons(UDialogResult button01Dialog, UDialogResult button02Dialog, UDialogResult button03Dialog)
        {
            showButtons(3);
            rename(button_01, button01Dialog);
            rename(button_02, button02Dialog);
            rename(button_03, button03Dialog);

        }

        private static void rename(UButton ubutton, UDialogResult buttonType)
        {

            if (buttonType == UDialogResult.Abort)
            {
                ubutton.name = buttons_str_english[_abort];
                ubutton.text = buttons_str[_abort];
            }
            else if (buttonType == UDialogResult.Cancel)
            {
                ubutton.name = buttons_str_english[_cancel];
                ubutton.text = buttons_str[_cancel];
            }
            else if (buttonType == UDialogResult.Ignore)
            {
                ubutton.name = buttons_str_english[_ignore];
                ubutton.text = buttons_str[_ignore];
            }
            else if (buttonType == UDialogResult.No)
            {
                ubutton.name = buttons_str_english[_no];
                ubutton.text = buttons_str[_no];
            }
            else if (buttonType == UDialogResult.OK)
            {
                ubutton.name = buttons_str_english[_ok];
                ubutton.text = buttons_str[_ok];
            }
            else if (buttonType == UDialogResult.Retry)
            {
                ubutton.name = buttons_str_english[_retry];
                ubutton.text = buttons_str[_retry];
            }
            else if (buttonType == UDialogResult.Yes)
            {
                ubutton.name = buttons_str_english[_yes];
                ubutton.text = buttons_str[_yes];
            }
        }


        public static void show(String message, String label, bool isPassword, Func<UDialogResult, string, int> returnFunction = null)
        {
            show("", message, label, isPassword, returnFunction);
        }


        public static void show(String title, String message, String label, bool isPassword, Func<UDialogResult, string, int> returnFunction = null)
        {
            show(title, message, label, isPassword, UDialogBoxButtons.OK, returnFunction);
        }

        public static void show(String title, String message, String label, bool isPassword, UDialogBoxButtons buttons, Func<UDialogResult, string, int> returnFunction = null)
        {
            if (mcontainer != null)
            {
                HOTween.Kill(_tweenReferenceID);
                onHideComplete();
            }

            _create();
            _returnFunction = returnFunction;

            title_output.text = title;
            msg_output.text = message;
            labelInput.text = label;

            setupButtons(buttons);

            if (isPassword)
                inputField.inputField.contentType = InputField.ContentType.Password;

            XGuiFX.setAlpha(mcontainer, 1, 0.3f);
        }

        private static UDialogResult getDialogResult(GameObject button)
        {

            if (button.name == buttons_str_english[_abort])
                return UDialogResult.Abort;
            else if (button.name == buttons_str_english[_cancel])
                return UDialogResult.Cancel;
            else if (button.name == buttons_str_english[_ignore])
                return UDialogResult.Ignore;
            else if (button.name == buttons_str_english[_no])
                return UDialogResult.No;
            else if (button.name == buttons_str_english[_ok])
                return UDialogResult.OK;
            else if (button.name == buttons_str_english[_retry])
                return UDialogResult.Retry;
            else if (button.name == buttons_str_english[_yes])
                return UDialogResult.Yes;

            return UDialogResult.None;
        }

        private static int onButtonPressed(GameObject sender)
        {

            String value = inputField.text;

            _hide();

            if (_returnFunction != null)
                _returnFunction.DynamicInvoke(getDialogResult(sender), value);

            return 0;
        }

    }
}*/
