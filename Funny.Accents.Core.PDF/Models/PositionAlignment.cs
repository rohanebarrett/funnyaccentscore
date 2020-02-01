using Funny.Accents.Core.Types.Enumerations;
using Funny.Accents.Core.Types.Position;
using iTextSharp.text;

namespace Funny.Accents.Core.Core.PDF.Models
{
    public class PositionAlignment : IPositionAlignment
    {
        public AlignmentValue Alignment { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float LowerX { get; set; }
        public float LowerY { get; set; }
        public float UpperX { get; set; }
        public float UpperY { get; set; }
        public int Rotation { get; set; }

        public PositionAlignment(AlignmentValue alignment = AlignmentValue.AlignLeft, float posX = 0, float posY = 0, float lowerX = 0
            , float lowerY = 0, float upperX = 0, float upperY = 0, int rotation = 0)
        {
            Alignment = alignment;
            PosX = posX;
            PosY = posY;
            LowerX = lowerX;
            LowerY = lowerY;
            UpperX = upperX;
            UpperY = upperY;
            Rotation = rotation;
        }

        public static explicit operator Rectangle(PositionAlignment positionAlignment)
        {
            var size = new Rectangle(positionAlignment.LowerX, positionAlignment.LowerY
                , positionAlignment.UpperX, positionAlignment.UpperY);
            return size;
        }/*End of PositionAlignment -> Rectangle explicit operator*/
    }/*End of PositionAlignment class*/
}/*End of Funny.Accents.Core.Core.PDF.Models namespace*/
