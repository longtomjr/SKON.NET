﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SKON.Internal.Utils
{
    public static class ParserUtils
    {
        public static string ConvertToUnicode(string input)
        {
            int length = input.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            }

            return new string(Encoding.UTF8.GetChars(bytes));
        }
        
        public static string EscapeString(string txt)
        {
            if (string.IsNullOrEmpty(txt)) { return txt; }
            StringBuilder retval = new StringBuilder(txt.Length);
            for (int ix = 0; ix < txt.Length;)
            {
                int jx = txt.IndexOf('\\', ix);
                if (jx < 0 || jx == txt.Length - 1) jx = txt.Length;
                retval.Append(txt, ix, jx - ix);
                if (jx >= txt.Length) break;
                switch (txt[jx + 1])
                {
                    case 'b': retval.Append('\b'); break;
                    case 'n': retval.Append('\n'); break;
                    case 'f': retval.Append('\f'); break;
                    case 'r': retval.Append('\r'); break;
                    case 't': retval.Append('\t'); break;
                    case '"': retval.Append('"'); break;
                    case '\\': retval.Append('\\'); break;
                    case 'u': retval.Append(ConvertToUnicode(txt.Substring(jx, 4))); jx += 4; break;
                    default:
                        throw new FormatException("Invalid character escape!");
                }
                ix = jx + 2;
            }
            return retval.ToString();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
