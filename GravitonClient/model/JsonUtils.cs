﻿//This file contains the JsonUtils class which helps with serialization and deserialization
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{

    //This class helps with serialization and deserialization
    class JsonUtils
    {
        //This method returns the length (in characters) of a json object
        private static int LengthOfJsonObject(string json, int startchar)
        {
            switch (json[startchar])
            {
                case '[':
                    return JsonUtils.LoopTillNextChar(json, startchar, '[', ']');
                case '{':
                    return JsonUtils.LoopTillNextChar(json, startchar, '{', '}');
                case '\"':
                    int length = 1;
                    foreach (char c in json.Substring(startchar + 1))
                    {
                        length++;
                        if (c == '\\')
                            continue;
                        if (c == '"')
                            break;
                    }
                    return length;
                default:
                    int length2 = 0;
                    foreach (char c in json.Substring(startchar + 1))
                    {

                        length2++;
                        if (c == ']' || c == '}' || c == ',' || Char.IsWhiteSpace(c))
                            break;
                    }
                    return  length2;
            }
        }

        //This method extracts a value from a json string
        public static string ExtractValue(string json, string key)
        {
            int i = json.IndexOf(String.Format("\"{0}\":", key));
            if (i == -1)
                throw new Exception("Key was not in string given.");
            i += key.Length + 3; //putting it at the first char of value
            return GetObject(json, i);
        }

        //This method returns the correct string for the next object being processed
        private static string GetObject(string json, int start)
        {
            return json.Substring(start, LengthOfJsonObject(json, start));
        }

        //This method loops through a string until it finds the next specified character
        private static int LoopTillNextChar(string json, int start, char front, char back)
        {
            int numOfUnclosed = 0;
            int length = 0;
            bool inString = false;
            bool escapeNextChar = false;

            foreach(char c in json.Substring(start))
            {
                length++;
                if (!escapeNextChar) {
                    if (!inString) {
                        
                        if(c == front)
                            numOfUnclosed++;
                        if (c == back)
                            numOfUnclosed--;
                        if (c == '"')
                            inString = true;
                    }
                    else if (c == '"')
                        inString = false;
                    else if (c == '\\')
                        escapeNextChar = true;
                }
                else
                    escapeNextChar = false;
                if (numOfUnclosed == 0)
                    break;
            }
            return length;
        }

       
        //This method returns a list of strings that represent objects
        public static List<string> GetObjectsInArray(string json)
        {
            var list = new List<string>();
            if (json[0] != '[')
                throw new FormatException("string is  not a json array");
            for(int i = 1; i < json.Length; i++)
            {
                char currentChar = json[i];
                if (Char.IsWhiteSpace(currentChar) || currentChar == ',')
                    continue;
                if (currentChar == ']')
                    break;
                string result = GetObject(json, i);
                i += result.Length - 1;
                list.Add(result);
            }
            return list;
        }

        //This method converts the input list of objects into a json string
        internal static object ToJsonList(List<Powerup.powerups> currentPowerups)
        {
            string result = "[\r\n";
            foreach (Powerup.powerups go in currentPowerups)
            {
                result += "    " + go.ToString() + ",\r\n";
            }
            result += "]";

            return result;
        }

        //This method converts the input list of objects into a json string
        public static string ToJsonList(IEnumerable<GameObject> gameObjects)
        {
            string result = "[\r\n";
            foreach(GameObject go in gameObjects) 
                result += "    " + go.Serialize() + ",\r\n";
            result += "]";

            return result;
        }

        //This method converts the input list of objects into a json string
        public static string ToJsonList(IEnumerable<int> ints)
        {
            string result = "[\r\n";
            foreach (int go in ints)
                result += "    " + go + ",\r\n";
            result += "]";

            return result;
        }

        //This method converts the input list of objects into a json string
        internal static string GameObjectsToJsonList(IEnumerable<GameObject> gameObjects)
        {
            string result = "[\r\n";
            foreach (GameObject go in gameObjects)
                result += "    " + go.Serialize() + ",\r\n";
            result += "]";

            return result;
        }
    }
}
