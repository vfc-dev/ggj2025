using AudioSystem;
using HighlightPlus;
using UnityEngine;

public class IcedObject : Cleanable
{
    [SerializeField] private float size = 0.01f;
    private bool IsCleaned = false;
    HighlightEffect _highlightEffect;

    public override void Start()
    {
        if (_highlightEffect == null)
        {
            _highlightEffect = transform.parent.gameObject.AddComponent<HighlightEffect>();
        }
        dirtyMaterial = GetComponent<Renderer>().material;
    }

    public override void Clean(Vector2 textureCoord, CleaningToolSO tool)
    {
        if (tool.toolType == CleaningToolSO.ToolType.Fire)
        {
            Melt();
        }
    }

    private void Melt()
    {
        transform.localScale -= new Vector3(size, size, size);
        if (transform.localScale.x <= .4f)
        {
            Debug.Log($"The object is clear - {gameObject.name}.");
            OnFinished();
            Destroy(gameObject);
        }
    }

    private void OnFinished()
    {
        if (!IsCleaned)
        {
            if (_highlightEffect != null)
            {
                _highlightEffect.HitFX(GameMaster.Instance.colorHighlight, 1f);
            }
            SoundManager.Instance.CreateSound()
            .WithSoundData(GameMaster.Instance.cleanedSFX) //If has soundData
            .WithPosition(transform.position)  //When its attached to a especific position
            .Play();

            Destroy(_highlightEffect, 2);
            Destroy(this, 2);
            IsCleaned = true;
        }
    }
}
