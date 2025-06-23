using System;
using System.Collections;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// Categorizes items with an ID for code logic.
    /// </summary>
    public enum ItemType
    {
        None,
        Hammer,
        Tungsten_Cube,
        Cannon_Ball,
        Cargo,
        Dud_Grenade,
        Large_Explosive,
        Tungsten_Chunk
    }


    /// <summary>
    /// A type of <c>Interactable</c>. Importantly, this can be picked up by the player, and gravity affects it.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Item : Interactable
    {
        public Rigidbody rb { get; private set; }
        [SerializeField] private ItemType itemName = ItemType.None;
        public string ItemName { get { return itemName.ToString().Replace('_', ' '); } }
        public ItemType it { get => itemName; }

        [SerializeField] private float _clangVolume = 1f;
        public float ClangVolume { get { return _clangVolume; } }
        public bool CanMakeNoise = false; // prevent crazy clashing sounds from happening at game start

        protected override void Start()
        {
            if (itemName == ItemType.None) Debug.LogWarning($"Item {gameObject} has no name.");
            rb = GetComponent<Rigidbody>();
        }

        /// <returns> the message for dropping the item.</returns>
        public string DropTooltip()
        {
            return $"{Utils.getKeys(PInput, "Drop")} Drop {ItemName}";
        }

        protected override String UniqueToolTip(EquippableItem equippedItem) => ItemName;

        void OnCollisionEnter(Collision collision)
        {
            if (CanMakeNoise) SFXManager.PlaySound(SFXManager.SoundType.ITEM_CLANG, _clangVolume);
        }

        /// <summary>
        /// Disables sounds on this item after a delay
        /// </summary>
        /// <param name="waitTime">how long to wait (s) before silencing the item.</param>
        public void startSoundFallOff(float waitTime = 5f) => StartCoroutine(startSoundFallOffHelper(waitTime));

        /// <summary>
        /// Coroutine helper that waits before silencing this item.
        /// </summary>
        /// <param name="waitTime">how long to wait (s) before silencing the item.</param>
        private IEnumerator startSoundFallOffHelper(float waitTime = 5f)
        {
            yield return new WaitForSeconds(waitTime);
            CanMakeNoise = false;
        }
    }

}