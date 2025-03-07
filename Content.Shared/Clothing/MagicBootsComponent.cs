using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Clothing;


[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedMagicBootsSystem))]
public sealed partial class MagicBootsComponent : Component
{
    [DataField]
    public ProtoId<AlertPrototype> MagicBootsAlert = "Magboots";

    [DataField]
    public string Slot = "shoes";
} 