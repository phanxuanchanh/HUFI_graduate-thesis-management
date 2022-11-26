using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Common.ShortUID
{
    /// <summary>
    /// Provides programmatic configuration for the shortid library.
    /// </summary>
    public class GenerationOptions
    {
        /// <summary>
        /// Determines whether numbers are used in generating the id.
        /// Default: false.
        /// </summary>
        public bool UseNumbers { get; }

        /// <summary>
        /// Determines whether special characters are used in generating the id.
        /// Default: true.
        /// </summary>
        public bool UseSpecialCharacters { get; }

        /// <summary>
        /// Determines the length of the generated id.
        /// Default: a random generated id length between 8 and 14 characters.
        /// </summary>
        public int Length { get; }

        public GenerationOptions(bool useNumbers = false, bool useSpecialCharacters = true, int? length = null)
        {
            UseNumbers = useNumbers;
            UseSpecialCharacters = useSpecialCharacters;
            Length = length ??
                     RandomUtils.GenerateNumberInRange(Constants.MinimumAutoLength, Constants.MaximumAutoLength);
        }
    }
}
