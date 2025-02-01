using Content.Shared.Damage;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery;

[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryTargetComponent : Component
{
    [DataField]
    public bool CanOperate = true;
    
    [DataField(required: true)]
    public DamageSpecifier PainDamage = default!;
}
