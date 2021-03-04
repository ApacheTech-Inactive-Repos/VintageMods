using JetBrains.Annotations;
using Vintagestory.API.Common;

namespace VintageMods.Core.Fluent.Primitives
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    public abstract class FluentCommandBase<TApi> where TApi : ICoreAPI
    {
        protected FluentCommandBase(TApi api)
        {
            Api = api;
        }

        protected TApi Api { get; }

        public EnumAppSide Side => Api.Side;

        public abstract void OnUnrecognisedOption();
        public abstract void OnDefaultOption();
        public abstract string HelpText();

        public virtual void RegisterNonFluentSyntax(){}
    }
}