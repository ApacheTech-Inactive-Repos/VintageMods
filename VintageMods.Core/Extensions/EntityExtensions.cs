using System;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace VintageMods.Core.Extensions
{
    public static class EntityExtensions
    {
        public static void ApplyForce(this Entity entity, Vec3d forwardVec, double force)
        {
            entity.Pos.Motion.X *= forwardVec.X * force;
            entity.Pos.Motion.Y *= forwardVec.Y * force;
            entity.Pos.Motion.Z *= forwardVec.Z * force;
        }

        public static EntityPos LookAwayFrom(this EntityPos agentPos, Vec3d targetPos)
        {
            var cartesianCoordinates = targetPos.SubCopy(agentPos.XYZ).Normalize();
            var yaw = GameMath.TWOPI - (float) Math.Atan2(cartesianCoordinates.Z, cartesianCoordinates.X);
            var pitch = (float) Math.Asin(-cartesianCoordinates.Y);
            var entityPos = agentPos.Copy();
            entityPos.Yaw = (yaw + GameMath.PI) % GameMath.TWOPI;
            entityPos.Pitch = pitch;
            return entityPos;
        }

        public static EntityPos LookAtTarget(this EntityPos agentPos, Vec3d targetPos)
        {
            var cartesianCoordinates = targetPos.SubCopy(agentPos.XYZ).Normalize();
            var yaw = GameMath.TWOPI - (float) Math.Atan2(cartesianCoordinates.Z, cartesianCoordinates.X);
            var pitch = (float) Math.Asin(cartesianCoordinates.Y);
            var entityPos = agentPos.Copy();
            entityPos.Yaw = yaw % GameMath.TWOPI;
            entityPos.Pitch = GameMath.PI - pitch;
            return entityPos;
        }
    }
}