using VintageMods.CinematicCamStudio.Exceptions;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace VintageMods.CinematicCamStudio.Camera.Targetting
{
    public class CamTargetEntity : CamTarget
    {
        private Entity _entity;
        private readonly long _entityId;

        protected CamTargetEntity(){}

        public CamTargetEntity(long entityId)
        {
            _entityId = entityId;
        }

        public CamTargetEntity(Entity entity)
        {
            _entity = entity;
            _entityId = entity.EntityId;
        }

        public override EnumCamTargetType TargetType { get; } = EnumCamTargetType.Entity;

        public override Vec3d GetPosition(IClientWorldAccessor world, float dt)
        {
            if (!(_entity is null))
            {
                return new Vec3d(_entity.Pos.X, _entity.LocalEyePos.Y, _entity.Pos.Z);
            }

            if (!world.LoadedEntities.ContainsKey(_entityId))
            {
                throw new CamStudioException("The specified target entity is not currently loaded. It may have de-spawned, or gone out of range.");
            }

            _entity = world.LoadedEntities[_entityId];

            return GetPosition();
        }

        public override Vec3d GetPosition()
        {
            return new Vec3d(_entity.Pos.X, _entity.LocalEyePos.Y, _entity.Pos.Z);
        }

        public CamTargetEntity(IClientWorldAccessor world)
        {
            if (!world.LoadedEntities.ContainsKey(_entityId))
            {
                throw new CamStudioException("The specified target entity is not currently loaded. It may have de-spawned, or gone out of range.");
            }

            _entity = world.LoadedEntities[_entityId];
        }
    }
}