using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SchizoQuest.Game
{
    public class HintCollection : MonoBehaviour
    {
        public List<HintBehaviour> hints;

        public void ShowHint(HintType type)
        {
            foreach (HintBehaviour hintBehaviour in hints.Where(h => h.myType == type))
            {
                hintBehaviour.gameObject.SetActive(true);
            }
        }

        public void HideHint(HintType type)
        {
            foreach (HintBehaviour hintBehaviour in hints.Where(h => h.myType == type))
            {
                Destroy(hintBehaviour.gameObject);
            }

            hints.RemoveAll(h => h.myType == type);
        }
    }
}
