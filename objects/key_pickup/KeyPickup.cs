using Godot;
using System;

namespace Project
{
    public class KeyObject
    {
        public IBaseUnlockable assignedObject;
        public KeyObject(IBaseUnlockable obj)
        {
            assignedObject = obj;
        }

        public void TryUse(Node _obj)
        {
            if (_obj is IBaseUnlockable obj && CanUseOn(_obj))
            {
                obj.Unlock();
                Global.player.keys.Remove(this);
            }
        }

        public bool CanUseOn(Node _obj) {
            return (_obj is IBaseUnlockable obj && obj == assignedObject);
        }
    }

    public class KeyPickup : Spatial, IBaseInteractable
    {
        // Export Variables
        [Export] public NodePath paperInfoPickupPath;
        [Export] public NodePath assignedObjectPath;

        // Variables
        public KeyObject keyObj;
        public Spatial indicatorLight;
        public AudioStreamPlayer pickupSound;

        public override void _Ready()
        {
            var assignedObject = GetNode<Spatial>(assignedObjectPath);
            var paperInfoPickup = GetNode<Spatial>(paperInfoPickupPath);
            indicatorLight = GetNode<Spatial>("IndicatorLight");
            paperInfoPickup.Connect(nameof(PaperPickup.on_pickup), this, nameof(_OnPaperPickup));
            indicatorLight.Visible = false;

            pickupSound = GetNode<AudioStreamPlayer>("PickupSound");
            pickupSound.Connect("finished", this, "queue_free");

            if (assignedObject is IBaseUnlockable obj)
                keyObj = new KeyObject(obj);
            else if (assignedObject == null)
                throw new Exception("[KeyPickup] Assigned object is NULL!!!");
            else
                throw new Exception("[KeyPickup] Assigned object is not unlockable!!");
        }

        public void _OnPaperPickup() {
            indicatorLight.Visible = true;
        }

        public void Interact()
        {
            if (!keyObj.assignedObject.IsLocked())
                GD.PushError("Picked up key for a door that isn't locked!");
            Global.player.keys.Add(keyObj);
            pickupSound.Play();
            Visible = false;
        }
    }
}