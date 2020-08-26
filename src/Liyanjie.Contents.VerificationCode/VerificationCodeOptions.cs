using System.Drawing;

namespace Liyanjie.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class VerificationCodeOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClickCodeImageDir { get; set; } = @"images\verificationCode";

        /// <summary>
        /// 
        /// </summary>
        public string PuzzleCodeImageDir { get; set; } = @"images\verificationCode";

        /// <summary>
        /// 
        /// </summary>
        public string SliderCodeImageDir { get; set; } = @"images\verificationCode";

        /// <summary>
        /// 
        /// </summary>
        public string[] FontFamilies { get; set; } = { "Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif" };

        /// <summary>
        /// 
        /// </summary>
        public FontStyle[] FontStyles { get; set; } = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular };

        /// <summary>
        /// 
        /// </summary>
        public string[] ArithmeticOperatorsText { get; set; } = { "加上", "减去", "乘以", "除以" };
    }
}
