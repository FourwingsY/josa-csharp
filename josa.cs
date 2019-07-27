using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace KoJosaReplace
{
    public class Josa
    {
        static Hashtable josaTable = new Hashtable();

        static void init()
        {
            josaTable.Add("은", makeJosaPicker("은", "는"));
            josaTable.Add("는", makeJosaPicker("은", "는"));

        }

        static int getJongseongCode(char c)
        {
            
            /*
            0 for no hasJongseong
            1 ~ 27 for jongseong Id
            */
            if (c <= '가' || '힇' <= c)
            {
                return 0;
            }
            int index = c - '가';
            return index % 28;
        }

        static Func<char, string> makeJosaPicker(string consonantJosa, string vowlJosa)
        {
            return prevChar => getJongseongCode(prevChar) > 0 ? consonantJosa : vowlJosa;
        }

        public static string replaceJosa(string input)
        {
            Regex josaTemplate = new Regex(@"#\{([^}]+)\}");
            MatchCollection matches = josaTemplate.Matches(input);
            StringBuilder builder = new StringBuilder();
            int stringIndex = 0;
            foreach (Match match in matches)
            {
                Group matched = match.Groups[0];
                string josaType = match.Groups[1].Value;
                
                builder.Append(input, stringIndex, matched.Index - stringIndex);

                // var prevChar = input[stringIndex + matched.Index - 1];
                // pick proper josa
                string josa = josaType;
                builder.Append(josa);
                stringIndex = matched.Index + matched.Length;
            }
            builder.Append(input, stringIndex, input.Length - stringIndex);
            return builder.ToString();
        }
    }
}
