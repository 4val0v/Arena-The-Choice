using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class CharacterView : MonoBehaviour
{
    public GameObject[] Childs;

    public GameObject NearHandFix;

    public void HideAll()
    {
        foreach (var child in Childs)
        {
            child.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void ShowOnlyIds(IEnumerable<int> ids)
    {
        HideAll();

        gameObject.SetActive(true);

        foreach (var child in Childs)
        {
            foreach (var id in ids)
            {
                if (child.name.Equals(id.ToString()))
                {
                    child.SetActive(true);
                    break;
                }
            }
        }

        NearHandFix.SetActive(ids.Any(n => n == 5 || n == 6));
    }

    private Animator _animator;

    public void PlayAttack(bool isFar)
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _animator.SetTrigger(isFar ? "Attack1" : "AttackBoth");
    }
}
