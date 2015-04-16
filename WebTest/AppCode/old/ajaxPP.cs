//
//*   http://weblog.cynosura.eu/post/2010/03/06/Json-Pretty-Printer.aspx
//*   Copyright (c) 2010, Raymond Glover
//*   All rights reserved.
//*
//*   Redistribution and use in source and binary forms, with or without modification, 
//*   are permitted provided that the following conditions are met:
//*
//*     1. Redistributions of source code must retain the above copyright notice, 
//*        this list of conditions and the following disclaimer.
//*     2. Redistributions in binary form must reproduce the above copyright 
//*        notice, this list of conditions and the following disclaimer in the 
//*        documentation and/or other materials provided with the distribution.
//*     3. Neither the name of Cynosura nor the names of its contributors may 
//*        be used to endorse or promote products derived from this software 
//*        without specific prior written permission.
//*
//*   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
//*   EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
//*   OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//*   SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//*   INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//*   TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
//*   OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//*   CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
//*   ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
//*   DAMAGE.
//*
//

using System.Collections.Generic;


namespace COR.Tools.JSON
{


    /// <summary>
    ///   A helper class for generating object graphs in 
    ///   JavaScript notation. Supports pretty printing.
    /// </summary>
    public class JsonHelper
    {
        public static string Serialize(object target)
        {
#if DEBUG
            return Serialize(target, true);
#else
			return Serialize(target, false);
#endif
        }

