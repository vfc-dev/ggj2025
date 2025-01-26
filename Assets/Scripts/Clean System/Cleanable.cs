using UnityEngine;
using HighlightPlus;
using AudioSystem;

//using Color = UnityEngine.Color;
public class Cleanable : MonoBehaviour
{
    public Material dirtyMaterial;
    public float dirtLevel = 1f; // 1 = fully dirty, 0 = clean
    private Vector2Int lastPaintPixelPosition;
    private float dirtAmountTotal;
    private float dirtAmount;
    public float proportionDirt = .2f;

    private Texture2D dirtMask;

    private bool highlighted = false;

    //Feedback
    private bool cleaned = false;
    HighlightEffect highlightEffect;
    //private Color colorHighlight = new Color(0f,0.7f,1f,0.7f);

    public virtual void Start()
    {
        dirtyMaterial = GetComponent<Renderer>().material;
        dirtMask = new Texture2D(512, 512);
        Texture2D dirtyTexture = (Texture2D)dirtyMaterial.GetTexture("_MainTex");

        for (int y = 0; y < dirtMask.height; y++)
        {
            for (int x = 0; x < dirtMask.width; x++)
            {

                if (dirtyTexture.isReadable && dirtyTexture.GetPixel(x, y).a > 0.01f)
                {
                    dirtMask.SetPixel(x, y, Color.green); // Fully dirty
                }
                else if (dirtyTexture.isReadable && dirtyTexture.GetPixel(x, y).a <= 0.01f)
                {
                    dirtMask.SetPixel(x, y, Color.black); // Fully clean
                }
                else
                {
                    dirtMask.SetPixel(x, y, Color.green); // Fully dirty
                }
            }
        }
        dirtMask.Apply();
        dirtyMaterial.SetTexture("_Mask", dirtMask);

        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMask.width; x++)
        {
            for (int y = 0; y < dirtMask.height; y++)
            {
                //dirtAmountTotal += ((Texture2D)dirtyMaterial.GetTexture("_Mask")).GetPixel(x, y).g;
                dirtAmountTotal += dirtMask.GetPixel(x, y).g;
            }
        }
        dirtAmount = dirtAmountTotal;

        if(highlightEffect == null)
        {
            highlightEffect = gameObject.AddComponent<HighlightEffect>();
        }

       // audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            highlighted = !highlighted;
            dirtyMaterial.SetFloat("_HighlightProgress", highlighted ? 1 : 0);
        }
    }

    public virtual void Clean ( Vector2 textureCoord, CleaningToolSO tool)
    {
        if(tool.toolType != CleaningToolSO.ToolType.Basic && !gameObject.CompareTag("Ice"))
        {
            return;
        }
        int pixelX = (int)(textureCoord.x * dirtMask.width);
        int pixelY = (int)(textureCoord.y * dirtMask.height);
        Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
        //Debug.Log("UV: " + textureCoord + "; Pixels: " + paintPixelPosition);

        int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
        int maxPaintDistance = 7;
        if (paintPixelDistance < maxPaintDistance)
        {
            // Painting too close to last position
            return;
        }
        lastPaintPixelPosition = paintPixelPosition;

        int pixelXOffset = pixelX - (tool.dirtBrush.width / 2);
        int pixelYOffset = pixelY - (tool.dirtBrush.height / 2);

        for (int x = 0; x < tool.dirtBrush.width; x++)
        {
            for (int y = 0; y < tool.dirtBrush.height; y++)
            {
                Color pixelDirt = tool.dirtBrush.GetPixel(x, y);
                Color pixelDirtMask = dirtMask.GetPixel(pixelXOffset + x, pixelYOffset + y);

                float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                dirtAmount -= removedAmount;

                dirtMask.SetPixel(
                    pixelXOffset + x,
                    pixelYOffset + y,
                    new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                );
            }
        }

        dirtMask.SetPixel(pixelX, pixelY, Color.black);
        dirtMask.Apply();
        dirtyMaterial.SetTexture("_Mask", dirtMask);

        if (dirtAmount / dirtAmountTotal <= proportionDirt)
        {
            
            CleanUp();
            //highlightEffect.HitFX(Color.cyan, .5f, 2f);
            //Destroy(gameObject);
        }
    }

    private void CleanUp()
    {
        for (int y = 0; y < dirtMask.height; y++)
        {
            for (int x = 0; x < dirtMask.width; x++)
            {
                dirtMask.SetPixel(x, y, Color.black);

            }
        }
        dirtMask.Apply();
        dirtyMaterial.SetTexture("_Mask", dirtMask);

        /*
        if(audioSource != null) {
            audioSource.Play();
        }
        */
        if (!cleaned)
        {
            // Cleaned
            Debug.Log("Cleaned");
            if (highlightEffect != null)
            {
                highlightEffect.HitFX(GameMaster.Instance.colorHighlight, 1f);
            }
            SoundManager.Instance.CreateSound()
            .WithSoundData(GameMaster.Instance.cleanedSFX) //If has soundData
            .WithPosition(transform.position)  //When its attached to a especific position
            .Play();

            Destroy(highlightEffect, 2);
            Destroy(this, 2);
            cleaned = true;
        }
    }

    [ContextMenu("Get Dirt Amount")]
    private float GetDirtAmount()
    {
        Debug.Log($"Dirt Amount: {dirtAmount / dirtAmountTotal}");
        return this.dirtAmount / dirtAmountTotal;
    }

    [ContextMenu("Get Proportion Dirt")]
    private float GetProportionDirt()
    {
        Texture2D tex = (Texture2D)dirtyMaterial.GetTexture("_MainTex");
        Color[] pixels = tex.GetPixels(); // 2048x2048 = 4,194,304 pixels
        int totalPixels = pixels.Length;

        // Count non-transparent pixels
        int nonTransparentCount = 0;
        foreach (Color pixel in pixels)
        {
            if (pixel.a > 0.01f) // Check if the alpha is above the threshold
            {
                nonTransparentCount++;
            }
        }
        Debug.Log($"Dirt Proportion: {(float)nonTransparentCount / totalPixels}");
        // Calculate and return the proportion of non-transparent pixels
        return (float)nonTransparentCount / totalPixels;
    }
}
