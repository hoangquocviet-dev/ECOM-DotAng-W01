using System.Text;
using System.Text.RegularExpressions;

namespace phase_1.Helpers
{
    public static class StringHelper
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return string.Empty;

            string str = phrase.ToLower();
            
            str = Regex.Replace(str, @"á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ", "a");
            str = Regex.Replace(str, @"é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ", "e");
            str = Regex.Replace(str, @"i|í|ì|ỉ|ĩ|ị", "i");
            str = Regex.Replace(str, @"ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ", "o");
            str = Regex.Replace(str, @"ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự", "u");
            str = Regex.Replace(str, @"ý|ỳ|ỷ|ỹ|ỵ", "y");
            str = Regex.Replace(str, @"đ", "d");

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }
    }
}
