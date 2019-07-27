using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KoreanJosaUtils
{
    using JosaPicker = Func<char, string>;
    public class Josa
    {
        static Dictionary<string, JosaPicker> pickerDict = new Dictionary<string, JosaPicker>();

        static Josa()
        {
            addPicker("은", "는");
            addPicker("이", "가");
            addPicker("을", "를");
            addPicker("과", "와");
            addPicker("이었", "였");
            addPicker("이어", "여");
            addPicker("이에요", "예요");
            addPicker("아", "야");
            addPicker("이?", "");
            pickerDict.Add("으로", makeSpecialJosaPicker("으로", "로"));
            pickerDict.Add("로", makeSpecialJosaPicker("으로", "로"));
        }

        static void addPicker(string consonantJosa, string vowelJosa)
        {
            pickerDict.Add(consonantJosa, makeJosaPicker(consonantJosa, vowelJosa));
            pickerDict.Add(vowelJosa, makeJosaPicker(consonantJosa, vowelJosa));
        }

        static JosaPicker makeJosaPicker(string consonantJosa, string vowelJosa)
        {
            return prevChar => getJongseongCode(prevChar) > 0 ? consonantJosa : vowelJosa;
        }

        // function for exceptional rule: ~로/~으로
        // ㄹ 받침이 들어가면 '~로'를 사용한다. ex) 밭으로 vs 길로
        static JosaPicker makeSpecialJosaPicker(string consonantJosa, string vowelJosa)
        {
            return prevChar => getJongseongCode(prevChar) > 0 && getJongseongCode(prevChar) != 8 ? consonantJosa : vowelJosa;
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
                
                // add non-josa strings
                builder.Append(input, stringIndex, matched.Index - stringIndex);

                // pick proper josa
                char prevChar = input[matched.Index - 1];
                JosaPicker josaPicker = Josa.pickerDict[josaType];
                string josa = josaPicker(prevChar);
                josa = josa.Replace("?", "");  // "#{이?}"

                // add josa
                builder.Append(josa);
                stringIndex = matched.Index + matched.Length;
            }
            // add remain strings
            builder.Append(input, stringIndex, input.Length - stringIndex);

            return builder.ToString();
        }
    }
}
