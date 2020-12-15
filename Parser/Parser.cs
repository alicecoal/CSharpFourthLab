using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Parser
{
    public class Parser
    {
        public T DeserializeJson<T>(string json) where T : new()
        {
            List<string> list = JsonParse(json);
            return Deserialize<T>(list);
        }

        public T DeserializeXML<T>(string XML) where T : new()
        {
            List<string> list = XMLParse(XML, true);
            return Deserialize<T>(list);
        }


        public T Deserialize<T>(List<string> list) where T : new()
        {            
            Type type = typeof(T);
            T ans = (T)Activator.CreateInstance(type);

            string key = "", value = "";

            Regex complex = new Regex(@"(\w+)\s*:\s*{(.*)}", RegexOptions.Singleline);
            Regex simple = new Regex(@"(\w+)\s*:\s*(.*)", RegexOptions.Singleline);
            Match match;

            foreach (string option in list)
            {
                if (complex.IsMatch(option))
                {
                    match = complex.Match(option);

                    key = match.Groups[1].Value;
                    value = match.Groups[2].Value;

                    PropertyInfo info = type.GetProperty(key);
                    object obj = typeof(Parser).GetMethod("DeserializeJson")
                        .MakeGenericMethod(new Type[] { info.PropertyType }).Invoke(this, new object[] { value });                    
                    info.SetValue(ans, obj);
                }
                else if (simple.IsMatch(option))
                {
                    match = simple.Match(option);

                    key = match.Groups[1].Value;
                    value = match.Groups[2].Value;


                    PropertyInfo info = type.GetProperty(key);

                    if (info.PropertyType.IsEnum)
                    {
                        info.SetValue(ans, Enum.Parse(info.PropertyType, value));
                    }
                    else
                    {
                        info.SetValue(ans, Convert.ChangeType(value, info.PropertyType));
                    }
                }
            }
            return ans;
        }

        public List<string> JsonParse(string json)
        {
            json = json.Trim(new char[] { ' ', '{', '}' });

            List<string> list = new List<string>();
            int brackets = 0;
            StringBuilder option = new StringBuilder();

            foreach (char symbol in json)
            {
                if (char.IsLetterOrDigit(symbol) || char.IsPunctuation(symbol))
                {
                    if (symbol == ',' && brackets == 0)
                    {
                        list.Add(option.ToString());
                        option.Clear();
                    }
                    else if (symbol == '{')
                    {
                        option.Append(symbol);
                        brackets++;
                    }
                    else if (symbol == '}')
                    {
                        option.Append(symbol);
                        brackets--;
                    }
                    else
                    {
                        option.Append(symbol);
                    }
                }
            }
            if (option.Length != 0)
                list.Add(option.ToString());
            return list;
        }


        public List<string> XMLParse(string xml, bool trim)
        {
            xml = xml.Trim(new char[] { '\n', '\t', '\r', ' ' });
            List<string> objects = new List<string>();
            string tagName;
            Match match;

            try
            {
                tagName = GetNextTag(xml, 0);
                if (trim)
                {
                    Regex trimming = new Regex($"^<{tagName}>(.*)</{tagName}>$", RegexOptions.Singleline);
                    match = trimming.Match(xml);
                    if (match.Success)
                    {
                        xml = match.Groups[1].Value;
                    }
                }
            }
            catch
            {
                return new List<string>() { xml };
            }

            Regex Tag = new Regex(@"<(/?.*)>");

            string mainTag = "", tag = "";
            int deep = 0;
            bool isMainTag = true, isValue = false;
            string value = "";
            bool quotes = false;

            foreach (char c in xml)
            {
                if ((c != '\t' && c != '\r' && c != '\n') || quotes)
                {
                    if (c == '\"')
                    {
                        quotes = !quotes;
                    }
                    if (quotes)
                    {
                        value += c;
                        continue;
                    }
                    if (c == '<')
                    {
                        isValue = false;
                        if (!isMainTag)
                        {
                            tag += c;
                        }
                    }
                    else if (c == '>')
                    {
                        if (isMainTag)
                        {
                            isMainTag = false;
                            isValue = true;
                            deep++;
                        }
                        else
                        {
                            tag += c;
                            match = Tag.Match(tag);

                            if (match.Success)
                            {
                                tagName = match.Groups[1].Value;
                                if (tagName[0] == '/')
                                {
                                    if ('/' + mainTag == tagName && deep == 1)
                                    {
                                        mainTag = "";
                                        tag = "";
                                        isMainTag = true;
                                        isValue = false;
                                        value = "";
                                    }
                                    else
                                    {
                                        value += tag;
                                        tag = "";
                                        isValue = true;
                                    }
                                    deep--;
                                }
                                else
                                {
                                    deep++;
                                    isValue = true;
                                    value += tag;
                                    tag = "";
                                }
                            }
                            else
                            {
                                throw new Exception("Damaged XML");
                            }
                        }
                    }
                    else
                    {
                        if (isValue)
                        {
                            value += c;
                        }
                        else if (isMainTag)
                        {
                            mainTag += c;
                        }
                        else
                        {
                            tag += c;
                        }
                    }
                }
            }

            if (mainTag != "")
            {
                return new List<string>() { mainTag };
            }

            return objects;
        }

        string GetNextTag(string xml, int startIndex)
        {
            StringBuilder tag = new StringBuilder("");
            bool isTag = false;
            for (int i = startIndex; i < xml.Length; i++)
            {
                if (xml[i] == '<')
                {
                    isTag = true;
                    continue;
                }
                else if (xml[i] == '>')
                {
                    return tag.ToString();
                }
                else if (isTag)
                {
                    tag.Append(xml[i]);
                }
            }
            throw new Exception("Matching tag wasn't found.");
        }
    }
}