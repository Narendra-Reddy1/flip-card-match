using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace CardGame
{
    public enum CardType
    {
        NormalCard,
    }

    [CreateAssetMenu(fileName = "newSpriteDatabase", menuName = "SO/Sprite Database")]
    public class SpriteDatabase : ScriptableObject
    {
        [SerializeField] private List<Sprite> _cardSprites;

        [Space]
        [SerializeField] private SerializedDictionary<CardType, Sprite> _cardTypeToCardBackFaceIcon;


        public ReadOnlyCollection<Sprite> GetCardSprites()
        {
            return _cardSprites.AsReadOnly();
        }
        public Sprite GetCardBackFace(CardType cardType)
        {
            return _cardTypeToCardBackFaceIcon[cardType];
        }

    }
}
