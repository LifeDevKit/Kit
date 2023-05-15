using System.Collections;
using System.Collections.Generic;
using Kit;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;

namespace System
{
    
    public class KitRuntimeAtlas
    {
        public int atlasSize = 2048;
        public List<string> spriteUrls;
        public Dictionary<string, Rect> spriteUvRects;
        private Texture2D runtimeAtlas;

        
        IEnumerator DownloadAndAddSprites(List<string> urls)
        {
            
            foreach (string url in urls)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    AddSpriteToAtlas(texture, url);
                }
            }
        } 
        
        private void AddSpriteToAtlas(Texture2D spriteTexture, string spriteUrl)
        {
            if (spriteTexture == null)
            {
                Debug.LogError("Texture is null, cannot add to atlas.");
                return;
            }
 
            // altas size (2048) 과 width를 나누어 현재 row 인덱스를 구한다.
            int spritesPerRow = atlasSize / spriteTexture.width;
            int spriteIndex = spriteUvRects.Count; 
            int xPos = (spriteIndex % spritesPerRow) * spriteTexture.width;
            int yPos = (spriteIndex / spritesPerRow) * spriteTexture.height;

            // Add the sprite to the atlas
            Graphics.CopyTexture(spriteTexture, 0, 0, 0, 0, spriteTexture.width, spriteTexture.height, runtimeAtlas, 0, 0, xPos, yPos);

            // Save the sprite's UV coordinates within the atlas
            Rect uvRect = new Rect((float)xPos / atlasSize, (float)yPos / atlasSize, (float)spriteTexture.width / atlasSize, (float)spriteTexture.height / atlasSize);
            spriteUvRects.Add(spriteUrl, uvRect);
        }
        
        public Texture2D GetAtlas()
        {
            return runtimeAtlas;
        }

        public Rect GetSpriteUvRect(string url)
        {
            if (spriteUvRects.ContainsKey(url))
            {
                return spriteUvRects[url];
            }
            return new Rect(0, 0, 0, 0);
        } 
    }
    public class KitAtlasKitManager : SingletonBehaviour<KitAtlasKitManager>, IKitManager
    {
        private Dictionary<string, SpriteAtlas> loadedSpriteAtlas;  
        private Dictionary<string, System.Action<SpriteAtlas>> loadedSpriteBinderCallback;


        public override void Awake()
        {
            base.Awake();
            this.Initialize();
        }

        public bool Initialized { get; set; }

        public void Initialize()
        {
            Initialized = true;
        }
        
        void OnEnable()
        {
            SpriteAtlasManager.atlasRequested += RequestAtlas; 
            SpriteAtlasManager.atlasRegistered += AtlasRegistered; 
        }

        void OnDisable()
        {
            SpriteAtlasManager.atlasRequested -= RequestAtlas;
            SpriteAtlasManager.atlasRegistered -= AtlasRegistered;
        }

        public void AddSpriteToRuntimeAtlas(Texture2D runtimeAtlas)
        {
            
        }
         
        
        /// <summary>
        /// 아틀라스 정보요청
        /// </summary> 
        public virtual void RequestAtlas(string tag, System.Action<SpriteAtlas> callback)
        {
            Debug.Log($"{tag} 아틀라스가 요청되었습니다.");
            loadedSpriteBinderCallback.TryAdd(tag, callback);  
            
        }
        /// <summary>
        /// 아틀라스가 등록됨
        /// </summary> 
        public virtual void AtlasRegistered(SpriteAtlas spriteAtlas)
        {
            Debug.Log("Sprite Atlas가 등록되었습니다. " + spriteAtlas.name);
            if (!loadedSpriteAtlas.ContainsKey(tag)) 
                loadedSpriteAtlas.Add(spriteAtlas.tag, spriteAtlas);  
        }

 
    }
}