using Content.Shared.Alert;
using Content.Shared.Atmos.Components;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Gravity;
using Content.Shared.Inventory;
using Content.Shared.Item;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Content.Shared.Toggleable;
using Robust.Shared.Physics.Components;
using Robust.Shared.Timing;

namespace Content.Shared.Clothing;


public sealed class SharedMagicBootsSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly ClothingSystem _clothing = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedGravitySystem _gravity = default!;
    [Dependency] private readonly SharedItemSystem _item = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MagicBootsComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<MagicBootsComponent, ClothingGotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<MagicBootsComponent, ClothingGotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<MagicBootsComponent, IsWeightlessEvent>(OnIsWeightless);
        SubscribeLocalEvent<MagicBootsComponent, InventoryRelayedEvent<IsWeightlessEvent>>(OnIsWeightless);
    }

    private void OnStartup(Entity<MagicBootsComponent> ent, ref ComponentStartup args)
    {
    }

    private void OnGotUnequipped(Entity<MagicBootsComponent> ent, ref ClothingGotUnequippedEvent args)
    {
        if (TryComp<MovedByPressureComponent>(args.Wearer, out var moved))
            moved.Enabled = true;

        _alerts.ClearAlert(args.Wearer, ent.Comp.MagicBootsAlert);
    }

    private void OnGotEquipped(Entity<MagicBootsComponent> ent, ref ClothingGotEquippedEvent args)
    {
        UpdateMagicBootEffects(args.Wearer, ent);
    }

    private void UpdateMagicBootEffects(EntityUid user, Entity<MagicBootsComponent> ent)
    {
        if (TryComp<MovedByPressureComponent>(user, out var moved))
            moved.Enabled = false;

        _alerts.ShowAlert(user, ent.Comp.MagicBootsAlert);
    }

    private void OnIsWeightless(Entity<MagicBootsComponent> ent, ref IsWeightlessEvent args)
    {
        if (args.Handled)
            return;

        if (!_gravity.EntityOnGravitySupportingGridOrMap(ent.Owner))
            return;

        args.IsWeightless = false;
        args.Handled = true;
    }

    private void OnIsWeightless(Entity<MagicBootsComponent> ent, ref InventoryRelayedEvent<IsWeightlessEvent> args)
    {
        OnIsWeightless(ent, ref args.Args);
    }
} 