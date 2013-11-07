using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpaceInvaders
{
    /// <summary>
    /// Stores and retrieves a high score from memory.
    /// </summary>
    class HighScore
    {
        /// <summary>
        /// This writes the high score to memory.
        /// </summary>
        /// <param name="score">The score</param>
        public static void WriteHighScore(ulong score)
        {
            if (RetrieveHighScore() < score)
            {
                using (StreamWriter stW = new StreamWriter(
                    new FileStream("score.txt", FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    stW.WriteLine(score);
                }
            }
        }

        /// <summary>
        /// This retrieves the high score from memory.
        /// </summary>
        /// <returns>The score</returns>
        public static ulong RetrieveHighScore()
        {
            ulong score = 0;

            using (StreamReader stR = new StreamReader(
                new FileStream("score.txt", FileMode.OpenOrCreate, FileAccess.Read)))
            {
                if (stR.Peek() > -1)
                    ulong.TryParse(stR.ReadLine(), out score);
            }

            return score;
        }
    }
}
