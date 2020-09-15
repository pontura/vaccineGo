using UnityEngine;
using System;


namespace com.data
{
	class Res
	{

        //**********************************************
        //***                                        ***
        //***                 SPGEN                  ***
        //***                                        ***
        //**********************************************
        /*public const int SPGEN_GUI_ATLAS =  0;
        public const int SPGEN_GUI_LEVELS = 1;*/


        //**********************************************
        //***                                        ***
        //***               TEXTURES                 ***
        //***                                        ***
        //**********************************************
        /*public const int TEX_GUI_ATLAS =            0;
        public const int TEX_GUI_LEVELS_ATLAS =     1;
        public const int TEXT_CONGRATULATIONS_SP =  2;
        public const int TEXT_CONGRATULATIONS_EN =  3;
        public const int TEX_SPLASH_SCREEN =        4;
        public const int TEX_BACKGROUND_COMMON =    5;
        public const int TEX_BACKGROUND_GALAXY =    6;
        public const int TEX_PACK01 =               7;
        public const int TEX_PACK02 =               8;
        public const int TEX_WATERMARK =            9;
        public const int TEX_TEST =                 10;
        public const int TEX_R_PANEL =              11;
        public const int TEX_MARBLE_CONTAINER_GLASS=12;
        public const int TEX_MARBLE_CONTAINER =     13;
        public const int TEX_MARBLE_OUTLINE =       14;
        public const int TEX_LEFT_ARROW_YELLOW =    15;
        public const int TEX_RIGHT_ARROW_YELLOW =   16;*/

        //**********************************************
        //***                                        ***
        //***            GUI ELEMENTS                ***
        //***                                        ***
        //**********************************************
        public const int GUI_SPLASH =   0;
        public const int LOGIN =        1;
        public const int IN_GAME_GUI =  2;
        public const int BLOCKER =      3;
        public const int GENERIC_GRID = 4;
        public const int GUI_DOT =      5;
        public const int LISTVIEW =     6;
        public const int MGUI_COMMON = 7;
        public const int BLOCKER_TRANSPARENT = 8;
        public const int OPPONENT_THINKING = 9;
        

        //**********************************************
        //***                                        ***
        //***                SPRITES                 ***
        //***                                        ***
        //**********************************************
        public const int SPRITE_SAMPLE_1 =                      0;
        public const int SPRITE_SAMPLE_2 =                      1;
        public const int SPRITE_DICE_ACTION_ADD_ORB =           2;
        public const int SPRITE_DICE_ACTION_ADD_AVATAR_ATTACK = 3;
        public const int SPRITE_DICE_ACTION_ADD_AVATAR_LIFE =   4;
        public const int SPRITE_DICE_ACTION_ADD_CARD_ATTACK =   5;
        public const int SPRITE_DICE_ACTION_ADD_CARD_LIFE =     6;
        

        
        //**********************************************
        //***                                        ***
        //***                 FONTS                  ***
        //***                                        ***
        //**********************************************
        /*public const int FONT_TRAJAN_MED =      0;
        public const int FONT_TRAJAN_BIG =      1;
        public const int FONT_DISCOGNATE =      8;*/


        //##############################################
        //###                                        ###
        //###                 SPGEN                  ###
        //###                                        ###
        //##############################################
        public static string getSPGenFile(int index)
        {
            TextAsset textAsset = null;


            /*switch (index)
            {

                case SPGEN_GUI_ATLAS:
                    textAsset = (TextAsset)Resources.Load("2D/gui/sgf/gui_map");
                    break;

                case SPGEN_GUI_LEVELS:
                    textAsset = (TextAsset)Resources.Load("2D/gui/sgf/levels_map");
                    break;

            }*/

            return textAsset.ToString();
        }


        //##############################################
        //###                                        ###
        //###                TEXTURES                ###
        //###                                        ###
        //##############################################
        public static Texture2D getTexture2D(int index)
        {
            Texture2D tex = null;

            /*switch (index)
            { 
                case TEX_GUI_ATLAS:
                    tex = Resources.Load("2D/gui/img/gui_tex") as Texture2D;
                    break;

                case TEX_GUI_LEVELS_ATLAS:
                    tex = Resources.Load("2D/gui/img/levels_map") as Texture2D;
                    break;

                case TEXT_CONGRATULATIONS_EN:
                    tex = Resources.Load("2D/others/text/congratulations_english") as Texture2D;
                    break;

                case TEXT_CONGRATULATIONS_SP:
                    tex = Resources.Load("2D/others/text/congratulations_spanish") as Texture2D;
                    break;

                case TEX_SPLASH_SCREEN:
                    tex = Resources.Load("2D/others/splash/splash") as Texture2D;
                    break;

                case TEX_BACKGROUND_COMMON:
                    tex = Resources.Load("2D/others/backgrounds/background_common") as Texture2D;
                    break;

                case TEX_BACKGROUND_GALAXY:
                    tex = Resources.Load("2D/others/backgrounds/background_galaxy") as Texture2D;
                    break;

                case TEX_PACK01:
                    tex = Resources.Load("2D/others/packages/pack01") as Texture2D;
                    break;

                case TEX_PACK02:
                    tex = Resources.Load("2D/others/packages/pack02") as Texture2D;
                    break;

                case TEX_WATERMARK:
                    tex = Resources.Load("2D/others/logos/watermarkUntref") as Texture2D;
                    break;

                case TEX_TEST:
                    tex = Resources.Load("2D/gui/img/test") as Texture2D;
                    break;

                case TEX_R_PANEL:
                    tex = Resources.Load("2D/others/panels/back_r_panel") as Texture2D;
                    break;

                case TEX_MARBLE_CONTAINER_GLASS:
                    tex = Resources.Load("2D/others/marbles/marble_container_glass") as Texture2D;
                    break;

                case TEX_MARBLE_CONTAINER:
                    tex = Resources.Load("2D/others/marbles/marble_container") as Texture2D;
                    break;

                case TEX_MARBLE_OUTLINE:
                    tex = Resources.Load("2D/others/marbles/marble_outline") as Texture2D;
                    break;

                case TEX_LEFT_ARROW_YELLOW:
                    tex = Resources.Load("2D/others/buttons/left_arrow_yellow") as Texture2D;
                    break;

                case TEX_RIGHT_ARROW_YELLOW:
                    tex = Resources.Load("2D/others/buttons/right_arrow_yellow") as Texture2D;
                    break;
            }*/

            return tex;
        }


