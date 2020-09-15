using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GEX.debug;

namespace GEX.io
{
    
	public class Filex
	{

        //-----------------------------------------------
        //----                                       ----
        //----      _getAbsolutePathForResources     ----
        //----                                       ----
        //-----------------------------------------------
        private static string _getAbsolutePathForResources()
        {
            //example: C:/unity_projects/Sonic3DBlast/Assets
            string resourcesPath = Application.persistentDataPath + "/";

            return resourcesPath;
        }

        //-----------------------------------------------
        //----                                       ----
        //----         getAbsolutePath               ----
        //----                                       ----
        //-----------------------------------------------
        public static string getAbsolutePath()
        {
            return _getAbsolutePathForResources();
        }

        //Example of other app path data/com.untrefmedia.marblelegends_demo/files/
        //Example of current app path data/com.untrefmedia.marblelegends_full/files/
        //otherAppName = 'marblelegends_demo'
        public static string getAbsolutePathForOtherApp(String thisAppName, String otherAppName)
        { 
            string path = _getAbsolutePathForResources();

            if (path.Contains(thisAppName))
                return path.Replace(thisAppName, otherAppName);
            else
                return "";
        }

        //-----------------------------------------------
        //----                                       ----
        //----         createDirIfNotExist           ----
        //----                                       ----
        //-----------------------------------------------
        public static void createDirIfNotExist(string dir)
        { 
            string path = _getAbsolutePathForResources();

            if (!Directory.Exists(path + "/" + dir))
                Directory.CreateDirectory(path + "/" + dir);

        }

        //-----------------------------------------------
        //----                                       ----
        //----             fileExists                ----
        //----                                       ----
        //-----------------------------------------------
        public static bool fileExists(string fileName)
        {
            String path = _getAbsolutePathForResources() + fileName;

            return File.Exists(path);
        }

        //**********************************************************************
        //**************                                        ****************
        //**************             SAVE OBJECT                ****************
        //**************                                        ****************
        //**********************************************************************
        public static void saveObject(string fileName, FileContainer objToWrite)
        {
            String path = _getAbsolutePathForResources() + fileName;

            XConsole.println("SAVE [try]: " + path);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, objToWrite);
            stream.Close();

            XConsole.println("SAVED [OK]: " + path);
        }


        //**********************************************************************
        //**************                                        ****************
        //**************             LOAD OBJECT                ****************
        //**************                                        ****************
        //**********************************************************************
        public static FileContainer loadObject(string path)
        {
            //String path = _getAbsolutePathForResources() + fileName;

            XConsole.println("LOAD [try]: " + path);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileContainer obj = (FileContainer)formatter.Deserialize(stream);
            stream.Close();

            XConsole.println("LOADED [OK]: " + path);

            return obj;
        }



        //**********************************************************************
        //**************                                        ****************
        //**************               JSON                     ****************
        //**************                                        ****************
        //**********************************************************************
        public static String convertToJSON(object objToWrite)
        {
            /*DataContractJsonSerializer js = new DataContractJsonSerializer(objToWrite.GetType());
            MemoryStream stream = new MemoryStream();
            js.WriteObject(stream, objToWrite);
            stream.Position = 0;
            String DATA = new StreamReader(stream).ReadToEnd();

            return DATA;*/
            return "";
        }

        public static void saveJSON(String path, object objToWrite)
        {
            /*String json = convertToJSON(objToWrite);

            TextWriter tw = new StreamWriter(path, false);
            tw.Write(json);
            tw.Close();*/
        }

        public static object loadJSON(String path, object objToWrite)
        {
            /*TextReader reader = new StreamReader(path);
            object retObj = loadJSONFromMem(reader.ReadToEnd(), objToWrite);
            reader.Close();

            return retObj;*/
            return null;
        }

        /*public static object loadJSONFromMem(String data, object objToWrite)
        {
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(data));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(objToWrite.GetType());

            stream.Position = 0;
            return ser.ReadObject(stream);
        }*/


        /*public static void saveAtPDataPath(string fileName)
        {
            String path = _getAbsolutePathForResources() + fileName;

            XConsole.println("SAVE [try]: " + path);

            save(path);

            XConsole.println("SAVED [OK]");

        }



        public static void loadAtPDataPath(string fileName)
        {
            string path = _getAbsolutePathForResources() + fileName;

            XConsole.println("LOAD [try]: " + path);

            load(path);

            XConsole.println("LOADED [OK]");
        }


        public static void save(string path)
        {
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("test");
            writer.Close();
        }

        public static void load(string path)
        {
            StreamReader reader = new StreamReader(path);

            XConsole.println(reader.ReadLine());

            reader.Close();

        }*/





	}
}
