using UnityEngine;

namespace StringToHell.InGame
{
    public class TagCheck : MonoBehaviour
    {
        public bool CheckTags(string[] targetTags, string tagToCheck )
        {
            foreach (string targetTag in targetTags)
            {
                if (tagToCheck == targetTag)
                {
                    Debug.Log("Tag matched: " + tagToCheck);
                    return true;
                }
            }
            Debug.Log("Tag not matched: " + tagToCheck);
            return false;
        }

    }
}
