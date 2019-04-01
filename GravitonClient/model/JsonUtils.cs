using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitonClient
{
    class JsonUtils
    {
        public static string ExtractValue(string json, string key)
        {
            int i = json.IndexOf(String.Format("\"{0}\":", key));
            if (i == -1)
                throw new Exception("Key was not in string given.");
            i += key.Length + 3; //putting it at the first char of value
            switch (json[i]) 
            {
                case '[':
                    return JsonUtils.LoopTillNextChar(json, i, '[', ']');
                case '{':
                    return JsonUtils.LoopTillNextChar(json, i, '{', '}');
                case '\"':
                    int length = 1;
                    foreach(char c in json.Substring(i + 1)){
                        length++;
                        if (c == '\\')
                            continue;
                        if (c == '"')
                            break;
                    }
                    return json.Substring(i, length);
                default:
                    int length2 = 1;
                    foreach (char c in json.Substring(i + 1)){
                        
                        length2++;
                        if (c == ']' || c == '}' || c == ','|| Char.IsWhiteSpace(c))
                            break;
                    }
                    return json.Substring(i, length2);
            }
        }

        private static string LoopTillNextChar(string json, int start, char front, char back)
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
            return json.Substring(start, length);
        }

    }
}
