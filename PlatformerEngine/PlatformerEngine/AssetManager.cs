﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// manages all game assets
    /// (textures, animated textures, sounds)
    /// </summary>
    public static class AssetManager
    {
        private static Dictionary<string, Texture2D> textureAssets = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Texture2D[]> framedTextureAssets = new Dictionary<string, Texture2D[]>();
        private static Dictionary<string, SoundEffect> soundAssets = new Dictionary<string, SoundEffect>();
        private static List<KeyValuePair<string, Action<Texture2D>>> textureAssetRequests = new List<KeyValuePair<string, Action<Texture2D>>>();
        private static List<KeyValuePair<string, Action<Texture2D[]>>> framedTextureAssetRequests = new List<KeyValuePair<string, Action<Texture2D[]>>>();
        private static List<KeyValuePair<string, Action<SoundEffect>>> soundAssetRequests = new List<KeyValuePair<string, Action<SoundEffect>>>();
        /// <summary>
        /// The content manager to load data from.
        /// Set this before you load with the asset manager.
        /// </summary>
        public static ContentManager Content;
        /// <summary>
        /// loads a texture of multiple frames
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <param name="frameCount">number of frames to load</param>
        /// <returns>a texture array with each loaded frame</returns>
        public static Texture2D[] LoadFramedTexture(string internalName, string location, int frameCount)
        {
            Texture2D[] frames = new Texture2D[frameCount];
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = Content.Load<Texture2D>(location + i.ToString());
            }
            foreach (KeyValuePair<string, Action<Texture2D[]>> req in framedTextureAssetRequests)
            {
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(frames);
                    framedTextureAssetRequests.Remove(req);
                }
            }
            framedTextureAssets[internalName] = frames;
            return frames;
        }
        /// <summary>
        /// loads a texture
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <returns>the loaded texture</returns>
        public static Texture2D LoadTexture(string internalName, string location)
        {
            Texture2D texture = Content.Load<Texture2D>(location);
            foreach (KeyValuePair<string, Action<Texture2D>> req in textureAssetRequests)
            {
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(texture);
                    textureAssetRequests.Remove(req);
                }
            }
            textureAssets[internalName] = texture;
            return texture;
        }
        /// <summary>
        /// loads a sound
        /// </summary>
        /// <param name="internalName">the name to call the asset in this manager</param>
        /// <param name="location">the location of the asset</param>
        /// <returns>the loaded sound</returns>
        public static SoundEffect LoadSound(string internalName, string location)
        {
            SoundEffect sound = Content.Load<SoundEffect>(location);
            foreach (KeyValuePair<string, Action<SoundEffect>> req in soundAssetRequests)
            {
                if (req.Key.Equals(internalName))
                {
                    req.Value.Invoke(sound);
                    soundAssetRequests.Remove(req);
                }
            }
            soundAssets[internalName] = sound;
            return sound;
        }
        /// <summary>
        /// requests a framed texture
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the frames</param>
        public static void RequestFramedTexture(string assetName, Action<Texture2D[]> callback)
        {
            if(framedTextureAssets.ContainsKey(assetName))
            {
                callback.Invoke(framedTextureAssets[assetName]);
            }
            else
            {
                framedTextureAssetRequests.Add(new KeyValuePair<string, Action<Texture2D[]>>(assetName, callback));
            }
        }
        /// <summary>
        /// requests a texture
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the texture</param>
        public static void RequestTexture(string assetName, Action<Texture2D> callback)
        {
            if (textureAssets.ContainsKey(assetName))
            {
                callback.Invoke(textureAssets[assetName]);
            }
            else
            {
                textureAssetRequests.Add(new KeyValuePair<string, Action<Texture2D>>(assetName, callback));
            }
        }
        /// <summary>
        /// requests a sound
        /// </summary>
        /// <param name="assetName">the name of the asset</param>
        /// <param name="callback">what to call back when we get the sound</param>
        public static void RequestSound(string assetName, Action<SoundEffect> callback)
        {
            if (soundAssets.ContainsKey(assetName))
            {
                callback.Invoke(soundAssets[assetName]);
            }
            else
            {
                soundAssetRequests.Add(new KeyValuePair<string, Action<SoundEffect>>(assetName, callback));
            }
        }
    }
}