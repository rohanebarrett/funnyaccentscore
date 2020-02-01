using Funny.Accents.Core.Types.Enumerations;

namespace Funny.Accents.Core.Types.Position
{
    public interface IPositionAlignment
    {
        AlignmentValue Alignment { get; set; }
        float PosX { get; set; }
        float PosY { get; set; }
        float LowerX { get; set; }
        float LowerY { get; set; }
        float UpperX { get; set; }
        float UpperY { get; set; }
        int Rotation { get; set; }
    }/*End of IPositionAlignment interface*/
}/*End of Funny.Accents.Core.Types.Position namespace*/
