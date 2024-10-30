using BarRaider.SdTools.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace ClipboardBuddy
{
    /// <summary>
    /// Library of tools used to manipulate graphics
    /// </summary>
    public static class GhostGraphicsTools
    {
        /// <summary>
        /// Generates one (or more) images where each one has a few letters drawn on them based on the parameters. You can set number of letters and number of lines per key. 
        /// Use expandToNextImage to decide if you want only one Image returned or multiple if text is too long for one key
        /// Will generate a plain background of the provided color.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentTextPosition"></param>
        /// <param name="lettersPerLine"></param>
        /// <param name="numberOfLines"></param>
        /// <param name="font"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="textColor"></param>
        /// <param name="expandToNextImage"></param>
        /// <param name="keyDrawStartingPosition"></param>
        /// <returns>A list of images generated</returns>
        public static Image[] DrawMultiLinedText(string text, int currentTextPosition, int lettersPerLine,
            int numberOfLines, Font font, Color backgroundColor, Color textColor, bool expandToNextImage,
            PointF keyDrawStartingPosition)
        {
            return DrawMultiLinedTextCommon(text, currentTextPosition, lettersPerLine, numberOfLines, font, backgroundColor, textColor, expandToNextImage, keyDrawStartingPosition);
        }

        /// <summary>
        /// Generates one (or more) images where each one has a few letters drawn on them based on the parameters. You can set number of letters and number of lines per key.
        /// Use expandToNextImage to decide if you want only one Image returned or multiple if text is too long for one key
        /// Will use the provided background image to generate the text on top of.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentTextPosition"></param>
        /// <param name="lettersPerLine"></param>
        /// <param name="numberOfLines"></param>
        /// <param name="font"></param>
        /// <param name="backgroundPath"></param>
        /// <param name="textColor"></param>
        /// <param name="expandToNextImage"></param>
        /// <param name="keyDrawStartingPosition"></param>
        /// <returns>A list of images generated.</returns>
        public static Image[] DrawMultiLinedTextWithBackground(string text, int currentTextPosition, int lettersPerLine,
            int numberOfLines, Font font, String backgroundPath, Color textColor, bool expandToNextImage,
            PointF keyDrawStartingPosition)
        {
            Image myBackground = Image.FromFile(backgroundPath);
            return DrawMultiLinedTextCommon(text, currentTextPosition, lettersPerLine, numberOfLines, font, Color.Transparent, textColor, expandToNextImage, keyDrawStartingPosition, myBackground);
        }

        /// <summary>
        /// Centralize the code to generate images with text wrapped:
        /// Generates one (or more) images where each one has a few letters drawn on them based on the parameters. You can set number of letters and number of lines per key.
        /// Use expandToNextImage to decide if you want only one Image returned or multiple if text is too long for one key
        /// Use the background image or generate a plain background of the provided color.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentTextPosition"></param>
        /// <param name="lettersPerLine"></param>
        /// <param name="numberOfLines"></param>
        /// <param name="font"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="textColor"></param>
        /// <param name="expandToNextImage"></param>
        /// <param name="keyDrawStartingPosition"></param>
        /// <param name="background"></param>
        /// <returns>A list of images generated.</returns>
        private static Image[] DrawMultiLinedTextCommon(string text, int currentTextPosition, int lettersPerLine, int numberOfLines, Font font, Color backgroundColor, Color textColor, bool expandToNextImage, PointF keyDrawStartingPosition, Image background = null)
        {
            float currentWidth = keyDrawStartingPosition.X;
            float currentHeight = keyDrawStartingPosition.Y;
            int currentLine = 0;
            List<Image> images = new List<Image>();
            Bitmap img = Tools.GenerateGenericKeyImage(out Graphics graphics);
            images.Add(img);

            if (background != null)
            {
                // Use the provided background
                graphics.DrawImage(background, 0, 0, img.Width, img.Height);
            }
            else
            {
                // Draw Background
                var bgBrush = new SolidBrush(backgroundColor);
                graphics.FillRectangle(bgBrush, 0, 0, img.Width, img.Height);
            }

            float lineHeight = img.Height / numberOfLines;
            if (numberOfLines == 1)
            {
                currentHeight = img.Height / 2; // Align to middle
            }

            float widthIncrement = img.Width / lettersPerLine;
            for (int letter = currentTextPosition; letter < text.Length; letter++)
            {
                // Check if I need to move back to the beginning of the key, but on a new line
                if (letter > currentTextPosition && letter % lettersPerLine == 0)
                {
                    currentLine++;
                    if (currentLine >= numberOfLines)
                    {
                        if (expandToNextImage)
                        {
                            images.AddRange(DrawMultiLinedTextCommon(text, letter, lettersPerLine, numberOfLines, font, backgroundColor, textColor, expandToNextImage, keyDrawStartingPosition, background));
                        }
                        break;
                    }

                    currentHeight += lineHeight;
                    currentWidth = keyDrawStartingPosition.X;
                }

                graphics.DrawString(text[letter].ToString(), font, new SolidBrush(textColor), new PointF(currentWidth, currentHeight));
                currentWidth += widthIncrement;
            }
            graphics.Dispose();
            return images.ToArray();
        }
    }
}