        //##############################################
        //###                                        ###
        //###              GUI ELEMENTS              ###
        //###                                        ###
        //##############################################
        public static GameObject getGUIElement(int index)
        {
            GameObject gobj = null;

            switch (index)
            {
                case GUI_SPLASH:
                    gobj = Resources.Load("GAME_GUI/SPLASH") as GameObject;
                    break;

                case LOGIN:
                    gobj = Resources.Load("GAME_GUI/LOGIN") as GameObject;
                    break;

                case IN_GAME_GUI:
                    gobj = Resources.Load("GAME_GUI/IN_GAME_GUI") as GameObject;
                    break;

                case BLOCKER:
                    gobj = Resources.Load("GAME_GUI/BLOCKER") as GameObject;
                    break;

                case GENERIC_GRID:
                    gobj = Resources.Load("GAME_GUI/GENERIC_GRID") as GameObject;
                    break;


                case GUI_DOT:
                    gobj = Resources.Load("GAME_GUI/DOT") as GameObject;
                    break;

                case LISTVIEW:
                    gobj = Resources.Load("GAME_GUI/LISTVIEW") as GameObject;
                    break;

                case MGUI_COMMON:
                    gobj = Resources.Load("GAME_GUI/MGUI_COMMON") as GameObject;
                    break;

                case BLOCKER_TRANSPARENT:
                    gobj = Resources.Load("GAME_GUI/BLOCKER_TRANSPARENT") as GameObject;
                    break;

                case OPPONENT_THINKING:
                    gobj = Resources.Load("GAME_GUI/OPPONENT_THINKING") as GameObject;
                    break;
            }


            return gobj;
        }

       

        //##############################################
        //###                                        ###
        //###                SPRITE                  ###
        //###                                        ###
        //##############################################
        public static Sprite getSprite(int index)
        {
            Sprite sp = null;

            switch (index)
            {
                case SPRITE_SAMPLE_1:
                    sp = Resources.Load<Sprite>("2D/sprites/sp_sample_1");
                    break;

                case SPRITE_SAMPLE_2:
                    sp = Resources.Load<Sprite>("2D/sprites/sp_sample_2");
                    break;

                case SPRITE_DICE_ACTION_ADD_ORB:
                    sp = Resources.Load<Sprite>("cards/dices/actions/dice_action_add_orb");
                    break;

                case SPRITE_DICE_ACTION_ADD_AVATAR_ATTACK:
                    sp = Resources.Load<Sprite>("cards/dices/actions/dice_action_add_avatar_attack");
                    break;

                case SPRITE_DICE_ACTION_ADD_AVATAR_LIFE:
                    sp = Resources.Load<Sprite>("cards/dices/actions/dice_action_add_avatar_life");
                    break;

                case SPRITE_DICE_ACTION_ADD_CARD_ATTACK:
                    sp = Resources.Load<Sprite>("cards/dices/actions/dice_action_add_card_attack");
                    break;

                case SPRITE_DICE_ACTION_ADD_CARD_LIFE:
                    sp = Resources.Load<Sprite>("cards/dices/actions/dice_action_add_card_life");
                    break;
            }

            return sp;
        }


        //##############################################
        //###                                        ###
        //###                  FONTS                 ###
        //###                                        ###
        //##############################################
        public static byte[] getFont(int index)
        {
            byte[] font;
            TextAsset textAsset = null;

            /*switch (index)
            { 

                case FONT_TRAJAN_MED:
                    textAsset = Resources.Load("fonts/trajanpro") as TextAsset;
                    break;

                case FONT_TRAJAN_BIG:
                    textAsset = Resources.Load("fonts/TrajanBig") as TextAsset;
                    break;

                case FONT_DISCOGNATE:
                    textAsset = Resources.Load("fonts/discognate") as TextAsset;
                    break;
            
            }*/

            font = textAsset.bytes;

            return font;
        }

	}
}
