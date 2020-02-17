using AHWeb.Json;
using AHWeb.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace AHWeb.Db
{
    public enum TimeLeft { Long, Medium, Short, VeryLong, Unknown };

    class Auction
    {
        public long ID { get; set; }

        [JsonProperty("auc")]
        public int AuctionID { get; set; }

        [JsonProperty("item")]
        public long Item { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("ownerRealm")]
        public string OwnerRealm { get; set; }

        [JsonProperty("bid")]
        public long Bid { get; set; }

        [JsonProperty("buyout")]
        public long Buyout { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("timeLeft")]
        [JsonConverter(typeof(TimeLeftConverter))]
        public TimeLeft TimeLeft { get; set; }

        [JsonProperty("bonusLists", NullValueHandling = NullValueHandling.Ignore)]
        [NotMapped]
        public List<BonusList> BonusLists { get; set; }

        [JsonProperty("modifiers", NullValueHandling = NullValueHandling.Ignore)]
        [NotMapped]
        public List<Modifier> Modifiers { get; set; }

        [JsonProperty("petSpeciesId", NullValueHandling = NullValueHandling.Ignore)]
        public long? PetSpeciesId { get; set; }

        [JsonProperty("petBreedId", NullValueHandling = NullValueHandling.Ignore)]
        public long? PetBreedId { get; set; }

        [JsonProperty("petLevel", NullValueHandling = NullValueHandling.Ignore)]
        public long? PetLevel { get; set; }

        [JsonProperty("petQualityId", NullValueHandling = NullValueHandling.Ignore)]
        public long? PetQualityId { get; set; }

        //foriegn keys
        public long AuctionSnapID { get; set; }
        public AuctionSnap AuctionSnap { get; set; }

        public List<AuctionBonusList> AuctionBonusLists { get; set; }

        public List<AuctionModifier> AuctionModifiers { get; set; }
    }

    class BonusList : IEquatable<BonusList>
    {
        public long ID { get; set; }

        [JsonProperty("bonusListId")]
        public long BonusListId { get; set; }

        public List<AuctionBonusList> AuctionsBonusLists { get; set; }

        public bool Equals(BonusList o)
        {
            if (o == null)
                return false;
            return BonusListId == o.BonusListId;
        }
        public override bool Equals(object obj) => Equals(obj as BonusList);
        public override int GetHashCode() => (BonusListId).GetHashCode();
    }

    class Modifier : IEquatable<Modifier>
    {
        public long ID { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }

        public List<AuctionModifier> AuctionModifiers { get; set; }

        public bool Equals(Modifier o)
        {
            if (o == null)
                return false;
            return Type == o.Type && Value == o.Value;
        }
        public override int GetHashCode() => (Type, Value).GetHashCode();
        public override bool Equals(object obj) => Equals(obj as Modifier);
    }

    class AuctionBonusList
    {
        public long AuctionID { get; set; }
        public Auction Auction { get; set; }

        public long BonusListID { get; set; }
        public BonusList BonusList { get; set; }
    }

    class AuctionModifier
    {
        public long AuctionID { get; set; }
        public Auction Auction { get; set; }

        public long ModifierID { get; set; }
        public Modifier Modifier { get; set; }
    }
}
