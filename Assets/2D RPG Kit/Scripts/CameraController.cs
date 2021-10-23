using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {

    [HideInInspector]
    public Transform player;

    public Tilemap tilemap;
    private Vector3 boundary1;
    private Vector3 boundary2;

    private float halfHeight;
    private float halfWidth;

    public int musicToPlay;
    private bool musicStarted;

	// Use this for initialization
	void Start () {


        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }







        //target = PlayerController.instance.transform;
        player = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        tilemap.CompressBounds();
        boundary1 = tilemap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        boundary2 = tilemap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

        PlayerController.instance.SetBounds(tilemap.localBounds.min, tilemap.localBounds.max);
	}
	
	// LateUpdate is called once per frame after Update
	void LateUpdate () {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y), transform.position.z);

        if(!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
	}
}
