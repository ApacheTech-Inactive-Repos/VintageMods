using JetBrains.Annotations;
using Vintagestory.API.Client;

namespace VintageMods.Core.Fluent.Primitives
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    public abstract class FluentClientCommand : FluentCommandBase<ICoreClientAPI>
    {
        protected FluentClientCommand(ICoreClientAPI api) : base(api)
        {
        }

        public override void OnUnrecognisedOption()
        {
            Api.ShowChatMessage(HelpText());
        }

        public override void OnDefaultOption()
        {
            Api.ShowChatMessage(HelpText());
        }
    }
}