        public static T Deserialize<T>(string strValue)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strValue);
        }

        public static List<Dictionary<string, string>> Deserialize(string strValue)
        {
            Newtonsoft.Json.Linq.JArray tV = (Newtonsoft.Json.Linq.JArray)Deserialize<object>(strValue);
            List<Dictionary<string, string>> tR = new List<Dictionary<string, string>>();

            for (int tC = 0; tC < tV.Count; tC++)
            {
                tR.Add(Deserialize<Dictionary<string, string>>(tV[tC].ToString()));
            }

            return tR;
        }


        //Cynosura.Base.JsonHelper.SerializeUnpretty(target)
        public static string SerializeUnpretty(object target)
        {
            return SerializeUnpretty(target, null);
        }

        public static string SerializeUnpretty(object target, string strCallback)
        {
            return Serialize(target, false, null);
        }


        //Cynosura.Base.JsonHelper.SerializePretty(target)
        public static string SerializePretty(object target)
        {
            return SerializePretty(target, null);
        }


        public static string SerializePretty(object target, string strCallback)
        {
            return Serialize(target, true, strCallback);
        }

        public static string Serialize(object target, bool prettyPrint)
        {
            return Serialize(target, prettyPrint, null);
        }

        public static string Serialize(object target, bool prettyPrint, string strCallback)
        {
            string strResult = null;

            // http://james.newtonking.com/archive/2009/10/23/efficient-json-with-json-net-reducing-serialized-json-size.aspx
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };
            if (!prettyPrint)
            {
                settings.Formatting = Newtonsoft.Json.Formatting.None;
            }
            else
            {
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            }


            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            //context.Response.Write(strCallback + " && " + strCallback + "(")

            if (string.IsNullOrEmpty(strCallback))
            {
                strResult = Newtonsoft.Json.JsonConvert.SerializeObject(target, settings);
                // JSONP
            }
            else
            {
                // https://github.com/visionmedia/express/pull/1374
                //strResult = strCallback + " && " + strCallback + "(" + Newtonsoft.Json.JsonConvert.SerializeObject(target, settings) + "); " + Environment.NewLine
                //typeof bla1 != "undefined" ? alert(bla1(3)) : alert("foo undefined");
                strResult = "typeof " + strCallback + " != 'undefined' ? " + strCallback + "(" + Newtonsoft.Json.JsonConvert.SerializeObject(target, settings) + ") : alert('Callback-Funktion \"" + strCallback + "\" undefiniert...'); " + System.Environment.NewLine;
            }

            settings = null;
            return strResult;



            //Dim sbSerialized As New StringBuilder()
            //Dim js As New JavaScriptSerializer()

            //js.Serialize(target, sbSerialized)

            //If prettyPrint Then
            //    Dim prettyPrintedResult As New StringBuilder()
            //    prettyPrintedResult.EnsureCapacity(sbSerialized.Length)

            //    Dim pp As New JsonPrettyPrinter()
            //    pp.PrettyPrint(sbSerialized, prettyPrintedResult)

            //    Return prettyPrintedResult.ToString()
            //Else
            //    Return sbSerialized.ToString()
            //End If
        }

        public static Dictionary<string, object> NvcToDictionary(System.Collections.Specialized.NameValueCollection nvc, bool handleMultipleValuesPerKey = true)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if ((values.Length == 1))
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }
            return result;
        }

        public static Dictionary<string, object> sessionToDictionary(System.Web.SessionState.HttpSessionState nvc)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                result.Add(key, nvc[key]);
            }
            return result;
        }
    }



    public class JsonPrettyPrinter
    {
        #region "class members"
        const string Space = " ";
        const int DefaultIndent = 0;
        const string Indent = Space + Space + Space + Space;

        static readonly string NewLine = System.Environment.NewLine;
        private static void BuildIndents(int indents, System.Text.StringBuilder output)
        {
            indents += DefaultIndent;
            while (indents > 0)
            {
                output.Append(Indent);
                indents -= 1;
            }
        }
        #endregion

        private bool inDoubleString = false;
        private bool inSingleString = false;
        private bool inVariableAssignment = false;

        //private char prevChar = ControlChars.NullChar;
        private char prevChar = '\0';

        private enum JsonContextType
        {
            Object,
            Array
        }


        private Stack<JsonContextType> context = new Stack<JsonContextType>();
        private bool InString()
        {
            return inDoubleString || inSingleString;
        }

        public void PrettyPrint(System.Text.StringBuilder input, System.Text.StringBuilder output)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }

            int inputLength = input.Length;
            char c = '\0';

            for (int i = 0; i <= inputLength - 1; i++)
            {
                //c = FileSystem.input(i);
                c = (char)i;

                switch (c)
                {
                    case '{':
                        if (!InString())
                        {
                            if (inVariableAssignment || (context.Count > 0 && context.Peek() != JsonContextType.Array))
                            {
                                output.Append(NewLine);
                                BuildIndents(context.Count, output);
                            }
                            output.Append(c);
                            context.Push(JsonContextType.Object);
                            output.Append(NewLine);
                            BuildIndents(context.Count, output);
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break;
                    case '}':
                        if (!InString())
                        {
                            output.Append(NewLine);
                            context.Pop();
                            BuildIndents(context.Count, output);
                            output.Append(c);
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break;
                    case '[':
                        output.Append(c);

                        if (!InString())
                        {
                            context.Push(JsonContextType.Array);
                        }

                        break;
                    case ']':
                        if (!InString())
                        {
                            output.Append(c);
                            context.Pop();
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break;
                    case '=':
                        output.Append(c);
                        break;
                    case ',':
                        output.Append(c);

                        if (!InString() && context.Peek() != JsonContextType.Array)
                        {
                            BuildIndents(context.Count, output);
                            output.Append(NewLine);
                            BuildIndents(context.Count, output);
                            inVariableAssignment = false;
                        }

                        break;
                    case '\'':
                        if (!inDoubleString && prevChar != '\\')
                        {
                            inSingleString = !inSingleString;
                        }

                        output.Append(c);
                        break;
                    case ':':
                        if (!InString())
                        {
                            inVariableAssignment = true;
                            output.Append(Space);
                            output.Append(c);
                            output.Append(Space);
                        }
                        else
                        {
                            output.Append(c);
                        }

                        break;
                    case '"':
                        if (!inSingleString && prevChar != '\\')
                        {
                            inDoubleString = !inDoubleString;
                        }

                        output.Append(c);
                        break;
                    default:

                        output.Append(c);
                        break;
                }
                prevChar = c;
            }
        }


    }


    namespace Classes
    {

        public class DropDown
        {
            public string Value;
            public string Text;
        }

        public class UM_Employee
        {
            public string ID;
            public string ZID;
            public string AP;
            public string Name;
            public string Nr;
            public string BG;
            public string F;
            public string T;
            public string ZT;
            public string ZF;
        }

    }

}
