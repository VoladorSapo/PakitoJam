using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class TransitionBase : MonoBehaviour
{
    [SerializeField] protected Image Image;
    protected Material MaterialInstance;
    protected bool DoingTransition;
    protected virtual void Awake()
    {
        MaterialInstance = Instantiate(Image.material);
        Image.material = MaterialInstance;
    }
    public abstract void MakeTransition(TransitionParameters parameters);
    protected abstract IEnumerator IEHandleTransition(TransitionParameters transitionData);
}