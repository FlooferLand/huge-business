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
            if (_obj is IBaseUnlockable obj && obj == assignedObject)
            {
                obj.Unlock();
                Global.player.keys.Remove(this);
            }
        }
    }

    public class KeyPickup : Spatial, IBaseInteractable
    {
        // Export Variables
        [Export] public NodePath assignedObjectPath;

        // Variables
        public KeyObject keyObj;

        public override void _Ready()
        {
            Spatial assignedObject = GetNode<Spatial>(assignedObjectPath);

            if (assignedObject is IBaseUnlockable obj)
                keyObj = new KeyObject(obj);
            else if (assignedObject == null)
                throw new Exception("[KeyPickup] Assigned object is NULL!!!");
            else
                throw new Exception("[KeyPickup] Assigned object is not unlockable!!");
        }

        public void Interact()
        {
            Global.player.keys.Add(keyObj);
            QueueFree();
        }
    }
}