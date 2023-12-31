﻿using System;
using SchizoQuest.Characters;

namespace SchizoQuest.Interaction
{
    public interface ICompoundInteractable<in TOther> : ICompoundInteractable where TOther : IInteractable
    {
        bool CanCompoundInteract(Player player, TOther other);
        void CompoundInteract(Player player, TOther other);

        new void Interact(Player player) { }

        bool IInteractable.CanInteract(Player player) => true;
        void IInteractable.Interact(Player player) => Interact(player);
        bool ICompoundInteractable.CanCompoundInteract(Player player, IInteractable other) => other is TOther otherT && CanCompoundInteract(player, otherT);
        void ICompoundInteractable.CompoundInteract(Player player, IInteractable other) => CompoundInteract(player, (TOther) other);
        Type ICompoundInteractable.GetOtherType() => typeof(TOther);
    }

    public interface ICompoundInteractable : IInteractable
    {
        bool CanCompoundInteract(Player player, IInteractable other);
        void CompoundInteract(Player player, IInteractable other);
        Type GetOtherType();
    }
}
