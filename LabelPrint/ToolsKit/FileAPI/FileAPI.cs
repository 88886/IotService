using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SKT.Dev.Utils.ToolsKit.FileAPI
{
    public class FileAPI
    {

        public static List<List<String>> ReadFileToMatrix(String absolutePath, System.Text.Encoding encode,char sig)
        {
            StreamReader reader = new StreamReader(absolutePath);
            String content = reader.ReadToEnd();

            
            String[] firstArr = content.Replace("\r\n", "@").Split('@');
            List<List<String>> listArray = new List<List<String>>();

            for (int i = 0; i < firstArr.Length; i++)
            {
                String[] columnsArr = firstArr[i].Split(sig);
                List<String> listSecond = new List<string>();
                foreach (var key in columnsArr)
                {
                    if(!String.IsNullOrEmpty(key)){
                        listSecond.Add(key);
                    }
                 
                }

                if (listSecond.Count == 5)
                {
                    listSecond.Add(listSecond[listSecond.Count - 1]);
                    listSecond[listSecond.Count - 2] = " ";
                }
                listArray.Add(listSecond);
            }

            return listArray;
        }
    }
}
