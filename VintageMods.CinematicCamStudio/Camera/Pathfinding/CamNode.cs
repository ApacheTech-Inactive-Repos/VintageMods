using System;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

// ReSharper disable UnusedMember.Global

namespace VintageMods.CinematicCamStudio.Camera.Pathfinding
{
    public class CamNode : EntityPos
    {
        internal CamNode(double x, double y, double z, float yaw = 0f, float pitch = 0f, float roll = 0f)
         : base(x, y, z, yaw, pitch, roll)
        {
            YawOriginal = yaw;
            PitchOriginal = pitch;
        }

        #region Targetting

        private float YawOriginal { get; }
        private float PitchOriginal { get; }

        internal void SetTarget(Vec3d targetPos)
        {
            var cartesianCoordinates = XYZ.SubCopy(targetPos).Normalize();
            var p = (float)Math.Asin(cartesianCoordinates.Y);
            var ya = GameMath.TWOPI - (float)Math.Atan2(cartesianCoordinates.Z, cartesianCoordinates.X);
            Yaw = ya % GameMath.TWOPI;
            Pitch = GameMath.PI - p;
        }

        internal void ClearTarget()
        {
            Yaw = YawOriginal;
            Pitch = PitchOriginal;
        }

        #endregion


        #region Camera Effects

        internal double FieldOfView { get; set; }
        internal double Saturation { get; set;  }
        internal double Sepia { get; set; }

        // TODO: Need an easier way to hook up new fields, rather than setting manually down the chain.


        // Fog levels can be used to fade to and from black or white.
        internal double FogLevel { get; set; }


        internal double FogColourR { get; set; }
        internal double FogColourG { get; set; }
        internal double FogColourB { get; set; }


        #endregion


        #region Interpolation

        internal CamNode GetPointBetween(CamNode point, double percent)
        {
            var node = new CamNode(
                x + (point.x - x) * percent, 
                y + (point.y - y) * percent, 
                z + (point.z - z) * percent, 
                (float)(Yaw + (point.Yaw - Yaw) * percent),
                (float)(Pitch + (point.Pitch - Pitch) * percent),
                (float)(Roll + (point.Roll - Roll) * percent));
            
            node.FieldOfView += (point.FieldOfView - FieldOfView) * percent;
            node.Saturation += (point.Saturation - Saturation) * percent;
            node.Sepia += (point.Sepia - Sepia) * percent;

            return node;
        }

        internal void LookAt(Vec3d pos, float minYaw, float minPitch, double ticks)
        {
            var d0 = pos.X - x;
            var d2 = pos.Y - z;
            var d1 = pos.Z - y;

            var d3 = Math.Sqrt(d0 * d0 + d2 * d2);
            var f2 = (Math.Atan2(d2, d0) * 180.0D / Math.PI) - 90.0D;
            var f3 = (-(Math.Atan2(d1, d3) * 180.0D / Math.PI));

            Pitch = (float)UpdateRotation(Pitch, f3, minPitch, ticks);
            Yaw = (float)UpdateRotation(Yaw, f2, minYaw, ticks);
        }

        private static double UpdateRotation(double rotation, double intended, double min, double ticks)
        {
            var f3 = GameMaths.WrapDegrees(intended - rotation);

            f3 = f3 > 0 ? Math.Min(Math.Abs(f3 * ticks), f3) : Math.Max(-Math.Abs(f3 * ticks), f3);

            return rotation + f3;
        }

        internal CamNode ExtrapolateFrom(CamNode p, int direction)
        {
            var dx = p.x - x;
            var dy = p.y - y;
            var dz = p.z - z;
            var dpitch = p.pitch - pitch;
            var dyaw = p.yaw - yaw;
            var droll = p.roll - roll;

            return new CamNode(
                dx * direction, 
                dy * direction, 
                dz * direction, 
                dyaw * direction, 
                dpitch * direction, 
                droll * direction);
        }

        #endregion


        internal bool PositionEquals(CamNode node)
        {
            return Math.Abs(node.X - X) == 0f && 
                   Math.Abs(node.Y - Y) == 0f && 
                   Math.Abs(node.Z - Z) == 0f;
        }

        internal static CamNode FromEntityPos(EntityPos entityPos)
        {
            return new CamNode(entityPos.X, entityPos.Y, entityPos.Z, entityPos.Yaw, entityPos.Pitch, entityPos.Roll);
        }

        internal CamNode Clone()
        {
            return new CamNode(X, Y, Z, Yaw, Pitch, Roll)
            {
                FieldOfView = FieldOfView,
                Saturation = Saturation,
                Sepia = Sepia
            };
        }
    }
}