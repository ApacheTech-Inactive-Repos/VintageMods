using JetBrains.Annotations;
using Vintagestory.API.Server;

namespace VintageMods.Core.Fluent.Primitives
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    public abstract class FluentServerCommand : FluentCommandBase<ICoreServerAPI>
    {
        protected FluentServerCommand(ICoreServerAPI api) : base(api)
        {
        }
    }
}