using System;
using KoreanJosaUtils;

class Test
{
    public static void Main()
    {
        // 나는 너를 사랑해.
        Console.WriteLine(Josa.replaceJosa("나#{은} 너#{을} 사랑해."));

        // 너였어? 몰랐네.
        Console.WriteLine(Josa.replaceJosa("너#{이었}어? 몰랐네."));

        // 재훈이랑 구라랑 카페로 들어갔어.
        Console.WriteLine(Josa.replaceJosa("재훈#{이?}랑 구라#{이?}랑 카페#{으로} 들어갔어."));

        // 발로 찬 공이 골대를 흔들었어요.
        Console.WriteLine(Josa.replaceJosa("발#{으로} 찬 공#{가} 골대#{을} 흔들었어요."));
    }
}
